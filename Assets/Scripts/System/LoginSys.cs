/****************************************************
    文件：LoginSys.cs
	作者：Happy-11
    日期：2020/11/12 10:42:13
	功能：登录系统
*****************************************************/

using UnityEngine;

public class LoginSys : SystemRoot 
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;
    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
    }

    //进入登入场景
    public void EnterLogin()
    {   //打开加载界面
        //更新加载进度
        //异步加载登录场景 
        //显示加载进度条
        //加载完成后显示登录注册场景
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
         {
             loginWnd.SetWndState();
         }
        );
        //播放背景音效
        audioSvc.PlayBGMusic(Constants.BGLogin);

    }

    public void RspCreatePlayer()
    {
        GameRoot.AddTips("登录成功");
        createWnd.SetWndState();
        loginWnd.SetWndState(false);
    }
    public void RspEnterGame()
    {
        loginWnd.SetWndState(false);
        MainCitySys.Instance.EnterMainCity();
    }

}