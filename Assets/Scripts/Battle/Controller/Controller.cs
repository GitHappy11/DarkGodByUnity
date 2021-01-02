/****************************************************
    文件：Controller.cs
	作者：Happy-11
    日期：2021/1/2 11:38:6
	功能：表现实体控制器基类
*****************************************************/

using UnityEngine;

public abstract class Controller : MonoBehaviour 
{
    public Animator ani;
    protected bool isMove = false;


    
    
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }

    }

    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend", blend);
    }
    
}