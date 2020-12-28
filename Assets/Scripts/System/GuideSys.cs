/****************************************************
    文件：GuideSys.cs
	作者：Happy-11
    日期：2020/11/27 21:52:23
	功能：任务界面服务
*****************************************************/

using UnityEngine;

public class GuideSys : SystemRoot 
{
    public GuideSys Instance = null;
    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
    }

 
}