/****************************************************
    文件：StrongWnd.cs
	作者：Happy-11
    日期：2020/11/28 21:50:14
	功能：强化界面
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class StrongWnd : WindowRoot 
{
    #region UI组件
    public Image imgCurtPos;
    public Text txtStar;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtCoin;
    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;
    public Transform cosTransRoot;
    #endregion

    StrongCfg nextSd;
    public Transform posBtnTrans;
    //数组需要初始化 否则会报空
    private GameObject[] bgsPick=new GameObject[6];
    private int currentIndex;
    protected override void InitWnd()
    {
        base.InitWnd();
        RegClickEvts();
        ClickPosItem(0);
       
    }

    private void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {

           
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            OnClick(img.gameObject, (object args) =>
             {
                 audioSvc.PlayUIAudio(Constants.UIClickBtn);
                 ClickPosItem((int)args);

             },i);
            bgsPick[i] = posBtnTrans.GetChild(i).transform.Find("bgPick").gameObject;

        }
    }
    public void ClickPosItem(int index)
    {
        currentIndex = index;
        for (int i = 0; i < bgsPick.Length; i++)
        {
            if (i==currentIndex)
            {
                //使用箭头表示
                bgsPick[i].SetActive(true);
            }
            else
            {
                bgsPick[i].SetActive(false);
            }
            
        }
        RefreshItem();
    }
    public void ClickLoseWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
    public void ClickStrong()
    {
        audioSvc.PlayUIAudio(Constants.FBItemEnter);
        if (PlayerData.StrongArr[currentIndex]<10)
        {
            if (PlayerData.Level<nextSd.minLv)
            {
                GameRoot.AddTips("角色等级不够！！");
                return;
            }
            if (PlayerData.Coin<nextSd.coin)
            {
                GameRoot.AddTips("金币不够！！！");
                return;
            }
            if (PlayerData.Crystal<nextSd.crystal)
            {
                GameRoot.AddTips("钻石不够！！！");
                return;
            }
            NetStrong netStrong = new NetStrong(currentIndex);
        }
        else
        {
            GameRoot.AddTips("装备等级已经满级！");
        }
        
    }
    private void RefreshItem()
    {
        //金币
        SetText(txtCoin, PlayerData.Coin);
        //装备图标
        switch (currentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaoBu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
            default:
                break;
        }
        //星级
        SetText(txtStar, PlayerData.StrongArr[currentIndex]+"星级");
        int curtStar = PlayerData.StrongArr[currentIndex];

        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i<curtStar)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }
        int nextStarLv = curtStar + 1;

        int sumAddHP = resSvc.GetPropAddValPreLv(currentIndex, curtStar, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, curtStar, 2);
        int sumAddef = resSvc.GetPropAddValPreLv(currentIndex, curtStar, 3);

        SetText(propHP1,"生命  +" + sumAddHP);
        SetText(propHurt1,"伤害  +" + sumAddHurt);
        SetText(propDef1,"防御  +" + sumAddef);



        nextSd = resSvc.GetStrongCfg(currentIndex, nextStarLv);
        if (nextSd!=null)
        {
            SetActive(propHP2, true);
            SetActive(propHurt2, true);
            SetActive(propDef2, true);
            SetActive(cosTransRoot, true);
            SetActive(propArr1, true);
            SetActive(propArr2, true);
            SetActive(propArr3, true);

            SetText(propHP2, "+"+nextSd.addHP);
            SetText(propHurt2, "+"+nextSd.addHurt);
            SetText(propDef2, "+"+nextSd.addef);

            SetText(txtNeedLv, "需要等级：" + nextSd.minLv);
            SetText(txtCostCoin,nextSd.coin);
            SetText(txtCostCrystal,nextSd.crystal+"/"+PlayerData.Crystal);
        }
        //超过10星就没有数据，更新一波UI
        else
        {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);
            SetActive(cosTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);
        }
    }
    public void CloseWnd()
    {
        SetWndState(false);
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
    }
    
}