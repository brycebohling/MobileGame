using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public List<UnityEvent> unityEvents = new();
    

    public void Event(int eventIndex)
    {
        unityEvents[eventIndex]?.Invoke();
    }
}
