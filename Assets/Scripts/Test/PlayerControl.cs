using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector3 dir;
    private float moveSpeed = 5;
    private Animator animator;

    CharacterController characterController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        EventCenter.Instance.AddEventListener<Vector2, bool>("JoyStick", CheckDirChange);
    }

    private void Update()
    {
        this.transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
        transform.LookAt(dir + transform.position);
    }

    private void CheckDirChange(Vector2 pos, bool isRun)
    {
        dir.x = pos.x;
        dir.z = pos.y;
        //animator.SetBool("IsRun", isRun);
    }

    private void OnAnimatorMove()
    {
        //characterController.Move(animator.deltaPosition);
        //transform.rotation = animator.rootRotation;
    }
}