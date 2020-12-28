/****************************************************
    文件：PETImeTask.cs
	作者：Happy-11
    日期：2020年12月3日21:48:14
	功能：定时任务数据类
*****************************************************/



using System;

public class PETimeTask 
{
    //唯一的任务ID  便于寻找
    public int tID;
    //时间
    public double destTime;
    //需要执行的任务
    public Action<int> callback;
    //执行次数
    public int count;
    //循环执行时间
    public double delay;

    //使用构造函数 方便new
    public PETimeTask(int tID,Action<int> callback, double destTime, double delay,int count)
    {
        this.tID = tID;
        this.destTime = destTime;
        this.callback = callback;
        this.delay = delay;
        this.count = count;
        
        
    }

}

//以帧为准的定时任务
public class PETrameTask
{
    //唯一的任务ID  便于寻找
    public int tID;
    //帧数
    public int destFrame;
    //需要执行的任务
    public Action<int> callback;
    //执行次数
    public int count;
    //循环执行帧数
    public int delay;

    //使用构造函数 方便new
    public PETrameTask(int tID, Action<int> callback, int destFrame, int delay, int count)
    {
        this.tID = tID;
        this.destFrame = destFrame;
        this.callback = callback;
        this.delay = delay;
        this.count = count;


    }

}

//设定时间单位
public enum PEtimeUnit
{
    MilliSecond,
    Second,
    Minute,
    Hour,
    Day
}