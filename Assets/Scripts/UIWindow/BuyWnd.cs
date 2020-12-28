/****************************************************
    文件：BuyWnd.cs
	作者：Happy-11
    日期：2020/12/2 23:12:37
	功能:购买交易界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;


public class BuyWnd :WindowRoot  
{
    public Text txtInfo;
    //0购买体力 1充值确定
    public int buyType;

    public void SetBuyType(int type)
    {

    }



    public void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                //体力购买
                txtInfo.text = "是否花费10钻石买100体力？";

                break;
            case 1:
                //充值界面
                txtInfo.text = "是否充值100金币和10钻石？";
                break;
            default:
                break;
        }
    }

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    public void ClickSureBuy()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        switch (buyType)
        {
            case 0:
                NetBuy netBuy = new NetBuy(Common.BuyCode.Power);
                break;
            case 1:
                NetBuy netBuy2 = new NetBuy(Common.BuyCode.Coin);
                break;
            default:
                break;
        }
    }

    public  void CloseWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}