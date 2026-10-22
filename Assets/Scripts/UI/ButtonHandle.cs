using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandle : MonoBehaviour, IButtonFunctions
{
    [SerializeField]
    protected Button button;

    [SerializeField]
    private EventTrigger eventTrigger;

    protected virtual void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        if(eventTrigger == null)
        {
            eventTrigger = GetComponent<EventTrigger>();
        }
    }

    protected void AddEventTrigger(EventTriggerType eventTriggerType,UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener((data) => {action((BaseEventData)data);});
        eventTrigger.triggers.Add(entry);
    }

    protected void AddEventTrigger(EventTriggerType eventTriggerType,UnityAction<AxisEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener((data) => {action((AxisEventData)data);});
        eventTrigger.triggers.Add(entry);
    }

    protected void AddEventTrigger(EventTriggerType eventTriggerType,UnityAction<PointerEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener((data) => {action((PointerEventData)data);});
        eventTrigger.triggers.Add(entry);
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void RemoveListener(UnityAction action)
    {
        button.onClick.RemoveListener(action);
    }

    public void RemoveAllListeners()
    {
        button.onClick.RemoveAllListeners();
    }

    public virtual void OnSelect(BaseEventData baseEventData)
    {
        Debug.Log("Button Selected");
    }

    public virtual void OnSubmit(BaseEventData baseEventData)
    {
        Debug.Log("Button Submit");
    }
    public virtual void OnDeselect(BaseEventData baseEventData)
    {
        Debug.Log("Button Deselected");
    }

    public virtual void OnMove(AxisEventData axisEventData)
    {
        Debug.Log("Button Move");
    }

    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Button Pointer Click");
    }

    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("Button Pointer Down");
    }

    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log("Button Pointer Up");
    }

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log("Button Pointer Enter");
    }

    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        Debug.Log("Button Pointer Exit");
    }
}
