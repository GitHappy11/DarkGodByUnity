/****************************************************
    文件：FuBenSys.cs
	作者：Happy-11
    日期：2020/12/29 0:21:10
	功能：副本业务
*****************************************************/

using UnityEngine;

public class FuBenSys : SystemRoot
{
    public static FuBenSys Instance = null;
    public FuBenWnd fuBenWnd;
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
    }

    public void EnterFuben()
    {
        fuBenWnd.SetWndState();
    }

}