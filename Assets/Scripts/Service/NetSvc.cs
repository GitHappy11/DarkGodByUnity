/****************************************************
    文件：NetSvc.cs
	作者：Happy-11
    日期：2020/11/17 21:40:1
	功能：网络服务
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;

public class NetSvc : MonoBehaviour, IPhotonPeerListener
{
    public static NetSvc Instance = null;
    public void InitSvc()
    {
        Instance = this;
    }


    public static PhotonPeer Peer { get; private set; }
    private Dictionary<OperationCode, Request> RequestDict = new Dictionary<OperationCode, Request>();
    private Dictionary<EventCode, BaseEvent> EventDict = new Dictionary<EventCode, BaseEvent>();

    

    void Awake()
    {
        //通过Listener接收服务器端的响应
        Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        Peer.Connect("127.0.0.1:5055", "DarkGodServer");
    }
    void Start()
    {
        //网络事件接收初始化
        EventChat eventChat = new EventChat();
    }

    void Update()
    {
        Peer.Service();
    }

    void OnDestroy()
    {
        if (Peer != null && Peer.PeerState == PeerStateValue.Connected)
        {
            Peer.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.LogWarning("ServerDebugInfo:" + level + "----" + message);
    }
    public void OnEvent(EventData eventData)
    {
        EventCode code = (EventCode)eventData.Code;
        BaseEvent e = null;

        if (EventDict.TryGetValue(code, out e))
        {
            e.OnEvent(eventData);
        }
        else
        {
            Debug.LogWarning("没有找到对应的事件处理对象");
        }
    }
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;
        Request request = null;
        bool temp = RequestDict.TryGetValue(opCode, out request);

        if (temp)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            //Debug.Log("没找到对应的响应处理对象");
        }
    }
    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
    }
    public void AddRequest(Request request)        
    {

        if (RequestDict.ContainsKey(request.OpCode))
        {
            //RequestDict.Remove(request.OpCode);
        }
        else
        {
            RequestDict.Add(request.OpCode, request);
        }
                
    }
    public void RemoveRequest(Request request)
    {
        RequestDict.Remove(request.OpCode);
    }
    public void AddEvent(BaseEvent e)
    {
        EventDict.Add(e.eventCode, e);
    }
    public void RemoveEvent(BaseEvent e)
    {
        EventDict.Remove(e.eventCode);
    }


    
}

public class NetLogin : Request
{
    readonly int _acct;
    readonly string _password;
    public NetLogin(int acct ,string password)
    {
        OpCode = OperationCode.Login;
        _acct = acct;
        _password = password;
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)UserCode.Acct, _acct);
        data.Add((byte)UserCode.Password, _password);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ReturnCode.Success)
        {
            NetDataLoad.Player(operationResponse.Parameters);
            
            if (PlayerData.playerID!=0)
            {
                NetGetPlayerData netGetPlayerData = new NetGetPlayerData(PlayerData.playerID);
                LoginSys.Instance.RspEnterGame();
            }
            else
            {
                LoginSys.Instance.RspCreatePlayer();
            }
        }
        else if (operationResponse.ReturnCode == (short)ReturnCode.Default)
        {
            GameRoot.AddTips("登录失败，该账号已经在线！");
        }
        else
        {
            NetRegister netRegister = new NetRegister(_acct, _password);
        }
        NetSvc.Instance.RemoveRequest(this);

    }
    
}

public class NetRegister : Request
{
    readonly int _acct;
    readonly string _password;
    public NetRegister(int acct,string password)
    {
        OpCode = OperationCode.Register;
        _acct = acct;
        _password = password;
        DefaultRequest();
    }
    

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)UserCode.Acct, _acct);
        data.Add((byte)UserCode.Password, _password);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode==(short)ReturnCode.Success)
        {
            GameRoot.AddTips("注册成功！请创建角色！");
            NetLogin netLogin = new NetLogin(_acct, _password);
        }
        else
        {
            GameRoot.AddTips("注册失败！账号重复");
        }
        NetSvc.Instance.RemoveRequest(this);
    }
}

public class NetCreatPlayer:Request
{
    readonly string _playerName;
    public NetCreatPlayer(string playeName)
    {
        OpCode = OperationCode.CreatePlayer;
        _playerName = playeName;
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)PlayerCode.Name, _playerName);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode==(short)ReturnCode.Success)
        {
            PlayerData.playerName = _playerName;
            NetDataLoad.Player(operationResponse.Parameters);
            GameRoot.AddTips("角色创建成功！");
            MainCitySys.Instance.EnterMainCity();
        }
        else
        {
            GameRoot.AddTips("该名称重复，请重新输入！");
        }
        NetSvc.Instance.RemoveRequest(this);
    }
}

public class NetGetPlayerData : Request
{
    readonly int _playerID;
    readonly int _index;
    readonly LoadPlayeDataOverCode _loadPlayerDataCode;
    
    public NetGetPlayerData(int playerID,LoadPlayeDataOverCode loadPlayeDataOverCode=LoadPlayeDataOverCode.Defalut,int index=0)
    {
        OpCode = OperationCode.GetPlayerData;
        _loadPlayerDataCode = loadPlayeDataOverCode;
        _playerID = playerID;
        _index = index;
        DefaultRequest();
    }
    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)PlayerCode.PlayerID, _playerID);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ReturnCode.Success)
        {
            NetDataLoad.Player(operationResponse.Parameters);
            switch (_loadPlayerDataCode)
            {
                case LoadPlayeDataOverCode.Defalut:
                    break;
                case LoadPlayeDataOverCode.RefreshUIByMainCityWnd:
                    MainCitySys.Instance.RefreshUIByMainCityWnd();
                    break;
                case LoadPlayeDataOverCode.RefreshUIByStrongWnd:
                    MainCitySys.Instance.RefreshUIByStrongWnd(_index);
                    MainCitySys.Instance.RefreshUIByMainCityWnd();
                    TaskSys.Instance.CalcTaskPrgs(3);
                    break;
                case LoadPlayeDataOverCode.RefreshUIByTaskWnd:
                    MainCitySys.Instance.RefreshUIByMainCityWnd();
                    TaskSys.Instance.RefreshUITaskWnd();
                    break;
                default:
                    break;
            }
        }
        else
        {
            GameRoot.AddTips("该ID角色游戏数据读取失败！请检查该ID是否存在！");
        }
        NetSvc.Instance.RemoveRequest(this);

    }
}

public class NetUpdatPlayerData : Request
{

    
    public NetUpdatPlayerData()
    {
        OpCode = OperationCode.UpdatePlayerData;
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)PlayerCode.PlayerID, PlayerData.playerID);
        data.Add((byte)PlayerCode.Name,PlayerData.playerName);
        data.Add((byte)PlayerCode.Level,PlayerData.Level);
        data.Add((byte)PlayerCode.Exp,PlayerData.Exp);
        data.Add((byte)PlayerCode.Power,PlayerData.Power);
        data.Add((byte)PlayerCode.Coin,PlayerData.Coin);
        data.Add((byte)PlayerCode.Diamond,PlayerData.Diamond);
        data.Add((byte)PlayerCode.HP,PlayerData.HP);
        data.Add((byte)PlayerCode.AD,PlayerData.AD);
        data.Add((byte)PlayerCode.AP,PlayerData.AP);
        data.Add((byte)PlayerCode.Addef,PlayerData.Addef);
        data.Add((byte)PlayerCode.Apdef,PlayerData.Apdef);
        data.Add((byte)PlayerCode.Dodge,PlayerData.Dodge);
        data.Add((byte)PlayerCode.Pierce,PlayerData.Pierce);
        data.Add((byte)PlayerCode.Critical,PlayerData.Critical);
        data.Add((byte)PlayerCode.Crystal, PlayerData.Crystal);
        data.Add((byte)PlayerCode.GuideID,PlayerData.GuideID);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ReturnCode.Success)
        {
            NetGetPlayerData netGetPlayerData = new NetGetPlayerData(PlayerData.playerID,LoadPlayeDataOverCode.RefreshUIByMainCityWnd);     
        }
        else
        {
            
        }
        NetSvc.Instance.RemoveRequest(this);
        
    }
}

public class NetStrong : Request
{
    readonly int _pos;

    public NetStrong(int pos)
    {
        OpCode = OperationCode.Strong;
        _pos = pos;
        DefaultRequest();
    }
    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)PlayerCode.PlayerID, PlayerData.playerID);
        data.Add((byte)PlayerCode.StrongPos, _pos);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ReturnCode.Success)
        {

            GameRoot.AddTips("强化成功");
            NetGetPlayerData netGetPlayerData = new NetGetPlayerData(PlayerData.playerID, LoadPlayeDataOverCode.RefreshUIByStrongWnd, _pos);
            
        }
        else
        {

        }
        NetSvc.Instance.RemoveRequest(this);
    }
}
public class NetTask : Request
{
    public NetTask()
    {
        OpCode = OperationCode.Task;
        DefaultRequest();
    }
    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)PlayerCode.PlayerID, PlayerData.playerID);
        data.Add((byte)PlayerCode.TaskArr, Tools.GetTaskData(PlayerData.TaskArr));
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ReturnCode.Success)
        {

            GameRoot.AddTips("任务数据更新成功");
        }
        else
        {

        }
        NetSvc.Instance.RemoveRequest(this);
    }
}

public class NetChat:Request
{
    readonly string _chatContent;
    public NetChat(string chatContent)
    {
        OpCode = OperationCode.Chat;
        _chatContent = chatContent;
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ChatCode.Content, _chatContent);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if(operationResponse.ReturnCode==(short)ReturnCode.Success)
        {
            GameRoot.AddTips("发送成功！");
        }
        else
        {
            GameRoot.AddTips("发送失败!");
        }
        NetSvc.Instance.RemoveRequest(this);
    }
    
}
public class NetBuy : Request
{
    BuyCode _buyCode;

    public NetBuy(BuyCode buyCode)
    {
        OpCode = OperationCode.Buy;
        _buyCode = buyCode;
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)_buyCode, null);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            switch (_buyCode)
            {
                case BuyCode.Coin:
                    GameRoot.AddTips("充值成功！！");
                    TaskSys.Instance.CalcTaskPrgs(5);
                    break;
                case BuyCode.Power:
                    GameRoot.AddTips("体力购买成功！！");
                    TaskSys.Instance.CalcTaskPrgs(4);
                    break;
                default:
                    break;
            }
        }
        NetGetPlayerData netGetPlayerData = new NetGetPlayerData(PlayerData.playerID, LoadPlayeDataOverCode.RefreshUIByMainCityWnd);
        NetSvc.Instance.RemoveRequest(this);
    }
}

public class NetReqTask : Request
{
    private readonly int _tid;

    public NetReqTask(int tid)
    {
        OpCode = OperationCode.ReqTask;
        _tid = tid;

        DefaultRequest();
    }
    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)TaskCode.Tid, _tid);
        NetSvc.Peer.OpCustom((byte)OpCode, data, true);
        NetSvc.Instance.AddRequest(this);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (byte)ReturnCode.Success)
        {
            TaskRewardCfg trc = ResSvc.Instance.GetTaskRewardCfg(_tid);
            GameRoot.AddTips("任务奖励领取成功！金币+"+trc.coin+"--经验+"+trc.exp);
  
            ComTools.CalcExp(trc.exp);
            
        }
  
        NetGetPlayerData netGetPlayerData2= new NetGetPlayerData(PlayerData.playerID, LoadPlayeDataOverCode.RefreshUIByTaskWnd);
        
        
        NetSvc.Instance.RemoveRequest(this);
    }
}



#region 网络事件接收器

public class EventChat : BaseEvent
{
    string name;
    string chatContent;

    public EventChat()
    {
        eventCode = EventCode.Chat;
        NetSvc.Instance.AddEvent(this);
    }

    public override void OnEvent(EventData eventData)
    {
        name = (string)Tools.DictGetValue<byte, object>(eventData.Parameters, (byte)ChatCode.name);
        chatContent = (string)Tools.DictGetValue<byte, object>(eventData.Parameters, (byte)ChatCode.Content);
        //接收的信息处理
        MainCitySys.Instance.RshChatMsg(name, chatContent);
    }
}

#endregion

public enum LoadPlayeDataOverCode
{
    Defalut,
    RefreshUIByMainCityWnd,
    RefreshUIByStrongWnd,
    RefreshUIByTaskWnd,
}



