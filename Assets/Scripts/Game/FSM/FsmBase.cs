using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有限状态机抽象基类
/// </summary>
public abstract class FsmBase
{
    /// <summary>
    /// 进入状态
    /// </summary>
    public abstract void EnterState();

    /// <summary>
    /// 离开状态
    /// </summary>
    public abstract void LeaveState();

    /// <summary>
    /// 更新循环状态
    /// </summary>
    public abstract void UpdateLoopState();
}