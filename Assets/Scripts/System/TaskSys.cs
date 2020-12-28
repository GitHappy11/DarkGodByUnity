/****************************************************
    文件：TaskSys.cs
	作者：Happy-11
    日期：2020/12/28 22:9:32
	功能：任务系统
*****************************************************/

using UnityEngine;

public class TaskSys : SystemRoot 
{
    public static TaskSys Instance=null;
    public TaskWnd taskWnd;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
    }

    public void CalcTaskPrgs(int tid)
    {
        TaskRewardCfg trd = resSvc.GetTaskRewardCfg(tid);
        string[] taskInfo = new string[5];
        for (int i = 0; i < PlayerData.TaskArr.Length; i++)
        {

            taskInfo = PlayerData.TaskArr[i].Split('|');
            //1|0|0
            if (int.Parse(taskInfo[0]) == tid)
            {
                if (int.Parse(taskInfo[1])>=trd.count)
                {
                    
                    taskInfo[1] = trd.count.ToString();
                    break;
                }
                else
                {
                    int index = int.Parse(taskInfo[1]);
                  
                    index += 1;
                    taskInfo[1] = index.ToString();
                    
                }

                PlayerData.TaskArr[i]= taskInfo[0] + "|" + taskInfo[1] + "|" + taskInfo[2];
                NetTask netTask = new NetTask();
                break;
            }
        }
        
    }
    public void GetTaskPrgs(int tid)
    {

    }
    public void RefreshUITaskWnd()
    {
        taskWnd.RefreshUI();
    }
}