using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : FsmBase
{
    private Animator animator;

    public PlayerIdle(Animator animator)
    {
        this.animator = animator;
    }

    public override void EnterState()
    {
        animator.SetInteger("StateIndex", (int)PlayerAnimatorStateType.IDLE + 1);
    }

    public override void LeaveState()
    {

    }

    public override void UpdateLoopState()
    {

    }
}