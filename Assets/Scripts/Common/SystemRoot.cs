/****************************************************
    文件：SystemRoot.cs
	作者：Happy-11
    日期：2020/11/12 19:55:4
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot:MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }
}