/****************************************************
    文件：EntityBase.cs
	作者：Happy-11
    日期：2021/1/2 11:2:0
	功能：逻辑实体基类
*****************************************************/



using UnityEngine;

public class EntityBase 
{
    public AniState currentAniState = AniState.None;

    public StateMgr stateMgr = null;
    public Controller controller = null;

    public void Move()
    {
        stateMgr.ChangeStatus(this, AniState.Move);
        
    }
    public void Idle()
    {
        stateMgr.ChangeStatus(this, AniState.Idle);
    }
    public virtual void SetBlend(float blend)
    {
        if (controller!=null)
        {
            controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if(controller != null)
        {
            controller.Dir = dir;
        }
    }
}