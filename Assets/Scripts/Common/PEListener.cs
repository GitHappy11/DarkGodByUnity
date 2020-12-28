/****************************************************
    文件：PEListener.cs
	作者：Happy-11
    日期：2020/11/22 19:55:58
	功能：UI事件监听
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PEListener : MonoBehaviour,IPointerClickHandler,IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    //不需要鼠标参数，只要一个监听事件
    public Action<object> onClick;
    public Action<PointerEventData> onClickDown;
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onClickDrag;

    public object args;

    //拖动事件
    public void OnDrag(PointerEventData eventData)
    {
        onClickDrag?.Invoke(eventData);
    }
    //点击事件
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(args);
    }

    //点击按下去的事件
    public void OnPointerDown(PointerEventData eventData)
    {
        onClickDown?.Invoke(eventData);
    }

    //鼠标松开的事件
    public void OnPointerUp(PointerEventData eventData)
    {
        onClickUp?.Invoke(eventData);
    }

}