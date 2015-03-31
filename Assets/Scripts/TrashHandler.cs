using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TrashHandler : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData)
    {
        GameObject.Destroy(EventHandler.catBeingDragged);
    }
}
