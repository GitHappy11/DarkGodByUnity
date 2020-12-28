/****************************************************
    文件：ResSvc.cs
	作者：Happy-11
    日期：2020/11/12 10:41:27
	功能：资源加载服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour 
{
    //单例
    public static ResSvc Instance = null;



    public void InitSvc()
    {
        Instance = this;
        //加载cfg资源文件
        InitRdNameCfgs(PathDefine.RDNameCfg);
        InitMapsCfgs(PathDefine.MapCfg);
        InitGuideCfgs(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);
        InitTaskRewardCfgs(PathDefine.TaskRewardCfg);
    }

    //更新回调
    private Action prgCB = null;
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="loaded">加载场景的方法</param>
    public void AsyncLoadScene(string sceneName,Action loaded)
    {
        //打开相应的窗口
        GameRoot.Instance.loadingWnd.SetWndState();
        //拿到异步操作
       AsyncOperation sceneAsync=  SceneManager.LoadSceneAsync(sceneName);
        //使用委托更新进度
        prgCB = () =>
        {
            //获取进度更新
            float val = sceneAsync.progress;
            //更新进度条
            GameRoot.Instance.loadingWnd.SetProgress(val);
            //加载完成后的操作
            if (val == 1)
            {
                if (loaded!=null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };
    }

    private void Update()
    {
        //if (prgCB != null)
        //{
        //    prgCB();
        //}

        prgCB?.Invoke();
    }

    //音乐资源缓存字典
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path,bool cache=false)
    {
        AudioClip au = null;
        //检查字典中是否已经有缓存
        if (!adDic.TryGetValue(path,out au))
        {
            //不存在就加载
            au = Resources.Load<AudioClip>(path);
            //需要缓存就把它缓存起来
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
     
        return au;
    }

    //游戏对象缓存字典
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path,bool cache=false)
    {
        GameObject prefabs;
        if (!goDic.TryGetValue(path,out prefabs))
        {
            prefabs = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefabs);
            }
        }
        GameObject go = null;
        if (prefabs!=null)
        {
            go = Instantiate(prefabs);
        }
        return go;
    }
    //图片缓存字典
    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();

    public Sprite LoadSprite(string path,bool cache=false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    #region RdNameCfgs
    //存储数据
    private List<string> surnameLst = new List<string>();
    private List<string> manLst = new List<string>();
    private List<string> womanLst = new List<string>(); 
    private void InitRdNameCfgs(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("Xml file:"+path+"加载失败，请检查文件是否正确");
        }
        else
        {
            //建立一个XML
            XmlDocument doc = new XmlDocument();
            //读取XML内容
            doc.LoadXml(xml.text);
            //读取root下的子节点 成为一个队列
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            //读取节点数据
            for (int i = 0; i < nodLst.Count; i++)
            {
               XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID")==null)
                {
                    continue;
                }
                int ID= Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameLst.Add(e.InnerText);
                            break;
                        case "man":
                            manLst.Add(e.InnerText);
                            break;
                        case "woman":
                            womanLst.Add(e.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }


        }

    }
    public string GetRDNnameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surnameLst[ComTools.RDint(0, surnameLst.Count - 1)];
        if (man)
        {
            rdName += manLst[ComTools.RDint(0, manLst.Count - 1)];
        }
        else
        {
            rdName += womanLst[ComTools.RDint(0, womanLst.Count - 1)];
        }
        return rdName;

    }

    #endregion
    #region MapCfgs
    //存储数据
    private Dictionary<int, MapCfgs> mapCfgsDict = new Dictionary<int, MapCfgs>();
    private void InitMapsCfgs(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("Xml file:" + path + "加载失败，请检查文件是否正确");
        }
        else
        {
            //建立一个XML
            XmlDocument doc = new XmlDocument();
            //读取XML内容
            doc.LoadXml(xml.text);
            //读取root下的子节点 成为一个队列
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            //读取节点数据
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfgs mc = new MapCfgs
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                { 
                    
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                            string[] valArr = e.InnerText.Split(',');
                            mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            break;
                        case "mainCamRote":
                            string[] valArr1 = e.InnerText.Split(',');
                            mc.mainCamRote = new Vector3(float.Parse(valArr1[0]), float.Parse(valArr1[1]), float.Parse(valArr1[2]));
                            break;
                        case "playerBornPos":
                            string[] valArr2 = e.InnerText.Split(',');
                            mc.playerBornPos = new Vector3(float.Parse(valArr2[0]), float.Parse(valArr2[1]), float.Parse(valArr2[2]));
                            break;
                        case "playerBornRote":
                            string[] valArr3 = e.InnerText.Split(',');
                            mc.playerBornRote = new Vector3(float.Parse(valArr3[0]), float.Parse(valArr3[1]), float.Parse(valArr3[2]));
                            break;
                        default:
                            break;
                    }
                }

                mapCfgsDict.Add(ID, mc);
            }


        }

    }
   public MapCfgs GetMapCfgData(int id)
    {
        MapCfgs data;
        if (mapCfgsDict.TryGetValue(id,out data))
        {
            return data;
        }
        return null;
    }

    #endregion

    #region 任务引导系统cfgs
    private Dictionary<int, AutoGuideCfgs> guideDict = new Dictionary<int, AutoGuideCfgs>();
    private void InitGuideCfgs(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.LogError("Xml file:" + path + "加载失败，请检查文件是否正确");
        }
        else
        {
            //建立一个XML
            XmlDocument doc = new XmlDocument();
            //读取XML内容
            doc.LoadXml(xml.text);
            //读取root下的子节点 成为一个队列
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            //读取节点数据
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfgs mc = new AutoGuideCfgs
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            mc.npcID = int.Parse(e.InnerText);
                            break;
                        case "dilogArr":
                            mc.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            mc.actID= int.Parse(e.InnerText);
                            break;
                        case "coin":
                            mc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            mc.exp = int.Parse(e.InnerText);
                            break;

                        default:
                            break;
                    }
                }

                guideDict.Add(ID, mc);
            }


        }
    }
    
    
    public AutoGuideCfgs GetAutoGuideCfgData(int id)
    {
        AutoGuideCfgs data;
        if (guideDict.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }

    #endregion

    #region 强化数据Cfgs
    //存储数据 根据位置获取这个位置所有的强化配置的字典，然后再到里面的字典获取某一个对应的星级的强化数据
    //简单来说  就是先获取某个装备的数据位置，在去获取该装备某一个星级的数据
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("xml file:" + path + " not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrongCfg sd = new StrongCfg
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos":
                            sd.pos = val;
                            break;
                        case "starlv":
                            sd.startLv = val;
                            break;
                        case "addhp":
                            sd.addHP = val;
                            break;
                        case "addhurt":
                            sd.addHurt = val;
                            break;
                        case "adddef":
                            sd.addef = val;
                            break;
                        case "minlv":
                            sd.minLv = val;
                            break;
                        case "coin":
                            sd.coin = val;
                            break;
                        case "crystal":
                            sd.crystal = val;
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongDic.TryGetValue(sd.pos, out dic))
                {
                    dic.Add(sd.startLv, sd);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(sd.startLv, sd);

                    strongDic.Add(sd.pos, dic);
                }
            }
        }
    }
    public StrongCfg GetStrongCfg(int pos, int starlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(starlv))
            {
                sd = dic[starlv];
            }
        }
        return sd;
    }

    public int GetPropAddValPreLv(int pos, int starlv, int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if (strongDic.TryGetValue(pos, out posDic))
        {
            for (int i = 0; i <=starlv; i++)
            {
                StrongCfg sd;
                if (posDic.TryGetValue(i, out sd))
                {
                    switch (type)
                    {
                        case 1://hp
                            val += sd.addHP;
                            break;
                        case 2://hurt
                            val += sd.addHurt;
                            break;
                        case 3://def
                            val += sd.addef;
                            break;
                    }
                }
            }
        }
        return val;
    }
    #endregion

    #region 任务奖励系统cfgs
    private Dictionary<int, TaskRewardCfg> taskRewardDict = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfgs(string path)
    {

        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            Debug.Log("xml file:" + path + " not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            //读取节点数据
            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                TaskRewardCfg mc = new TaskRewardCfg
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "count":
                            mc.count = int.Parse(e.InnerText);
                            break;
                        case "taskName":
                            mc.taskName = e.InnerText;
                            break;
                        case "coin":
                            mc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            mc.exp = int.Parse(e.InnerText);
                            break;

                        default:
                            break;
                    }

                }

                taskRewardDict.Add(ID, mc);
            }


        }
    }


    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg data;
        if (taskRewardDict.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    //public TaskRewardData CalcTaskRewardData(Player player, int rid)
    //{
    //    TaskRewardData trd = null;
    //    string[] taskArr = Common.Tools.GetTaskData(player.TaskArr);
    //    for (int i = 0; i < taskArr.Length; i++)
    //    {

    //        string[] taskInfo = taskArr[i].Split('|');
    //        //1|0|0
    //        if (int.Parse(taskInfo[0]) == rid)
    //        {
    //            trd = new TaskRewardData
    //            {
    //                ID = int.Parse(taskInfo[0]),
    //                prgs = int.Parse(taskInfo[1]),
    //                taked = taskInfo[2].Equals("1")
    //            };
    //            break;
    //        }
    //    }
    //    return trd;
    //}

    #endregion

}