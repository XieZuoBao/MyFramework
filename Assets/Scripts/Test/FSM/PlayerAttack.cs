using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : FsmBase
{
    private Animator animator;
    private float timeCount = 0;

    public PlayerAttack(Animator animator)
    {
        this.animator = animator;
    }

    public override void EnterState()
    {
        animator.SetInteger("StateIndex", (int)PlayerAnimatorStateType.ATTACK + 1);
        timeCount = 0;
    }

    public override void LeaveState()
    {

    }

    public override void UpdateLoopState()
    {
        timeCount += Time.deltaTime;
        if (timeCount > 0.23f)
        {
            timeCount = 0;
            EventCenter.Instance.EventTrigger<sbyte>("AttackToIdle", (sbyte)PlayerAnimatorStateType.IDLE);
        }
    }
}