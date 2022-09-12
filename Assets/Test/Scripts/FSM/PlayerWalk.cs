using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : FsmBase
{
    private Animator animator;

    public PlayerWalk(Animator animator)
    {
        this.animator = animator;
    }

    public override void EnterState()
    {
        animator.SetInteger("StateIndex", (int)PlayerAnimatorStateType.WALK + 1);
    }

    public override void LeaveState()
    {

    }

    public override void UpdateLoopState()
    {

    }
}