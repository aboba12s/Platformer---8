using UnityEngine;

public delegate void OnDamage(Health health, DamageInfo damageInfo);

public class Health : MonoBehaviour, IHealth
{
    [SerializeField, Min(0f)] private float max = 100f;
    [SerializeField, Min(0f)] private float actual = 100f;

    public float Max => max;

    public float Ratio => actual/max;

    public bool IsAlive => actual > 0f;

    public event OnDamage onDamage;
    public event OnDamage onDeath;

    private void Start()
    {
        actual = Mathf.Min(actual, max);
    }

    private void SetHealth(float value)
    {
        actual = Mathf.Clamp(value, 0f, max);
    }

    public bool CanBeDamaged()
    {
        return IsAlive;
    }

    public float TakeDamage(DamageInfo damageInfo)
    {
        if (!CanBeDamaged()) return 0f;
        if (damageInfo.damage < 0f) return 0f;

        var oldActual = actual;
        SetHealth(actual - damageInfo.damage);

        onDamage?.Invoke(this, damageInfo);
        if (IsAlive) onDeath?.Invoke(this, damageInfo);

        return Mathf.Abs(oldActual - actual);
    }
}
