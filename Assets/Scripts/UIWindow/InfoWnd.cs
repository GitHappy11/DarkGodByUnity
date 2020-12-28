/****************************************************
    文件：InfoWnd.cs
	作者：Happy-11
    日期：2020/11/24 17:44:17
	功能：角色信息界面
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot
{
    #region UI组件
    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public RawImage rawImgPlayer;

    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtAttack;
    public Text txtDef;

    public Text txtDtHP;
    public Text txtDtAD;
    public Text txtDtAP;
    public Text txtDtAddef;
    public Text txtDtApdef;
    public Text txtDtDodge;
    public Text txtDtPierce;
    public Text txtDtCritlcal;

    public Transform transDetail;

    public Vector2 startPos;
    #endregion
    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvets();
        RefreshUI();
    }

    private void RefreshUI()
    {
        SetText(txtInfo, PlayerData.playerName + "Lv." + PlayerData.Level);
        SetText(txtExp, PlayerData.Exp + "/" + Common.Tools.GetExpUpValByLv(PlayerData.Level));
        imgExpPrg.fillAmount = PlayerData.Exp * 1.0f / Common.Tools.GetExpUpValByLv(PlayerData.Level);
        SetText(txtPower, PlayerData.Power + "/" + Common.Tools.GetPowerLimit(PlayerData.Level));
        imgPowerPrg.fillAmount = PlayerData.Power * 1.0f / Common.Tools.GetPowerLimit(PlayerData.Level);

        SetText(txtJob, "暗夜刺客");
        SetText(txtFight, Common.Tools.GetFightByProps(PlayerData.Level, PlayerData.AD, PlayerData.AP, PlayerData.Addef, PlayerData.Apdef));
        SetText(txtHP, PlayerData.HP);
        SetText(txtAttack, PlayerData.AD+PlayerData.AP);
        SetText(txtDef, PlayerData.Addef + PlayerData.Apdef);

        SetText(txtDtHP, PlayerData.HP);
        SetText(txtDtAD, PlayerData.AD);
        SetText(txtDtAP, PlayerData.AP);
        SetText(txtDtAddef, PlayerData.Addef);
        SetText(txtDtApdef, PlayerData.Apdef);
        SetText(txtDtDodge, PlayerData.Dodge);
        SetText(txtDtPierce, PlayerData.Pierce);
        SetText(txtDtCritlcal, PlayerData.Critical);
        
    }

    private void RegTouchEvets()
    {
        OnClickDown(rawImgPlayer.gameObject, (PointerEventData evt) =>
         {
             startPos = evt.position;
             MainCitySys.Instance.SetStartRoate();
         });
        OnClickDrag(rawImgPlayer.gameObject, (PointerEventData evt) =>
         {
             float roate = -(evt.position.x - startPos.x)*0.4f;
             MainCitySys.Instance.SetPlayerRoate(roate);
         });
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }
    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail);
    }
    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail,false);
    }


}