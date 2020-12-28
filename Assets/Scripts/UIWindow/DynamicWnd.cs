/****************************************************
    文件：DynamicWnd.cs
	作者：Happy-11
    日期：2020/11/12 20:13:52
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DynamicWnd : WindowRoot 
{
    public Animation tipsAni;
    public Text txtTips;

    

    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(txtTips,false);
    }

    private Queue<string> tipsQue = new Queue<string>();
    private bool isTipsShow=false;
    public void AddTips(string tips)
    {
        //线程锁
        lock (tipsQue)
        {
            //限制队列数量
            if (tipsQue.Count<2)
            {
                tipsQue.Enqueue(tips);
            }
            
        }
    }

    public void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAni.GetClip("aniTips");
        tipsAni.Play();
        //延时关闭激活状态
        //使用协程  
        StartCoroutine(AniPlayDone(clip.length, () =>
         {
             SetActive(txtTips, false);
             isTipsShow = false;
         }));
    }

    private IEnumerator AniPlayDone(float sec,Action cb)
    {
        //延时时间
        yield return new WaitForSeconds(sec);
        //延时完成后进行委托内的方法
        //if (cb!=null)
        //{
        //    cb();
        //}
        cb?.Invoke();


    }

    private void Update()
    {
        if (tipsQue.Count>0&&isTipsShow==false)
        {
            lock(tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }
}