/****************************************************
    文件：LoginWnd.cs
	作者：Happy-11
    日期：2020/11/12 16:18:45
	功能：登录界面逻辑
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnLogin;
    public Button btnNotice;


    protected override void InitWnd()
    {
        base.InitWnd();
        //获取本地存储的账号密码
        if (PlayerPrefs.HasKey("Acct")&&PlayerPrefs.HasKey("Pass"))
        {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }

    //TODO 记录玩家账号密码

    public void ClickLogin()
    {
        audioSvc.PlayUIAudio(Constants.UILoginBtn);
        string acct = iptAcct.text;
        string pass = iptPass.text;
        if (acct!=""&&pass!="")
        {
            //更新本地储存的密码
            PlayerPrefs.SetString("Acct", acct);
            PlayerPrefs.SetString("Pass", pass);
            //TODO 发送网络消息
            NetLogin netLogin = new NetLogin(int.Parse(acct), pass);
        }
        else
        {
            GameRoot.AddTips("账号密码不能为空!!!!");
        }

    }
    public void ClickNotice()
    {
        GameRoot.AddTips("该模块暂时未开发！");

        audioSvc.PlayUIAudio(Constants.UIClickBtn);
    }
}