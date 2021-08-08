using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有限状态机控制
/// </summary>
public class FsmBaseCtrl
{
    /// <summary>
    /// 有限状态机中所有的状态
    /// </summary>
    private FsmBase[] allFsmBase;
    /// <summary>
    /// 有限状态机中最后一个有效状态的索引
    /// </summary>
    sbyte lastStateIndex = -1;
    /// <summary>
    /// 有限状态机中当前正在运作的状态索引
    /// </summary>
    sbyte currentSteteIndex = -1;

    public FsmBaseCtrl(byte allStateCount)
    {
        allFsmBase = new FsmBase[allStateCount];
    }

    /// <summary>
    /// 添加状态到有限状态机当中
    /// </summary>
    /// <param name="state">要添加的状态</param>
    public void AddState(FsmBase state)
    {
        if (lastStateIndex < allFsmBase.Length)
        {
            lastStateIndex++;
            allFsmBase[lastStateIndex] = state;
        }
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="targetStateIndex">目标状态的索引</param>
    public void ChangeState(sbyte targetStateIndex)
    {
        //防止索引越界
        targetStateIndex = (sbyte)(targetStateIndex % allFsmBase.Length);
        //当前状态为非空
        if (currentSteteIndex != -1)
        {
            //当前状态离开
            allFsmBase[currentSteteIndex].LeaveState();
        }
        //进入目标状态
        currentSteteIndex = targetStateIndex;
        allFsmBase[currentSteteIndex].EnterState();
    }

    /// <summary>
    /// 更新循环状态
    /// </summary>
    public void UpdateLoopState()
    {
        if (currentSteteIndex != -1)
        {
            allFsmBase[currentSteteIndex].UpdateLoopState();
        }
    }
}