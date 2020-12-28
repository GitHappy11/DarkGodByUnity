/****************************************************
    文件：PlayerData.cs
	作者：Happy-11
    日期：2020/11/18 20:23:1
	功能：玩家数据保存(本地保存，危险行为，正式项目请勿模仿！！！)
*****************************************************/

using UnityEngine;

public  static class PlayerData  
{
    public static int id;
    public static int acct;
    public static int playerID;
    public static string playerName;
    public static int Level;
    public static int Exp;
    public static int Power;
    public static int Coin;
    public static int Diamond;
    public static int HP;
    public static int AD;
    public static int AP;
    public static int Addef;
    public static int Apdef;
    public static int Dodge;
    public static int Pierce;
    public static int Critical;
    public static int GuideID;
    public static int Crystal;
    public static int[] StrongArr=new int[6];
    public static string[] TaskArr = new string[6];
}