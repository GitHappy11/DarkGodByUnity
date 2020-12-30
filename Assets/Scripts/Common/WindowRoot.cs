/****************************************************
    文件：WindowRoot.cs
	作者：Happy-11
    日期：2020/11/12 16:35:30
	功能：UI界面基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour 
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    /// <summary>
    /// 改变窗口状态，默认打开窗口
    /// </summary>
    /// <param name="isActive"></param>
    public void SetWndState(bool isActive=true)
    {
        if (gameObject.activeSelf!=isActive)
        {
            SetActive(gameObject, isActive);
        }
        //打开窗口的时候初始化窗口，否则清理窗口
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }
    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }
    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
    }


    #region 文字组件信息改变
    /// <summary>
    /// 更改Text组件文字信息
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="context"></param>
    protected void SetText(Text txt,string context="")
    {
        txt.text = context;
    }
    protected void SetText(Transform trans, int num=0)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
    protected void SetText(Transform trans, string context="")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num=0)
    {
        SetText(txt, num.ToString());
    }
    #endregion

    #region 组件激活状态改变
    protected void SetActive(GameObject go,bool isActive=true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }
    #endregion

    #region 图片改变
    public void SetSprite(Image img,string path)
    {
        if (resSvc!=null)
        {
            Sprite sp = resSvc.LoadSprite(path, true);
            img.sprite = sp;
        }
        
    }
    #endregion
    //检测游戏对象上是否有相关组件如果没有则获取   检测范围（只能是可以被挂载的组件）
    protected T GetOrAddComponect<T>(GameObject go) where T:Component
    {
        T t = go.GetComponent<T>();
        if (t==null)
        {
            t = go.AddComponent<T>();
        }
        return t;

    }

    //寻找一个transform游戏对象
    protected Transform GetTrans(Transform trans,string name)
    {
        if (trans!=null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    }

    #region 监听鼠标事件
    protected void OnClick(GameObject go, Action<object> cb, object args)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClick = cb;
        listener.args = args;
    }

    protected void OnClickDrag(GameObject go,Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickDrag = cb;
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickDown = cb;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponect<PEListener>(go);
        listener.onClickUp = cb;
    }

    #endregion
}


