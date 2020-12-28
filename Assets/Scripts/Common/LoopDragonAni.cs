/****************************************************
    文件：LoopDragonAni.cs
	作者：Happy-11
    日期：2020/11/12 20:5:51
	功能：飞龙循环动画
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour 
{
    private Animation ani;

    private void Awake()
    {
        ani = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        if (ani!=null)
        {
            //重复调用方法 方法名  过多久开始   间隔时间
            InvokeRepeating("PlayDragonAni", 0, 20);
        }
    }

    private void PlayDragonAni()
    {
        if (ani!=null)
        {
            ani.Play();
        }
    }
}