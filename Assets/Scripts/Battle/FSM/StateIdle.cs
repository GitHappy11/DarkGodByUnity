/****************************************************
    文件：StateIdle.cs
	作者：Happy-11
    日期：2021/1/2 11:3:23
	功能：待机状态
*****************************************************/



public class StateIdle : IState
{
    public void Enter(EntityBase entity)
    {
        entity.currentAniState = AniState.Idle;
    }


    public void Process(EntityBase entity)
    {
        entity.SetBlend(Constants.BlendIdel);
    }

    public void Exit(EntityBase entity)
    {
        
    }

    
}