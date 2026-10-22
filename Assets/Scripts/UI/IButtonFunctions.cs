using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IButtonFunctions 
{
    public abstract void AddListener(UnityAction action);

    public abstract void RemoveListener(UnityAction action);

    public abstract void RemoveAllListeners();
    public void OnSelect(BaseEventData baseEventData)
    {
        
    }

    public void OnSubmit(BaseEventData baseEventData)
    {
        
    }
    public void OnDeselect(BaseEventData baseEventData)
    {
        
    }

    public void OnMove(AxisEventData axisEventData)
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        
    }
}
