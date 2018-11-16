using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionSimpleInput : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler
{
    public UnityEvent selectEvent;
    public UnityEvent deselectEvent;
    public UnityEvent submitEvent;

    public void OnSelect(BaseEventData eventData)
    {
        selectEvent.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        deselectEvent.Invoke();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        submitEvent.Invoke();
    }
}
