using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionHorizontalInput : MonoBehaviour, IMoveHandler, ISubmitHandler
{
    public UnityEvent leftEvent;
    public UnityEvent rightEvent;
    public UnityEvent submitEvent;

    public void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Left:
                leftEvent.Invoke();
                break;

            case MoveDirection.Right:
                rightEvent.Invoke();
                break;

        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        submitEvent.Invoke();
    }
}
