/****************************************************
    文件：BattleMgr.cs
	作者：Happy-11
    日期：2020/12/30 21:10:0
	功能：战斗管理器
*****************************************************/

using System;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;
    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    private EntityPlayer entityPlayer;

    public void Init(int mapID)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        //初始化各个管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();


        //加载战场地图
        MapCfgs mapData = resSvc.GetMapCfgData(mapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
         {
             //初始化地图数据
             GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
             mapMgr = map.GetComponent<MapMgr>();
             mapMgr.Init();

             //初始化地图位置和摄像机位置
             map.transform.localPosition = Vector3.zero;
             map.transform.localScale = Vector3.zero;
             Camera.main.transform.position = mapData.mainCamPos;
             Camera.main.transform.localEulerAngles = mapData.mainCamRote;


             LoadPlayer(mapData);
             audioSvc.PlayBGMusic(Constants.BGHuangYe);
             entityPlayer.Idle();
             

         });


    }


    public void LoadPlayer(MapCfgs mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.ASsissnBattlePlayerPrefab);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        //实例化一个逻辑实体
        entityPlayer = new EntityPlayer();
        //注入状态管理器
        entityPlayer.stateMgr = stateMgr;

        //初始化角色控制器
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entityPlayer.controller = playerCtrl;



    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        //设置玩家移动
        if (dir==Vector2.zero)
        {
            entityPlayer.Idle();
            entityPlayer.SetDir(Vector2.zero);
        }
        else
        {
            entityPlayer.Move();
            entityPlayer.SetDir(dir);
        }

    }


    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
            default:
                break;
        }
    }

    private void ReleaseNormalAtk()
    {
        Debug.Log("释放了一次普通攻击");
    }
    private void ReleaseSkill1()
    {
        Debug.Log("释放了技能一");
    }
    private void ReleaseSkill2()
    {
        Debug.Log("释放了技能二");
    }
    private void ReleaseSkill3()
    {
        Debug.Log("释放了技能三");
    }
}