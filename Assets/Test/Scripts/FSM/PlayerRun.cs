using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : FsmBase
{
    private Animator animator;

    public PlayerRun(Animator animator)
    {
        this.animator = animator;
    }

    public override void EnterState()
    {
        animator.SetInteger("StateIndex", (int)PlayerAnimatorStateType.RUN + 1);
    }

    public override void LeaveState()
    {

    }

    public override void UpdateLoopState()
    {

    }
}