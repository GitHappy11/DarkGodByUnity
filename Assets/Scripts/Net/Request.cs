/****************************************************
    文件：Request.cs
	作者：Happy-11
    日期：2020/11/17 22:7:15
	功能：网络数据上传服务端基类
*****************************************************/

using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public  abstract class Request
{ 
    public OperationCode OpCode;
    public abstract void OnOperationResponse(OperationResponse operationResponse);
    public abstract void DefaultRequest();

}