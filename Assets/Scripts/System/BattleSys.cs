/****************************************************
    文件：BattleSys.cs
	作者：Happy-11
    日期：2020/12/30 20:58:49
	功能：战斗业务系统
*****************************************************/

using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    } 

    public void StartBattle(int mapID)
    {
        MainCitySys.Instance.mainCityWnd.SetWndState(false);

        FuBenSys.Instance.SetFubenWnd(false);

        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Instance.transform);
        BattleMgr battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapID);
        

    }
}