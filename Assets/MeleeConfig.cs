using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackInfo
{
    [SerializeField, Min(0f)] private float range;
    [SerializeField, Min(0f)] private float preparatoion;
    [SerializeField, Min(0f)] private float execution;
    [SerializeField, Min(0f)] private float cooldown;

    public float Range => range;
    public float Preparatoion => preparatoion;
    public float Execution => execution;
    public float Cooldown => cooldown;
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Melee")]
public class MeleeConfig : ScriptableObject
{
   

    [Header("Wearpon")]
    [SerializeField] private DamageInfo damage;
    

    [Header("Combo")]
    [SerializeField] private float comboCooldown;
    [SerializeField] private AttackInfo[] attacks;

    public DamageInfo Damage => damage;

    public float ComboCooldown => comboCooldown;

    public AttackInfo[] Attacks => attacks;
}

public class MeleeWeapon : MonoBehaviour
{
    public enum WeaponState
    {
        Idle,
        Preparation,
        Execution,
        Cooldown
    }

    [Header("Melee")]
    [SerializeField] private MeleeConfig config;


    [Header("Attack")]
    [SerializeField] private Transform attackCenter;
    [SerializeField] private LayerMask layers;


    private bool pendingAttack;
    private int comboCounter;

    private WeaponState state;

    public int ComboCounter
    {

        get => comboCounter;
        private set => comboCounter = value % config.Attacks.Length;


    }

    public bool Isattacking => state > WeaponState.Idle;


    public delegate void OnAttack(int counter);
    public event OnAttack onAttack;


    public void Attack()
    {
        if (pendingAttack) return;
        pendingAttack = true;
        if (state > WeaponState.Idle) return;
        StartCoroutine(PerformAttack());

    }
    private IEnumerator PerformAttack()
    {
        if (Isattacking) yield break;

        var attack = config.Attacks[ComboCounter];


        onAttack?.Invoke(comboCounter);
        state = WeaponState.Preparation;
        yield return new WaitForSeconds(attack.Preparatoion);

        pendingAttack = false;
        state = WeaponState.Execution;
        StartCoroutine(DealDamage(attack.Range));
        yield return new WaitForSeconds(attack.Execution);


        state = WeaponState.Cooldown;
        yield return new WaitForSeconds(attack.Cooldown);

        ComboCounter++;

        state = WeaponState.Idle;

        if (pendingAttack)
        {
            StartCoroutine(PerformAttack());
            yield break;

        }


        yield return new WaitForSeconds(config.ComboCooldown);

        if (!Isattacking) ComboCounter = 0;
    }

    private IEnumerator DealDamage(float range)
    {
        while (state == WeaponState.Execution)
        {
            var results = Physics2D.CircleCastAll(attackCenter.position, range, transform.right, 0f, layers);

            foreach (var result in results)
            {
                var damagable = result.collider.GetComponent<IDamageble>();
                damagable?.TakeDamage(config.Damage);
            }



            yield return new WaitForFixedUpdate();
        }
    }
}
