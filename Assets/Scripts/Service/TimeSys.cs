/****************************************************
    文件：PETimer.cs
	作者：Happy-11
    日期：2020年12月4日21:09:30
	功能：服务端使用定时系统
*****************************************************/


using UnityEngine;
using System;
using System.Collections.Generic;

public class TimeSys : MonoBehaviour
{
    public static TimeSys instance = null;

    public void InitSvc()
    {
        instance = this;
    }
    private Action<string> taskLog;

    //使用计算机元年进行计时
    private DateTime starDatetTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private double nowTime;
    public TimeSys()
    {
        tIDList.Clear();
        recList.Clear();
        tmpTimeList.Clear();
        taskTimeList.Clear();
        tmpFrameList.Clear();
        taskFrameList.Clear();
    }


    //设置一个锁 防止同时生成 导致相同的ID
    private static readonly string obj = "lock";
    //设置一个全局的tID 进行赋值
    private int tID;
    //以ID来保存所有任务
    private List<int> tIDList = new List<int>();
    //需要进行回收的ID
    private List<int> recList = new List<int>();


    //缓存任务列表
    private List<PETimeTask> tmpTimeList = new List<PETimeTask>();
    //定时任务列表
    private List<PETimeTask> taskTimeList = new List<PETimeTask>();


    //获取当前帧
    private int frameCounter;
    //缓存帧任务列表
    private List<PETrameTask> tmpFrameList = new List<PETrameTask>();
    //帧任务列表
    private List<PETrameTask> taskFrameList = new List<PETrameTask>();



    #region 定时任务
    /// <summary>
    /// 定时系统任务添加 
    /// </summary>
    /// <param name="callback">要执行的任务</param>
    /// <param name="delay">延时时间</param>
    /// <param name="time">时间单位</param>
    /// <param name="count">执行次数（0为执行无数次）</param>
    /// <returns>新建一个任务后返回一个任务ID</returns>
    public int AddTimeTask(Action<int> callback, double delay, PEtimeUnit timeUnit = PEtimeUnit.Second, int count = 1)
    {
        switch (timeUnit)
        {
            case PEtimeUnit.MilliSecond:
                delay = delay / 1000;
                break;
            case PEtimeUnit.Second:
                //delay = delay;
                break;
            case PEtimeUnit.Minute:
                delay = delay * 60;
                break;
            case PEtimeUnit.Hour:
                delay = delay * 60 * 60;
                break;
            case PEtimeUnit.Day:
                delay = delay * 60 * 60 * 24;
                break;
            default:
                break;
        }


        //获得一个唯一的任务ID
        int tID = GetTaskID();
        //触发时间=当前时间（游戏启动到现在经历的时间）+需要延迟的时间
        nowTime = GetUTCSeconds() + delay;

        //新建一个定时任务 给这个任务设定数据 

        //增加进缓存任务列表
        tmpTimeList.Add(new PETimeTask(tID, callback, nowTime, delay, count));


        tIDList.Add(tID);
        return tID;
    }
    /// <summary>
    /// 删除定时任务
    /// </summary>
    /// <param name="tID">任务ID</param>
    /// <returns></returns>
    public bool DelTimeTask(int tID)
    {
        //是否已经找到这个任务
        bool exist = false;
        //先去任务列表中寻找这个任务
        for (int i = 0; i < taskTimeList.Count; i++)
        {
            PETimeTask task = taskTimeList[i];
            //任务列表中找到了 就在列表中删除这个任务
            if (task.tID == tID)
            {
                taskTimeList.RemoveAt(i);
                //删除任务后，再从tid合集中把这个tid删除，让tid空闲出来，给其他的任务使用
                for (int j = 0; j < tIDList.Count; j++)
                {
                    if (tIDList[j] == tID)
                    {
                        tIDList.RemoveAt(j);
                        break;
                    }
                }
                exist = true;
                break;
            }

        }
        //如果任务列表中没有找到这个任务，那就去缓存任务列表找
        if (!exist)
        {
            for (int i = 0; i < tmpTimeList.Count; i++)
            {
                PETimeTask task = tmpTimeList[i];
                //任务列表中找到了 就在列表中删除这个任务
                if (task.tID == tID)
                {
                    tmpTimeList.RemoveAt(i);
                    //删除任务后，再从tid合集中把这个tid删除，让tid空闲出来，给其他的任务使用
                    for (int j = 0; j < tIDList.Count; j++)
                    {
                        if (tIDList[j] == tID)
                        {
                            tIDList.RemoveAt(j);
                            break;
                        }
                    }
                    exist = true;
                    break;
                }
            }
        }
        return exist;

    }
    //替换任务
    public bool ReplaceTimeTask(Action<int> callback, float delay, PEtimeUnit timeUnit = PEtimeUnit.Second, int count = 1)
    {
        switch (timeUnit)
        {
            case PEtimeUnit.MilliSecond:
                delay = delay / 1000;
                break;
            case PEtimeUnit.Second:
                //delay = delay;
                break;
            case PEtimeUnit.Minute:
                delay = delay * 60;
                break;
            case PEtimeUnit.Hour:
                delay = delay * 60 * 60;
                break;
            case PEtimeUnit.Day:
                delay = delay * 60 * 60 * 24;
                break;
            default:
                break;
        }
        //触发时间=当前时间（游戏启动到现在经历的时间）+需要延迟的时间
        nowTime = GetUTCSeconds() + delay;
        //新建这个task  
        PETimeTask newTask = new PETimeTask(tID, callback, nowTime, delay, count);
        bool isRep = false;
        //去任务列表中寻找这个tid的任务，把它替换掉
        for (int i = 0; i < taskTimeList.Count; i++)
        {
            if (taskTimeList[i].tID == tID)
            {
                taskTimeList[i] = newTask;
                isRep = true;
                break;
            }
        }
        //没有找到的话就去缓存列表里面找
        if (!isRep)
        {
            for (int i = 0; i < tmpTimeList.Count; i++)
            {
                if (tmpTimeList[i].tID == tID)
                {
                    tmpTimeList[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }
        return isRep;
    }
    #endregion

    #region 帧定时任务
    /// <summary>
    /// 帧定时系统任务添加 
    /// </summary>
    /// <param name="callback">要执行的任务</param>
    /// <param name="delay">延时帧</param>
    /// <param name="count">执行次数（0为执行无数次）</param>
    /// <returns>新建一个任务后返回一个任务ID</returns>
    public int AddFrameTask(Action<int> callback, int delay, int count = 1)
    {
        //获得一个唯一的任务ID
        int tID = GetTaskID();
        //新建一个定时任务 给这个任务设定数据 
        //增加进缓存任务列表
        tmpFrameList.Add(new PETrameTask(tID, callback, frameCounter + delay, delay, count));
        tIDList.Add(tID);
        return tID;
    }
    /// <summary>
    /// 删除定时任务
    /// </summary>
    /// <param name="tID">任务ID</param>
    /// <returns></returns>
    public bool DelFrameTask(int tID)
    {
        //是否已经找到这个任务
        bool exist = false;
        //先去任务列表中寻找这个任务
        for (int i = 0; i < taskFrameList.Count; i++)
        {
            PETrameTask task = taskFrameList[i];
            //任务列表中找到了 就在列表中删除这个任务
            if (task.tID == tID)
            {
                taskFrameList.RemoveAt(i);
                //删除任务后，再从tid合集中把这个tid删除，让tid空闲出来，给其他的任务使用
                for (int j = 0; j < tIDList.Count; j++)
                {
                    if (tIDList[j] == tID)
                    {
                        tIDList.RemoveAt(j);
                        break;
                    }
                }
                exist = true;
                break;
            }

        }
        //如果任务列表中没有找到这个任务，那就去缓存任务列表找
        if (!exist)
        {
            for (int i = 0; i < tmpFrameList.Count; i++)
            {
                PETrameTask task = tmpFrameList[i];
                //任务列表中找到了 就在列表中删除这个任务
                if (task.tID == tID)
                {
                    tmpFrameList.RemoveAt(i);
                    //删除任务后，再从tid合集中把这个tid删除，让tid空闲出来，给其他的任务使用
                    for (int j = 0; j < tIDList.Count; j++)
                    {
                        if (tIDList[j] == tID)
                        {
                            tIDList.RemoveAt(j);
                            break;
                        }
                    }
                    exist = true;
                    break;
                }
            }
        }
        return exist;

    }
    //替换任务
    public bool ReplaceFrameTask(Action<int> callback, int delay, int count = 1)
    {


        //新建这个task  
        PETrameTask newTask = new PETrameTask(tID, callback, frameCounter + delay, delay, count);
        bool isRep = false;
        //去任务列表中寻找这个tid的任务，把它替换掉
        for (int i = 0; i < taskFrameList.Count; i++)
        {
            if (taskFrameList[i].tID == tID)
            {
                taskFrameList[i] = newTask;
                isRep = true;
                break;
            }
        }
        //没有找到的话就去缓存列表里面找
        if (!isRep)
        {
            for (int i = 0; i < tmpFrameList.Count; i++)
            {
                if (tmpFrameList[i].tID == tID)
                {
                    tmpFrameList[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }
        return isRep;
    }
    #endregion
    //检测定时任务是否应该执行
    public void CheckTimeTask()
    {
        //如果临时列表里还有未取出的任务，就上锁，防止添加任务的时候被清理

        nowTime = GetUTCSeconds();
        //取出缓存区的任务，将里面的任务加入到即将执行的任务列表中
        for (int tmpIndex = 0; tmpIndex < tmpTimeList.Count; tmpIndex++)
        {
            taskTimeList.Add(tmpTimeList[tmpIndex]);
        }
        //缓存任务全部取出以后 清空缓存列表 以空出索引 好进行下次遍历
        tmpTimeList.Clear();

        for (int index = 0; index < taskTimeList.Count; index++)
        {

            PETimeTask task = taskTimeList[index];
            //当前时间是否小于这个任务里面的执行时间
            if (nowTime < task.destTime)
            {
                //如果小于，说明时间还没到。就跳出，检测下一个任务
                continue;
            }
            else
            {
                //如果时间到了  就执行里面的任务（委托） 使用try捕获委托中可能出现的异常，防止因为委托的原因导致定时系统崩溃
                try
                {
                    task.callback?.Invoke(task.tID);
                }
                catch (Exception e)
                {

                    LogInfo(e.ToString());
                }


                //次数消耗完毕后移除任务
                if (task.count == 1)
                {
                    //执行完毕后 移除任务
                    taskTimeList.Remove(task);
                    //索引提前 继续遍历  如果觉得不妥 可以新建一个要执行的任务列表 进行循环执行
                    index--;
                    //回收tID
                    recList.Add(task.tID);
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    //下次执行的时间为目标时间加上延迟的时间
                    task.destTime += task.delay;
                }


            }
            //如果有需要回收的ID 就执行回收函数 回收掉这些ID
            if (recList.Count > 0)
            {
                RecycleTid();
            }

        }
    }
    //检测帧任务是否应该执行
    public void CheckFrameTask()
    {
        //取出缓存区的任务，将里面的任务加入到即将执行的任务列表中
        for (int tmpIndex = 0; tmpIndex < tmpFrameList.Count; tmpIndex++)
        {
            taskFrameList.Add(tmpFrameList[tmpIndex]);
        }
        //缓存任务全部取出以后 清空缓存列表 以空出索引 好进行下次遍历
        tmpFrameList.Clear();

        //update每秒都会加帧
        frameCounter += 1;
        for (int index = 0; index < taskFrameList.Count; index++)
        {

            PETrameTask task = taskFrameList[index];
            //当前帧是否小于这个任务里面的帧
            if (frameCounter < task.destFrame)
            {
                //如果小于，说明时间还没到。就跳出，检测下一个任务
                continue;
            }
            else
            {
                //如果时间到了  就执行里面的任务（委托） 使用try捕获委托中可能出现的异常，防止因为委托的原因导致定时系统崩溃
                try
                {
                    task.callback?.Invoke(task.tID);
                }
                catch (Exception e)
                {

                    LogInfo(e.ToString());
                }


                //次数消耗完毕后移除任务
                if (task.count == 1)
                {
                    //执行完毕后 移除任务
                    taskFrameList.Remove(task);
                    //索引提前 继续遍历  如果觉得不妥 可以新建一个要执行的任务列表 进行循环执行
                    index--;
                    //回收tID
                    recList.Add(task.tID);
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    //下次执行的时间为目标时间加上延迟的时间
                    task.destFrame += task.delay;
                }


            }
            //如果有需要回收的ID 就执行回收函数 回收掉这些ID
            if (recList.Count > 0)
            {
                RecycleTid();
            }

        }
    }

    #region 工具类



    public int GetTaskID()
    {
        //加锁防止同时生成
        lock (obj)
        {
            tID += 1;
            //安全代码，以防万一溢出
            while (true)
            {
                if (tID == int.MaxValue)
                {
                    tID = 0;
                }
                //检查使用的ID是否已经被占用
                bool used = false;
                for (int i = 0; i < tIDList.Count; i++)
                {
                    if (tID == tIDList[i])
                    {
                        used = true;
                        break;
                    }
                }
                //如果没有被使用 就可以跳出安全监测了
                if (!used)
                {
                    break;
                }
                //如果被使用了 那就把tID+1 然后再进行检测  直到tid没有被使用为止
                else
                {
                    tID += 1;
                }

            }

        }
        return tID;
    }
    //回收Tid
    public void RecycleTid()
    {
        for (int i = 0; i < recList.Count; i++)
        {
            int tID = recList[i];
            for (int j = 0; j < tIDList.Count; j++)
            {
                if (tIDList[j] == tID)
                {
                    tIDList.RemoveAt(j);
                    break;
                }
            }
        }
        recList.Clear();
    }

    //C#高级日志工具
    public void SetLog(Action<string> log)
    {
        taskLog = log;
    }
    //调用日志工具
    private void LogInfo(string log)
    {
        taskLog?.Invoke(log);
    }
    //计时工具
    public double GetUTCSeconds()
    {
        //现在的时间剪去计算机元年的时间  
        TimeSpan ts = DateTime.UtcNow - starDatetTime;
        //这中间所经过的秒数
        return ts.TotalSeconds;
    }
    //重置函数
    public void Reset()
    {
        tID = 0;
        tIDList.Clear();
        recList.Clear();
        tmpTimeList.Clear();
        taskTimeList.Clear();
        tmpFrameList.Clear();
        taskFrameList.Clear();
    }
    //获取时间
    public double GetSecondsTime()
    {
        return nowTime;
    }

    //获取本地日期
    public DateTime GetLocalDateTime()
    {
        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(starDatetTime.AddMilliseconds(nowTime));
        return dt;
    }

    //获取年月日
    public int GetYear()
    {
        return GetLocalDateTime().Year;
    }
    public int GetMonth()
    {
        return GetLocalDateTime().Month;
    }
    public int GetDay()
    {
        return GetLocalDateTime().Day;
    }
    public int GetWeek()
    {
        return (int)GetLocalDateTime().DayOfWeek;
    }
    //获取当前时间的字符串
    public string GetLoaclTimeStr()
    {
        DateTime dt = GetLocalDateTime();
        string str = GetTimeStr(dt.Hour) + ":" + GetTimeStr(dt.Minute) + ":" + GetTimeStr(dt.Second);
        return str;
    }
    private string GetTimeStr(int time)
    {
        if (time < 10)
        {
            return "0" + time;
        }
        else
        {
            return time.ToString();
        }
    }
    #endregion
    private void Update()
    {
        CheckTimeTask();
        CheckFrameTask();
    }
}
