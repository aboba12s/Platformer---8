using System;

[Serializable]
public struct DamageInfo
{
    public float damage;

}

public interface IDamageble
{
    public void TakeDamage(DamageInfo damageInfo);
} 