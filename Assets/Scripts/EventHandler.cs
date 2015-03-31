using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler {
    public static GameObject catBeingDragged;
    private Vector3 startPosition;
    private Vector3 screenPoint;
    private Vector3 offset;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        catBeingDragged = gameObject;
        startPosition = this.transform.position;
        //
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}
