using System;
using UnityEngine;

public abstract class HealthDecorator : IHealth
{
    private IHealth _health;

    public virtual float Max => _health.Max;

    public virtual float Ratio => _health.Ratio;

    public virtual bool IsAlive => _health.IsAlive;

    public virtual event OnDamage onDamage
    {
        add
        {
            _health.onDamage += value;
        }

        remove
        {
            _health.onDamage -= value;
        }
    }

    public virtual event OnDamage onDeath
    {
        add
        {
            _health.onDeath += value;
        }

        remove
        {
            _health.onDeath -= value;
        }
    }

    public virtual bool CanBeDamaged()
    {
        return _health.CanBeDamaged();
    }

    public virtual float TakeDamage(DamageInfo damageInfo)
    {
        return _health.TakeDamage(damageInfo);
    }
    public virtual IHealth Assign(IHealth health)
    {
        _health = health;
        return health;
    }
}

[Serializable]
public class DamageCapDecorator : HealthDecorator
{
    [SerializeField] private float maxDamage;

    public override float TakeDamage(DamageInfo damageInfo)
    {
        damageInfo.damage = Mathf.Min(damageInfo.damage, maxDamage);
        return base.TakeDamage(damageInfo);
    }
}