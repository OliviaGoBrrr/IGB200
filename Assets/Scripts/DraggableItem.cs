using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityEditor.Progress;

[RequireComponent(typeof(Image))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameManager gameManager;
    private GameObject draggingIcon;
    private RectTransform iconTransform;
    private Image draggableStaticIcon;

    private GameScreen UIToolkitGameScript;

    [Header("Action Stats")]
    public GameTile.TileStates changeState;
    public int itemUses;
    private TMP_Text itemUsesText;

    [Header("Item Values")]
    [SerializeField] private float dragIconScale = 0.7f;
    [SerializeField] private float disableAlpha = 50f;
    [SerializeField] private bool itemDisabled = false;

    [Header("Action Audio")]
    public AudioClip dragAudio;
    public float intensity;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        draggableStaticIcon = GetComponent<Image>();
        itemUsesText = GetComponentInChildren<TMP_Text>();

        UIToolkitGameScript = FindFirstObjectByType<GameScreen>();

        UpdateItemUIText();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Check if we clicked something that can be dragged
        var canvas = FindInParents<Canvas>(gameObject);
        if(canvas == null) { return; }

        if (!itemDisabled)
        {
            draggingIcon = new GameObject("icon");

            draggingIcon.transform.SetParent(canvas.transform, false);
            draggingIcon.transform.SetAsLastSibling();

            var image = draggingIcon.AddComponent<Image>();

            image.sprite = draggableStaticIcon.sprite;

            iconTransform = canvas.transform as RectTransform;

            SetDraggedPosition(eventData);
        }
    }

    private void Update()
    {
        if (gameManager.draggableSelected)
        {
            if(draggingIcon != null)
            {
                Vector3 mouseScreenPositon = Input.mousePosition;

                RectTransform canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
                Vector3 localPoint;

                var rt = draggingIcon.GetComponent<RectTransform>();
                draggingIcon.GetComponent<Image>().color = draggableStaticIcon.color;
                    //new Color(draggableStaticIcon.color.r, draggableStaticIcon.color.g, draggableStaticIcon.color.b, 1.0f);


                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, mouseScreenPositon, null, out localPoint))
                {
                    rt.position = localPoint;
                    rt.rotation = iconTransform.rotation;
                    rt.localScale = new Vector3(dragIconScale, dragIconScale, 1f);
                }
            }
        }

        if(gameManager.draggableSelected && Input.GetMouseButtonDown(0))
        {
            gameManager.draggableSelected = false;
            if (gameManager.selectedDraggable.draggingIcon != null) { Destroy(gameManager.selectedDraggable.draggingIcon); }

            gameManager.PlayerActionTaken(gameManager.selectedDraggable.changeState, gameManager.selectedDraggable);
            UpdateItemUIText();
            FindAnyObjectByType<SceneAudio>().PlayGameSound(dragAudio, intensity);

            if (itemUses <= 0)
            {
                DisableDraggable();
            }
        }

        if (itemUses <= 0)
        {
            DisableDraggable();
        }
        else
        {
            EnableDraggable();
        }

        UpdateItemUIText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if we clicked something that can be dragged
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null) { return; }

        if (!itemDisabled)
        {
            if (!gameManager.draggableSelected)
            {
                gameManager.draggableSelected = true;
                gameManager.selectedDraggable = this;
                draggingIcon = new GameObject("icon");
                draggingIcon.transform.SetParent(canvas.transform, false);
                draggingIcon.transform.SetAsLastSibling();

                var image = draggingIcon.AddComponent<Image>();

                image.sprite = draggableStaticIcon.sprite;

                iconTransform = canvas.transform as RectTransform;

                iconTransform.localScale = new Vector3(dragIconScale, dragIconScale, 1f);
            }
        }
    }

    private void OnValidate()
    {
        // All of this is called IN EDITOR ONLY
        itemUsesText = GetComponentInChildren<TMP_Text>();
        draggableStaticIcon = GetComponent<Image>();

        if (itemUses <= 0)
        {
            DisableDraggable();
        }
        else
        {
            EnableDraggable();
        }

        UpdateItemUIText();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(draggingIcon != null)
        {
            SetDraggedPosition(eventData);
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        var rt = draggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        draggingIcon.GetComponent<Image>().color = draggableStaticIcon.color;
            //new Color(draggableStaticIcon.color.r, draggableStaticIcon.color.g, draggableStaticIcon.color.b, 1.0f);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(iconTransform, data.position,
            data.pressEventCamera, out globalMousePos))
        { 
            rt.position = globalMousePos;
            rt.rotation = iconTransform.rotation;
            rt.localScale = new Vector3(dragIconScale, dragIconScale, 1f);
        }
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        if(itemDisabled) { return; }

        if (draggingIcon != null) { Destroy(draggingIcon); }

        gameManager.PlayerActionTaken(changeState, this);
        UpdateItemUIText();
        FindAnyObjectByType<SceneAudio>().PlayGameSound(dragAudio, intensity);

        if(itemUses <= 0)
        {
            DisableDraggable();
        }
    }

    /// <summary>
    /// Disables UI draggable item from being interacted with.
    /// </summary>
    public void DisableDraggable()
    {
        itemDisabled = true;
        itemUses = 0;
        // Set icon alpha to show its disabled
        Color disableColor = draggableStaticIcon.color;
        disableColor = new Color(draggableStaticIcon.color.r, draggableStaticIcon.color.g, draggableStaticIcon.color.b, disableAlpha / 255f);
        draggableStaticIcon.color = disableColor;
    }


    /// <summary>
    /// Enables UI draggable item so it can be interacted with. Adds additional item uses on re-enabling.
    /// </summary>
    public void EnableDraggable(int addItemUses = 0)
    {
        // Re-enable item
        itemDisabled = false;
        draggableStaticIcon.color = new Color(draggableStaticIcon.color.r, draggableStaticIcon.color.g, draggableStaticIcon.color.b, 1.0f);
        itemUses += addItemUses;
        UpdateItemUIText();
    }

    public void UpdateItemUIText()
    {
        if(itemUsesText != null)
        {
            itemUsesText.SetText($"x{itemUses}");
            UIToolkitGameScript.UpdateAllText();
        }
        else
        {
            Debug.LogError($"Item: {name} does not have an Action Cost UI object. Please attach an object with a TMP Text component to {name}.");
        }
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) { return null; }

        var component = go.GetComponent<T>();

        if(component != null) {  return component; }

        Transform t = go.transform.parent;
        
        while (t != null && component == null)
        {
            component = t.gameObject.GetComponent<T>();
            t= t.parent;
        }
        return component;
    }
}
    