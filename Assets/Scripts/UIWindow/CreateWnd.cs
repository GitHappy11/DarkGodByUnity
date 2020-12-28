/****************************************************
    文件：CreateWnd.cs
	作者：Happy-11
    日期：2020/11/12 21:31:13
	功能：角色创建页面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
public class CreateWnd : WindowRoot 
{
    public InputField iptName;

    protected override void InitWnd()
    {
        base.InitWnd();
        //随机给个女名字
        iptName.text = resSvc.GetRDNnameData(false);
    }
   
    public void ClickRand()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn); ;
        string rdName = resSvc.GetRDNnameData(false);
        iptName.text = rdName;
    }

    public void ClickEnter()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (iptName.text!="")
        {
            //发送网络消息
            NetCreatPlayer netCreatPlayer = new NetCreatPlayer(iptName.text);
        }
        else
        {
            GameRoot.AddTips("名称不能为空！");
        }
        
    }
    

   
}