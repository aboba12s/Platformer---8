using System;
using System.Linq;
using UnityEngine;

[Serializable]
public enum TriggerType
{
    Enter,
    Exit,
    Interactive
}

[RequireComponent(typeof(Collider2D))]
public abstract class Trigger : MonoBehaviour, IInteractive
{
    [Header("Trigger")]
    [SerializeField] private TriggerType type;
    [SerializeField] private int priority = -1;
    [SerializeField] private string[] tags = { "Player" };
    [SerializeField] private bool once;
    
    private bool _done;

    public int Priority => priority;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!tags.Contains(other.tag)) return;
        if (type != TriggerType.Enter || _done) return;
        _done = once;
        Activate(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!tags.Contains(other.tag)) return;
        if (type != TriggerType.Exit || _done) return;
        _done = once;
        Activate(other);
    }
    
    public abstract void Activate(Collider2D other);
    
    public void Interact(Interactor instigator)
    {
        if (!CanInteractWith(instigator)) return;
        _done = once;
        Activate(instigator.GetComponent<Collider2D>());
    }

    public bool CanInteractWith(Interactor instigator)
    {
        return tags.Contains(instigator.tag) 
               && type == TriggerType.Interactive 
               && !_done;
    }
}

public class DamageTrigger : Trigger
{
    [SerializeField] private DamageInfo Damage;

    public override void Activate(Collider2D other)
    {
        var damagable = other.GetComponent<IDamageble>();
        damagable?.TakeDamage(Damage);
    }
}