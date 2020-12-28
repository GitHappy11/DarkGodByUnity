/****************************************************
    文件：ChatWnd.cs
	作者：Happy-11
    日期：2020/12/1 22:53:21
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot 
{
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;
    public Text txtChat;

    public InputField iptChat;

    private int chatType;
    //不同的聊天记录 用不同的list存储起来
    private List<string> chatLIst = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();

        //用0,1,2来表示当前聊天类型（可以用枚举，这里用int方便）
        chatType = 0;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (chatType==0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatLIst.Count; i++)
            {
                chatMsg += chatLIst[i] + "\n";

            }
            SetText(txtChat, chatMsg);
            SetSprite(imgWorld,"ResImages/btnType1");
            SetSprite(imgGuild,"ResImages/btnType2");
            SetSprite(imgFriend,"ResImages/btnType2");
        }
        
        else if (chatType==1)
        {
            SetText(txtChat, "当前你还没有公会");
            SetSprite(imgWorld, "ResImages/btnType2");
            SetSprite(imgGuild, "ResImages/btnType1");
            SetSprite(imgFriend, "ResImages/btnType2");
        }
        else if (chatType==2)
        {
            SetText(txtChat, "当前你还没有好友");
            SetSprite(imgWorld, "ResImages/btnType2");
            SetSprite(imgGuild, "ResImages/btnType2");
            SetSprite(imgFriend, "ResImages/btnType1");
        }
    }

    public void AddChatMsg(string name,string chatContent)
    {
        chatLIst.Add(Constants.Color(name + ":", TxtColor.Blue) + chatContent);
        if (chatLIst.Count>10)
        {
            chatLIst.RemoveAt(0);
        }
        RefreshUI();
    }

    public void ClickSendMsgBtn()
    {
        if (iptChat.text!=null&&iptChat.text!=""&iptChat.text!=" ")
        {
            //发送网络消息 群发给所有客户端
            NetChat netChat = new NetChat(iptChat.text);
        }
        else
        {
            GameRoot.AddTips("输入的消息不合法！请重新输入！");
        }
        TaskSys.Instance.CalcTaskPrgs(6);
    }

    public void ClickWorldChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }
    public void ClickGuildChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }
    public void ClickFriendChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
    public void ClickCloseBtn()
    {
        chatType = 0;
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}