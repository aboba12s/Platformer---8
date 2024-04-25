using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum SwitchType
{
    Once,
    Loop,
    PingPong
}

public class SwitchTrigger : Trigger
{
    [Header("Switch")] 
    [SerializeField] private SwitchType switchType = SwitchType.Loop;
    [SerializeField] private int state;
    [SerializeField] private UnityEvent[] events;

    private int State => switchType switch
    {
        SwitchType.Once => state,
        SwitchType.Loop => state % events.Length,
        SwitchType.PingPong => (int)Mathf.PingPong(state, events.Length - 1),
        _ => throw new ArgumentOutOfRangeException()
    };

    public override void Activate(Collider2D other)
    {
        if (State >= events.Length) return;
        events[State].Invoke();
        state++;
    }
}