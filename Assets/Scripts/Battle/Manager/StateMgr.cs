/****************************************************
	文件：StateMgr.cs
	作者：Happy-11
	日期：2020/12/30 21:23   	
	功能：状态管理器
*****************************************************/
using System.Collections.Generic;
using UnityEngine;
 public class StateMgr:MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();

    public void Init()
    {
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Move, new StateMove());
    }

    public void ChangeStatus(EntityBase entity,AniState targetState)
    {
        //当前状态等于要改变的状态 就直接return
        if (entity.currentAniState==targetState)
        {
            return;
        }
        //我们当前这个实体取出对应的状态
        if (fsm.ContainsKey(targetState))
        {
            if (entity.currentAniState!=AniState.None)
            {
                //退出当前状态
                fsm[entity.currentAniState].Exit(entity);
            }
            //进入这个状态
            fsm[targetState].Enter(entity); 
           //处理这个状态
            fsm[targetState].Process(entity);
        }
    }
}

