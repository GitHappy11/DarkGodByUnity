/****************************************************
    文件：ComTools.cs
	作者：Happy-11
    日期：2020/11/13 19:58:6
	功能：功能类
*****************************************************/

using UnityEngine;
public class ComTools  
{
    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <returns></returns>
    public static int RDint(int min,int max,System.Random rd=null)
    {
        if (rd==null)
        {
            rd = new System.Random();
        }
        //包含前面一个数，不包含后面一个数，所以后面数要+1
        int val = rd.Next(min, max + 1);
        return val;
    }
    public static void CalcExp(int addExp)
    {
        int addResExp = addExp;
        int curtExp = PlayerData.Exp;
        int curtLv = PlayerData.Level;
        while (true)
        {
            int upNeedExp = Common.Tools.GetExpUpValByLv(curtLv) - curtExp;
            if (addResExp >= upNeedExp)
            {
                curtLv += 1;
                curtExp = 0;
                addResExp -= upNeedExp;
            }
            else
            {
                PlayerData.Level = curtLv;
                PlayerData.Exp = curtExp + addResExp;
                break;
            }
        }
    }

}