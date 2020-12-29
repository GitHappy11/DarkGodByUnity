/****************************************************
    文件：MainCitySys.cs
	作者：Happy-11
    日期：2020/11/19 21:10:27
	功能：主城业务系统
*****************************************************/

using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;
    public TaskWnd taskWnd;
    public static MainCitySys Instance = null;

    private AutoGuideCfgs curtTaskData;
    PlayeController playeController;

    private Transform camTrans;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;
    private bool isNavGuide = false;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    }
    public void EnterMainCity()
    {
        //获取主城数据
        MapCfgs mapCfgs = resSvc.GetMapCfgData(Constants.MainCityMapID);
        LoginSys.Instance.createWnd.SetWndState(false);
        resSvc.AsyncLoadScene(mapCfgs.sceneName, () =>
         {

             //打开主城场景UI
             mainCityWnd.SetWndState();

             //加主城音乐
             audioSvc.PlayBGMusic(Constants.BGMainCity);

             GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
             MainCityMap mcm = map.GetComponent<MainCityMap>();
             npcPosTrans = mcm.NpcPosTrans;

             //加载主角
             LoadPlayer(mapCfgs);
             //人物相机设置
             if (camTrans != null)
             {
                 camTrans.gameObject.SetActive(false);
             }


         });

    }
    private void LoadPlayer(MapCfgs mapData)
    {
        GameObject player;
        player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        //设置大小
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playeController = player.GetComponent<PlayeController>();
        playeController.Init();
        nav = player.GetComponent<NavMeshAgent>();

    }


    #region 监听鼠标事件
    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playeController.transform.localEulerAngles.y;
    }
    public void SetMoveDir(Vector2 dir)
    {
        //使用轮盘运动时关闭自动导航
        StopNavTask();
        if (dir == Vector2.zero)
        {
            playeController.SetBlend(Constants.BlendIdel);
        }
        else
        {
            playeController.SetBlend(Constants.BlendWalk);
        }
        playeController.Dir = dir;
    }
    public void SetPlayerRoate(float roate)
    {
        playeController.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }
    #endregion
    
    #region 任务AI导航
    public void RunTask(AutoGuideCfgs agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }
        //导航系统开启
        nav.enabled = true;
        //解析任务数据
        if (curtTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playeController.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playeController.SetBlend(Constants.BlendIdel);
                nav.enabled = false;
                
                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playeController.SetBlend(Constants.BlendWalk);

            }
        }
        else
        {
            OpenGuideWnd();
        }
    }
    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playeController.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playeController.SetBlend(Constants.BlendIdel);
            nav.enabled = false;
            OpenGuideWnd();
        }
    }
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            nav.enabled = false;
            playeController.SetBlend(Constants.BlendIdel);
        }
    }
    #endregion

    #region UI界面的开启和关闭
    private void OpenGuideWnd()
    {
        //TODO 任务引导界面
        guideWnd.SetWndState();

    }
    public void OpenStrongWnd()
    {
        strongWnd.SetWndState();
    }
    public void OpenInfoWnd()
    {
        StopNavTask();
        if (camTrans == null)
        {
            camTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;

        }
        //设置人物展示相机的相对位置
        camTrans.localPosition = playeController.transform.position + playeController.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        camTrans.localEulerAngles = new Vector3(0, 180 + playeController.transform.localEulerAngles.y, 0);
        camTrans.localScale = Vector3.one;
        camTrans.gameObject.SetActive(true);

        infoWnd.SetWndState();
    }
    public void OpenChatWnd()
    {
        chatWnd.SetWndState();
    }
    public void OpenBuyWnd(int type=0)
    {
        buyWnd.buyType = type;

        buyWnd.SetWndState();
    }
    public void OpenTaskWnd()
    {
        taskWnd.SetWndState();
    }
    public void OpenFubenWnd()
    {
        FuBenSys.Instance.EnterFuben();
    }
    public void CloseInfoWnd()
    {
        if (camTrans != null)
        {
            camTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }
    
    
    #endregion

    public AutoGuideCfgs GetCurtTaskData()
    {
        return curtTaskData;
    }


    public void RshChatMsg(string name,string chatContent)
    {
        chatWnd.AddChatMsg(name, chatContent);
    }
    public void RefreshUIByMainCityWnd()
    {
        if (mainCityWnd!=null)
        {
            mainCityWnd.RefreshUI();
        }
        
    }
    public void RefreshUIByStrongWnd(int currentIndex)
    {
        if (strongWnd != null)
        {
            strongWnd.ClickPosItem(currentIndex);
        }

    }

    public void GetMoney()
    {
        PlayerData.Coin += 100;
        PlayerData.Crystal += 10;
        NetUpdatPlayerData netUpdatPlayerData = new NetUpdatPlayerData();
    }

    private void Update()
    {
        //处于导航状态就更新
        if (isNavGuide)
        {
            IsArriveNavPos();
            playeController.SetCam();
        }
    }
    

}