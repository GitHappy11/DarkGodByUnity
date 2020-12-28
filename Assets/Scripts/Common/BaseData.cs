/****************************************************
    文件：BaseData.cs
	作者：Happy-11
    日期：2020/11/23 22:6:55
	功能：数据类基类
*****************************************************/

using UnityEngine;

public class MapCfgs : BaseData<MapCfgs>
{
    public string mapName;
    public string sceneName;

    public Vector3 mainCamPos;
    public Vector3 mainCamRote;

    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}

public class AutoGuideCfgs:BaseData<AutoGuideCfgs>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

public class StrongCfg:BaseData<StrongCfg>
{
    public int pos;
    public int startLv;
    public int addHP;
    public int addHurt;
    public int addef;
    public int minLv;
    public int coin;
    public int crystal;
}

public class TaskRewardCfg:BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData:BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}


public class BaseData<T> 
{
    public int ID;
}