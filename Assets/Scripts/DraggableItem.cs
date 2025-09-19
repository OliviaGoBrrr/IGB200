using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

[RequireComponent(typeof(Image))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameManager gameManager;
    private GameObject draggingIcon;
    private RectTransform iconTransform;
    private Image draggableStaticIcon;

    [Header("Action Stats")]
    public GameTile.TileStates changeState;
    public int itemUses;
    private TMP_Text itemUsesText;

    [Header("Item Disable Values")]
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

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(iconTransform, data.position,
            data.pressEventCamera, out globalMousePos))
        { 
            rt.position = globalMousePos;
            rt.rotation = iconTransform.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        if(itemDisabled) { return; }

        if (draggingIcon != null) { Destroy(draggingIcon); }

        gameManager.PlayerActionTaken(changeState, this);
        UpdateItemUIText();
        gameManager.sceneAudio.PlayGameSound(dragAudio, intensity);

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
    }

    public void UpdateItemUIText()
    {
        if(itemUsesText != null)
        {
            itemUsesText.SetText($"Uses: {itemUses}");
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
    