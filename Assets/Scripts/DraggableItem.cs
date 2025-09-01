using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggingIcon;
    private RectTransform iconTransform;
    public int actionCost;
    public GameTile.TileStates changeState;
    private GameManager gameManager;

    void Start()
    {
       gameManager = FindFirstObjectByType<GameManager>();
       TMP_Text actionCostText = GetComponentInChildren<TMP_Text>();
       actionCostText.SetText("Cost: " + actionCost);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Check if we clicked something that can be dragged
        var canvas = FindInParents<Canvas>(gameObject);
        if(canvas == null) { return; }

        draggingIcon = new GameObject("icon");

        draggingIcon.transform.SetParent(canvas.transform, false);
        draggingIcon.transform.SetAsLastSibling();

        var image = draggingIcon.AddComponent<Image>();

        image.sprite = GetComponent<Image>().sprite;

        iconTransform = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);
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
        if (draggingIcon != null) { Destroy(draggingIcon); }

        gameManager.PlayerActionTaken(changeState, actionCost);
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
    