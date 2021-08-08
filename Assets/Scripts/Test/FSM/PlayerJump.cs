using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : FsmBase
{
    private Animator animator;

    public PlayerJump(Animator animator)
    {
        this.animator = animator;
    }

    public override void EnterState()
    {
        animator.SetInteger("StateIndex", (int)PlayerAnimatorStateType.JUMP + 1);
    }

    public override void LeaveState()
    {

    }

    public override void UpdateLoopState()
    {

    }
}