using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ImageDrop CurrentSlot { get; set; }
    private Image image;
    private Canvas canvas;
    public ImageDrop startSlot;
    public int charIndex;
    bool droppedOnSlot;
    void Awake()
    {
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startSlot = CurrentSlot;
        droppedOnSlot = false;
        image.color = new Color(1, 1, 1, 0.5f);
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    } 
    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = Color.white;
        image.raycastTarget = true;
        if (!droppedOnSlot && startSlot != null)
        {
            startSlot.Place(this);
        }
    }
    public void MarkDropped()
    {
        droppedOnSlot = true;
    }
}
