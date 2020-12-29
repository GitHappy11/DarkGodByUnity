/****************************************************
    文件：NetDataLoad.cs
	作者：Happy-11
    日期：2020/11/18 21:49:7
	功能：服务端数据加载
*****************************************************/


using Common;
using System.Collections.Generic;
using UnityEngine;
public static class NetDataLoad  
{
    public  static void User(Dictionary<byte,object> data)
    {
        foreach (var item in data)
        {
            switch (item.Key)
            {
                case (byte)UserCode.ID:
                    PlayerData.id = int.Parse(item.Value.ToString());
                    break;
                case (byte)UserCode.Acct:
                    PlayerData.acct = int.Parse(item.Value.ToString());
                    break;
                case (byte)UserCode.PlayerID:
                    PlayerData.playerName = item.Value.ToString();
                    break;
                default:
                    break;
            }
        }
    }
    public static void Player(Dictionary<byte, object> data)
    {

        foreach (var item in data)
        {
            switch (item.Key)
            {
                case (byte)PlayerCode.PlayerID:
                    PlayerData.playerID = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Name:
                    PlayerData.playerName = item.Value.ToString();
                    break;
                case (byte)PlayerCode.Level:
                    PlayerData.Level = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Exp:
                    PlayerData.Exp = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Power:
                    PlayerData.Power = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Coin:
                    PlayerData.Coin = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Diamond:
                    PlayerData.Diamond = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.HP:
                    PlayerData.HP = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.AD:
                    PlayerData.AD = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.AP:
                    PlayerData.AP = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Addef:
                    PlayerData.Addef = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Apdef:
                    PlayerData.Apdef = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Dodge:
                    PlayerData.Dodge = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Pierce:
                    PlayerData.Pierce = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Critical:
                    PlayerData.Critical = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.GuideID:
                    PlayerData.GuideID = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Crystal:
                    PlayerData.Crystal = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.Fuben:
                    PlayerData.Fuben = int.Parse(item.Value.ToString());
                    break;
                case (byte)PlayerCode.StrongArr:
                    PlayerData.StrongArr = Tools.GetPlayerStrongData((item.Value.ToString()));     
                    break;
                case (byte)PlayerCode.TaskArr:
                    PlayerData.TaskArr = Tools.GetTaskData((item.Value.ToString()));
                    break;
                default:
                    break;
            }
        }
        
    }
    



}