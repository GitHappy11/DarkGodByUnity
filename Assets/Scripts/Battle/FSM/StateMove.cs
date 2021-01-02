/****************************************************
    文件：StateMove.cs
	作者：Happy-11
    日期：2021/1/2 11:4:0
	功能：行走状态
*****************************************************/

using UnityEngine;

public class StateMove : IState
{
    public void Enter(EntityBase entity)
    {
        //标记当前状态
        entity.currentAniState = AniState.Move;
    }

    public void Process(EntityBase entity)
    {
        entity.SetBlend(Constants.BlendMove);
    }

    public void Exit(EntityBase entity)
    {
        
    }

    
}