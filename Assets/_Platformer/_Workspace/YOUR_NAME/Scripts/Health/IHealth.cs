public interface IHealth
{
    public float Max { get; }
    public float Ratio { get; }
    public bool IsAlive { get; }

    public event OnDamage onDamage;
    public event OnDamage onDeath;

    public bool CanBeDamaged();
    public float TakeDamage(DamageInfo damageInfo);
}
