/****************************************************
    文件：Constants.cs
	作者：Happy-11
    日期：2020/11/12 10:56:55
	功能：常量配置
*****************************************************/

using UnityEngine;
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Constants 
{

    private const string ColorRed="<color=#FF0000FF>";
    private const string ColorGreen="<color=#00FF00FF>";
    private const string ColorBlue="<color=#00B4FFFF>";
    private const string ColorYellow="<color=#FFFF00FF>";
    private const string ColorEnd="</color>";

    public static string Color(string str,TxtColor color)
    {
        string result = "";
        switch (color)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
          
        }
        return  result;
    }


    //场景名称
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "SceneMainCity";

    //登录音乐名称
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";
    //登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";
    //普通的UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string FBItemEnter = "fbitem";


    //AutoGuideNPCID
    public const int NPCWiseMan= 0;
    public const int NPCGeneral= 1;
    public const int NPCArtisan= 2;
    public const int NPCTrader= 3;

    

    //屏幕标准宽高比
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;
    //摇杆点标准距离
    public const int ScreenOPDis = 90;

    //角色移动速度
    public const int PlayerMoveSpeed= 8;
    public const int MonsterMoveSpeed= 4;

    //运动平滑
    public const float AccelerSpeed = 5;


    //混合参数
    public const int BlendIdel = 0;
    public const int BlendWalk = 1;
}