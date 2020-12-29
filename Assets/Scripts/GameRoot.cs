/****************************************************
    文件：GameRoot.cs
	作者：Happy-11
    日期：2020/11/11 21:29:35
	功能：游戏初始化
*****************************************************/
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance=null;


    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start()
    {
        Instance = this;
        //切换场景时不被销毁
        DontDestroyOnLoad(this);
        //清理预览界面开启的UI
        ClearUIRoot();
        Init();
    }
    //系统初始化
    private void Init()
    {
        //服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimeSys time = GetComponent<TimeSys>();
        time.InitSvc();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCity = GetComponent<MainCitySys>();
        mainCity.InitSys();
        TaskSys taskSys = GetComponent<TaskSys>();
        taskSys.InitSys();
        FuBenSys fuBenSy = GetComponent<FuBenSys>();
        fuBenSy.InitSys();

        //进入登录场景并加载相应的UI
        login.EnterLogin();

    }

    //清理UI
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWnd.SetWndState();
    }

    //封装API  更好使用
    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }
}