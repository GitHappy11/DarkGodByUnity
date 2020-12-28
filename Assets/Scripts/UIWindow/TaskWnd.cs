/****************************************************
    文件：TaskWnd.cs
	作者：Happy-11
    日期：2020/12/6 21:3:16
	功能：任务界面
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskWnd : WindowRoot 
{
    public Transform scrollTrans;
    private List<TaskRewardData> trdList = new List<TaskRewardData>();
    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }


    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void RefreshUI()
    {
        //刷新前先把之前的删除
        for (int i = 0; i < scrollTrans.childCount; i++)
        {
            Destroy(scrollTrans.GetChild(i).gameObject);
        }

        //清理列表
        trdList.Clear();
        //临时列表
        //没有完成的任务列表
        List<TaskRewardData> todoList = new List<TaskRewardData>();
        //已经完成的任务列表
        List<TaskRewardData> doneList = new List<TaskRewardData>();
        for (int i = 0; i < PlayerData.TaskArr.Length-1; i++)
        {
            string[] taskInfo = PlayerData.TaskArr[i].Split('|');

         
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1"),
            };

            if (trd.taked)
            {
                doneList.Add(trd);
            }
            else
            {
                todoList.Add(trd);
            }
        }
        //按照先后顺序存入
        trdList.AddRange(todoList);
        trdList.AddRange(doneList);

        

        for (int i = 0; i < trdList.Count; i++)
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollTrans);

            go.name = "taskItem_" + i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trf = resSvc.GetTaskRewardCfg(trd.ID);
            

            SetText(GetTrans(go.transform, "txtTaskName"),trf.taskName);
            SetText(GetTrans(go.transform, "txtPrg"),trd.prgs+"/"+trf.count);
            SetText(GetTrans(go.transform, "txtExp"),"经验："+trf.exp);
            SetText(GetTrans(go.transform, "txtCoin"),"金币："+trf.coin);

            Image imgPrg=GetTrans(go.transform, "prgBar/prgVal").GetComponent<Image>();
            float prgVal = trd.prgs * 1.0f / trf.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = GetTrans(go.transform, "btnTask").GetComponent<Button>();
            btnTake.onClick.AddListener(() =>
            {
                ClickTaskBtn(go.name);
            });

            Transform transComp = GetTrans(go.transform, "imgCount");
            if (trd.taked)
            {
                btnTake.interactable = false;
                SetActive(transComp);
            }
            else
            {
                SetActive(transComp, false);
                if (trd.prgs==trf.count)
                {
                    btnTake.interactable=true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }
        }
        
           
    }

    private void ClickTaskBtn(string name)
    {
        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);
        NetReqTask netReqTask = new NetReqTask(trdList[index].ID);
    }
}