using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class InGameButtonsHandle : ButtonHandle
{
    public UnityEvent onClickButtonStart;
    public UnityEvent onClickButtonCancelled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        AddEventTrigger(EventTriggerType.PointerEnter,OnPointerEnter);
        AddEventTrigger(EventTriggerType.PointerExit,OnPointerExit);
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        base.OnPointerEnter(pointerEventData);
        onClickButtonStart?.Invoke();
    }

    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        base.OnPointerExit(pointerEventData);
        onClickButtonCancelled?.Invoke();
    }
}
