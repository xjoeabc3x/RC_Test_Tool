using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MySlider : Slider
{
    public delegate void EventType_OnPointUp();
    public event EventType_OnPointUp onPointUpEvent;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointUpEvent();
    }
}
