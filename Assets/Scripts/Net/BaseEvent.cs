/****************************************************
    文件：BaseEvent.cs
	作者：Happy-11
    日期：2020/11/17 22:7:33
	功能：网络事件发送接收
*****************************************************/

using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseEvent  
{
    public EventCode eventCode;
    public abstract void OnEvent(EventData eventData);

   
}