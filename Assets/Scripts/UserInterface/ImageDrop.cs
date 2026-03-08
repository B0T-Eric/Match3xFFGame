using UnityEngine;
using UnityEngine.EventSystems;

public class ImageDrop : MonoBehaviour, IDropHandler
{
    public DraggableImage Occupant;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableImage incoming = eventData.pointerDrag?.GetComponent<DraggableImage>();
        if (incoming == null)
        {
            return;
        }
        if (Occupant == null)
        {
            Place(incoming);
        }
        else
        {
            DraggableImage outgoing = Occupant;
            ImageDrop fromSlot = incoming.CurrentSlot;
            outgoing.CurrentSlot = null;
            Occupant = null;
            Place(incoming);
            if (fromSlot != null)
            {
                fromSlot.Place(outgoing);
            }
        }
        incoming.MarkDropped();
    }

    public void Place(DraggableImage image)
    { 
        if (image.CurrentSlot != null)
        {
            image.CurrentSlot.Occupant = null;
        }
        Occupant = image;
        image.CurrentSlot = this;
        image.transform.SetParent(transform);
        image.transform.localPosition = Vector3.zero;
    }
}
