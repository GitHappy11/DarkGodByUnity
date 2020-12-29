/****************************************************
    文件：FuBenWnd.cs
	作者：Happy-11
    日期：2020/12/29 0:20:21
	功能：副本界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class FuBenWnd : WindowRoot 
{
    public Transform pointerTrans;
    public Button[] fbBtnArr;

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    public void RefreshUI()
    {
        
        for (int i = 0; i < fbBtnArr.Length; i++)
        {
            if (i<PlayerData.Fuben%10000)
            {
                SetActive(fbBtnArr[i].gameObject);
                if (i == PlayerData.Fuben % 10000 - 1)
                {
                    pointerTrans.SetParent(fbBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(25, 100, 0);
                 }
            }
            else
            {
                SetActive(fbBtnArr[i].gameObject, false);
            }
        }
    }
    public void ClickFuben(int FubenID)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        //检查体力是否足够
        if (PlayerData.Power>=resSvc.GetMapCfgData(FubenID).power)
        {
            GameRoot.AddTips("进入副本！");
            PlayerData.Power -= resSvc.GetMapCfgData(FubenID).power;
            NetUpdatPlayerData netUpdatPlayerData = new NetUpdatPlayerData();
        }
        else
        {
            GameRoot.AddTips("体力不足！无法进入副本");
        }
        
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

}