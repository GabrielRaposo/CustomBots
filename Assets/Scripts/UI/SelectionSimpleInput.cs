using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionSimpleInput : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public UnityEvent selectEvent;
    public UnityEvent deselectEvent;

    public void OnSelect(BaseEventData eventData)
    {
        selectEvent.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        deselectEvent.Invoke();
    }
}
