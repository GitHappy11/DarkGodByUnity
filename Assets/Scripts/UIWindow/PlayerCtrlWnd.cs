/****************************************************
    文件：PlayerCtrlWnd.cs
	作者：Happy-11
    日期：2021/1/2 10:1:55
	功能：玩家控制界面
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot 
{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Text txtName;
    public Text txtLevel;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    public float pointDis;
    //轮盘控制中心位置
    public Vector2 starPos = Vector2.zero;
    //轮盘默认位置
    public Vector2 defaultPos = Vector2.zero;

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

  
        SetText(txtLevel, PlayerData.Level);
        SetText(txtName, PlayerData.playerName);

        //expprg 进度条自适应
        //获取当前经验值是升级所需经验值的百分比
        int expPrgVal = (int)(PlayerData.Exp * 1.0f / Common.Tools.GetExpUpValByLv(PlayerData.Level) * 100);
        //改变组件的值
        SetText(txtExpPrg, expPrgVal + "%");
        //把经验值百分比分成10份
        int index = expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        //计算实际场景高度是当前分辨率的多少占比
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        //计算出来当前的场景宽度应该是多少
        float screenWidth = Screen.width * globalRate;
        //计算经验条应该是多长（实际场景宽度减去边边角角）
        float width = (screenWidth - 106) / 10;
        //然后调整经验条的位置
        grid.cellSize = new Vector2(width, 5);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            //前面几份肯定是满的
            if (i < index)
            {
                img.fillAmount = 1;
            }
            //到了半满那份就要调整宽度
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            //最后几份肯定是空的
            else
            {
                img.fillAmount = 0;
            }
        }
        #endregion

     


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
            SetActive(imgDirPoint, false);
            //这里是localPosition  因为是基于它的父物体的归零位置
            imgDirPoint.transform.localPosition = Vector2.zero;
            //TODO 方向信息传递
            BattleSys.Instance.SetMoveDir(Vector2.zero);
        });

        OnClickDrag(imgDirBg.gameObject, (PointerEventData evt) =>
        {
            //拖动的距离
            Vector2 dir = evt.position - starPos;
            //离点中心的距离
            float len = dir.magnitude;
            //距离如果没超过规定的范围
            if (len > pointDis)
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
            BattleSys.Instance.SetMoveDir(dir.normalized);

        });

    }

    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }
    public void ClickSkill1()
    {
        BattleSys.Instance.ReqReleaseSkill(1);
    }
    public void ClickSkill2()
    {
        BattleSys.Instance.ReqReleaseSkill(2);
    }
    public  void ClickSkill3()
    {
        BattleSys.Instance.ReqReleaseSkill(3);
    }
}