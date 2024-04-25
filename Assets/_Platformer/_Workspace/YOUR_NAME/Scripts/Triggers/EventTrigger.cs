using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : Trigger
{
    [Header("Event")]
    [SerializeField] private UnityEvent onEvent;

    public override void Activate(Collider2D other)
    {
        onEvent?.Invoke();
    }
}