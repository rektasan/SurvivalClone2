using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float atkRange;
    [SerializeField] private int defaultDmg;
    [SerializeField] private LayerMask enemyLayer;
    private int curDmg;

    public bool IsAttacking => IsAttacking;
    private bool isAttacking;

    private bool wasDamaged;
    private float timer;

    private const float attackAnimTimerBefore = 0.14f;
    private const float attackAnimTimerFull = 0.95f;

    private bool hasWeapon;
    private const float attackAnimTimerBeforeW = 0.6f;
    private const float attackAnimTimerFullW = 1.45f;

    private PlayerAnimations anim;

    private float animTimeBefore
    {
        get
        {
            if (!hasWeapon)
            {
                return attackAnimTimerBefore;
            }
            else
            {
                return attackAnimTimerBeforeW;
            }
        }
    }

    private float animTimeFull
    {
        get
        {
            if (!hasWeapon)
            {
                return attackAnimTimerFull;
            }
            else
            {
                return attackAnimTimerFullW;
            }
        }
    }

    void Start()
    {
        anim = GetComponentInChildren<PlayerAnimations>();
        curDmg = defaultDmg;
    }

    public void SetNewDamage(int dmg)
    {
        curDmg = dmg;
    }

    public void SetDamageToDef()
    {
        curDmg = defaultDmg;
    }

    public void SetWeaponAnimInd(int ind)
    {
        if (ind > 0)
        {
            hasWeapon = true;
        }
        else
        {
            hasWeapon = false;
        }
        anim.SetWeaponInd(ind);
    }

    void Update()
    {
        if (Pause.Instance.IsPaused || MerchantDisplayer.Instance.IsWithMerchant
            || PlayerUI.Instance.IsInInventory || DialogDisplayer.Instance.IsInDialog)
            return;

        if(!isAttacking)
        {
            if(Input.GetMouseButton(0))
            {
                isAttacking = true;
                anim.SetAttackAnim(true);
                timer = 0f;
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (!wasDamaged)
            {
                if (timer >= animTimeBefore)
                {
                    wasDamaged = true;
                    Collider[] colls = Physics.OverlapSphere(transform.position, atkRange, enemyLayer);
                    foreach (Collider coll in colls)
                    {
                        if (coll.gameObject.TryGetComponent(out MobHealth mobHealth))
                        {
                            mobHealth.TakeDamage(curDmg);
                        }
                    }
                }
            }
            else
            {
                if (timer >= animTimeFull)
                {
                    wasDamaged = false;
                    timer = 0f;
                    isAttacking = false;
                    anim.SetAttackAnim(false);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, atkRange);
    }
}