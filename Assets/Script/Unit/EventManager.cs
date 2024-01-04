using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum GameEvents
{
    CallSave,
    Save,
    Load,
    GameStop,
    GameEnd,
}
public class EventManager<T> : Singleton<EventManager<T>> where T : Enum
{
    Dictionary<T, Action<T, Component, object>> Listeners = new Dictionary<T, Action<T, Component, object>>();
    public void AddListener(T eventType, Action<T, Component, object> listener)
    {
        if (Listeners.ContainsKey(eventType))
            Listeners[eventType] += listener;
        else
        {
            Action<T, Component, object> action = null;
            Listeners.Add(eventType, action);
            Listeners[eventType] += listener;
        }
    }
    public void RemoveEvent(T eventType, Action<T, Component, object> listener)
    {
        Listeners.Remove(eventType);
    }
    public void PostEvent(T eventType, Component sender, object param)
    {
        if (Listeners.ContainsKey(eventType))
        {
            Debug.Log("알리기1");
            Listeners[eventType](eventType, sender, param);
            Debug.Log("알리기2");
        }
    }
}
