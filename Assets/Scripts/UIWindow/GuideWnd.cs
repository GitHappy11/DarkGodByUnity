/****************************************************
    文件：GuideWnd.cs
	作者：Happy-11
    日期：2020/11/26 22:3:38
	功能：任务对话引导界面
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot 
{

    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    private AutoGuideCfgs curtTaskData;
    private string[] dialogArr;
    private int index;
    protected override void InitWnd()
    {
        base.InitWnd();
        curtTaskData = MainCitySys.Instance.GetCurtTaskData();
        //使用分割符号切对话
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0]=="0")
        {
            //自己
            SetSprite(imgIcon,PathDefine.SelfIcon);
            SetText(txtName, PlayerData.playerName);
        }
        else
        {
            //对话NPC
            switch (curtTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小云");
                    break;
            }
        }

        //等于点击了“设置原生大小”，保持统一
        imgIcon.SetNativeSize();
        //将文本中的“$name”改变成自己的名字
        SetText(txtTalk, talkArr[1].Replace("$name", PlayerData.playerName));
    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        index += 1;
        if (index==dialogArr.Length)
        {
            //对话完成 发送任务完成的服务器消息
            SetWndState(false);
            PlayerData.GuideID += 1;
            PlayerData.Coin += curtTaskData.coin;
            ComTools.CalcExp(curtTaskData.exp);
            GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curtTaskData.coin + "经验" + curtTaskData.exp,TxtColor.Blue));
            switch (curtTaskData.actID)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    break;
            }
            
            
            


            NetUpdatPlayerData net = new NetUpdatPlayerData();


        }
        else
        {
            SetTalk();
        }
        
    }
    
}