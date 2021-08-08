using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorCtrl : MonoBehaviour
{
    Animator animator;
    FsmBaseCtrl fsmBaseCtrl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fsmBaseCtrl = new FsmBaseCtrl((byte)PlayerAnimatorStateType.MAX_VALUE);

        PlayerIdle playerIdle = new PlayerIdle(animator);
        fsmBaseCtrl.AddState(playerIdle);

        PlayerWalk playerWalk = new PlayerWalk(animator);
        fsmBaseCtrl.AddState(playerWalk);

        PlayerRun playerRun = new PlayerRun(animator);
        fsmBaseCtrl.AddState(playerRun);

        PlayerJump playerJump = new PlayerJump(animator);
        fsmBaseCtrl.AddState(playerJump);

        PlayerAttack playerAttack = new PlayerAttack(animator);
        fsmBaseCtrl.AddState(playerAttack);
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<sbyte>("AttackToIdle", AttackToIdle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            fsmBaseCtrl.ChangeState((sbyte)PlayerAnimatorStateType.WALK);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            fsmBaseCtrl.ChangeState((sbyte)PlayerAnimatorStateType.ATTACK);
        }
        fsmBaseCtrl.UpdateLoopState();
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<sbyte>("AttackToIdle", AttackToIdle);
    }

    void AttackToIdle(sbyte stateIndex)
    {
        fsmBaseCtrl.ChangeState(stateIndex);
    }
}