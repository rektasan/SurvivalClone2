using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float attackAnimTimerBefore = 0.52f;
    private float attackAnimTimerFull = 1.133f;
    private bool attackedPlayer;
    private float timer;

    private Transform target;
    private Animator animator;
    private MobStatsSO stats;
    private PlayerStats playerStats;

    private AudioClip attackClip;

    public AttackState(MobStateMachine states, Transform targetTransform, MobStatsSO statsSO, AudioClip clip)
    {
        stateMachine = states;
        animator = stateMachine.GetComponent<Animator>();
        target = targetTransform;
        stats = statsSO;
        playerStats = target.GetComponent<PlayerStats>();

        attackClip = clip;
    }

    public override void Enter()
    {
        animator.SetBool("Attack", true);
    }

    public override void Exit()
    {
        animator.SetBool("Attack", false);
        timer = 0f;
        attackedPlayer = false;
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(stateMachine.transform.position, target.position) > stats.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.Follow);
            return;
        }

        timer += Time.deltaTime;

        if (!attackedPlayer)
        {
            if (timer >= attackAnimTimerBefore)
            {
                attackedPlayer = true;
                playerStats.TakeDamage(stats.Power);
                AudioManager.Instance.PlaySound(attackClip, true);
            }
        }
        else
        {
            if (timer >= attackAnimTimerFull)
            {
                attackedPlayer = false;
                timer = 0f;
            }
        }
    }
}