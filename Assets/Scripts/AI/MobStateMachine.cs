using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStateMachine : MonoBehaviour
{
    public MobStatsSO Stats => stats;
    [SerializeField] private MobStatsSO stats;
    [SerializeField] private Transform player;

    [SerializeField] private AudioClip attackClip;

    public IdleState Idle => idle;
    private IdleState idle;
    public PatrolState Patrol => patrol;
    private PatrolState patrol;
    public FollowState Follow => follow;
    private FollowState follow;
    public AttackState Attack => attack;
    private AttackState attack;
    public DeathState Death => death;
    private DeathState death;

    private BaseState curState;

    void Start()
    {
        idle = new IdleState(this, player, stats);
        patrol = new PatrolState(this, player, stats);
        follow = new FollowState(this, player, stats);
        attack = new AttackState(this, player, stats, attackClip);
        death = new DeathState(this, stats);

        ChangeState(idle);
    }

    public void ChangeState(BaseState newState)
    {
        if (curState == death)
            return;

        //Debug.Log("Old state: " + curState);

        if (curState != null)
            curState.Exit();

        curState = newState;

        curState.Enter();

        //Debug.Log("New state: " + curState);
    }

    void Update()
    {
        if (curState != null)
            curState.LogicUpdate();
    }
}