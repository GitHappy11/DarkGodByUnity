/****************************************************
    文件：DeBugWnd.cs
	作者：Happy-11
    日期：2020/11/21 22:13:19
	功能：Debug用界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class DeBugWnd :WindowRoot  
{
    public Text txtFuben;
    public Text txtGuideID;
    public Text txtStrongArr;
    public Text txtTaskArr;
    private void OnEnable()
    {
        txtFuben.text = "当前副本进度："+PlayerData.Fuben;

        txtGuideID.text= "当前任务ID：" + PlayerData.GuideID.ToString();

        txtStrongArr.text = "当前装备列表：" + PlayerData.StrongArr[0];

        txtTaskArr.text = "当前任务列表：" + PlayerData.TaskArr[0]+ PlayerData.TaskArr[1]+PlayerData.TaskArr[2]+PlayerData.TaskArr[3]+ PlayerData.TaskArr[4]+ PlayerData.TaskArr[5];

        
    }
}