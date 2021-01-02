/****************************************************
    文件：IState.cs
	作者：Happy-11
    日期：2021年1月2日19:00:15
	功能：状态接口
*****************************************************/



public interface IState 
{
    //进入这个状态的时候
    void Enter(EntityBase entity);

    //处理这个状态
    void Process(EntityBase entity);

    //退出这个状态
    void Exit(EntityBase entity);


    
}

public enum AniState
{
    None,
    Idle,
    Move,
}