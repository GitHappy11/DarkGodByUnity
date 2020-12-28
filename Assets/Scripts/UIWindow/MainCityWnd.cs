/****************************************************
    文件：MainCityWnd.cs
	作者：Happy-11
    日期：2020/11/19 21:9:45
	功能：主城界面
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot 
{
    public Text txtFight;
    public Text txtPower;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    


    public Image imgPowerPrg;
    public Image imgTouch;
    public Image imgDirPoint;
    public Image imgDirBg;
    public Transform expPrgTrans;

    public Button btnMenu;

    public Animation aniMenu;


    public float pointDis;
    //轮盘控制中心位置
    public Vector2 starPos = Vector2.zero;
    //轮盘默认位置
    public Vector2 defaultPos = Vector2.zero;

    //当前任务数据
    private AutoGuideCfgs curtTaskData;
    public Button btnGuide;
    

    protected override void InitWnd()
    {
        base.InitWnd();
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        //游戏开始默认轮盘位置
        defaultPos = imgTouch.transform.position;
        //游戏开始轮盘控件中心不应该出现
        SetActive(imgDirPoint, false);

        //轮盘控制
        RegisterTouchEvets();
        RefreshUI();
    }


    public void RefreshUI()
    {
        #region UI
        SetText(txtFight, Common.Tools.GetFightByProps(PlayerData.Level, PlayerData.AD, PlayerData.AP, PlayerData.Addef, PlayerData.Apdef));
        SetText(txtPower, "体力" + PlayerData.Power + "/" + Common.Tools.GetPowerLimit(PlayerData.Level));
        imgPowerPrg.fillAmount=PlayerData.Power * 1.0f / Common.Tools.GetPowerLimit(PlayerData.Level);
        SetText(txtLevel, PlayerData.Level);
        SetText(txtName, PlayerData.playerName);

        //expprg 进度条自适应
        //获取当前经验值是升级所需经验值的百分比
        int expPrgVal=(int)(PlayerData.Exp * 1.0f / Common.Tools.GetExpUpValByLv(PlayerData.Level) * 100);
        //改变组件的值
        SetText(txtExpPrg, expPrgVal + "%");
        //把经验值百分比分成10份
        int index=expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        //计算实际场景高度是当前分辨率的多少占比
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        //计算出来当前的场景宽度应该是多少
        float screenWidth = Screen.width * globalRate;
        //计算经验条应该是多长（实际场景宽度减去边边角角）
        float width = (screenWidth - 106)/10;
        //然后调整经验条的位置
        grid.cellSize = new Vector2(width, 5);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            //前面几份肯定是满的
            if (i<index)
            {
                img.fillAmount = 1;
            }
            //到了半满那份就要调整宽度
            else if(i==index)
            {
                img.fillAmount = expPrgVal%10 * 1.0f / 10;
            }
            //最后几份肯定是空的
            else
            {
                img.fillAmount = 0;
            }
        }
        #endregion

        //设置自动任务图标
        if (resSvc!=null)
        {
            curtTaskData = resSvc.GetAutoGuideCfgData(PlayerData.GuideID);
        }
        
        if (curtTaskData!=null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
        

    }

    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        //
        SetSprite(img, spPath);
    }


    private bool menuState = true;
    public void ClikcMenuBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);
        //点击后将状态转换
        menuState = !menuState;
        AnimationClip clip = null;
        if (menuState)
        {
            clip = aniMenu.GetClip("aniBtnMenu");
        }
        else
        {
            clip = aniMenu.GetClip("aniBtnMenuOff");
        }
        aniMenu.Play(clip.name);

    }
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (curtTaskData!=null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("游戏内容暂时完结，请等待下一个版本。。。");
        }
    }
    public void ClickMoney()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.OpenBuyWnd(1);
    }
    public void ClickChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.OpenChatWnd();
    }
    public void ClickStrongWnd()
    {
        MainCitySys.Instance.OpenStrongWnd();
    }
    public void ClickVipBtn()
    {
        NetTask netTask = new NetTask();
        GameRoot.AddTips("测试用按钮");
    }
    public void ClickTaskWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.OpenTaskWnd();
    }
    public void ClickBuyPower()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.OpenBuyWnd(0);
    }

    public void ClickFubenWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
    }
    public void RegisterTouchEvets()
    {

        OnClickDown(imgDirBg.gameObject, (PointerEventData evt) =>
         {
             starPos = evt.position;
             SetActive(imgDirPoint);
             imgTouch.transform.position = evt.position;
         });

        OnClickUp(imgDirBg.gameObject, (PointerEventData evt) =>
        {
            imgTouch.transform.position = defaultPos;
            SetActive(imgDirPoint,false);
            //这里是localPosition  因为是基于它的父物体的归零位置
            imgDirPoint.transform.localPosition = Vector2.zero;
            //TODO 方向信息传递
            MainCitySys.Instance.SetMoveDir(Vector2.zero);
        });

        OnClickDrag(imgDirBg.gameObject, (PointerEventData evt) =>
        {
            //拖动的距离
            Vector2 dir = evt.position - starPos;
            //离点中心的距离
            float len = dir.magnitude;
            //距离如果没超过规定的范围
            if (len>pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                //就把点位置设置成初始位置加上拖动的距离
                imgDirPoint.transform.position = starPos + clampDir;
            }
            else
            {
                //超过了就原地不动
                imgDirPoint.transform.position = evt.position; ;
            }
            //方向信息传递
            MainCitySys.Instance.SetMoveDir(dir.normalized);

        });

    }
 
}