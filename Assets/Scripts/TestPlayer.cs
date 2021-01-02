/****************************************************
    文件：TestPlayer.cs
	作者：Happy-11
    日期：2021/1/2 12:48:36
	功能：测试主角脚本
*****************************************************/

using System.Collections;
using UnityEngine;

public class TestPlayer : MonoBehaviour 
{

    private Vector3 camOffset;
    public CharacterController ctrl;

    public Transform camTrans;

    private float targetBlend;
    private float currenBlend;

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



    public void Start()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

    }
    public void Update()
    {
        #region Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //向量规格化
        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero)
        {
            Dir = _dir; 
            SetBlend(Constants.BlendMove);
        }
        else
        {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdel);
        }
        #endregion
        if (currenBlend != targetBlend)
        {
            UpdateMixBlend();
        }


        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }

    }
    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }
    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }
    public  void SetBlend(float blend)
    {
        targetBlend = blend;

    }
    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currenBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currenBlend = targetBlend;
        }
        else if (currenBlend > targetBlend)
        {
            currenBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currenBlend += Constants.AccelerSpeed * Time.deltaTime;
        }

        ani.SetFloat("Blend", currenBlend);
    }


    public void ClickSkill1Btn()
    {

        //播放动画Action  1
        //先设置事件  然后再设置什么时候触发这个事件  具体操作看暗黑战神教程202课时
        ani.SetInteger("Action", 1);

        StartCoroutine("Delay");
    }


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        ani.SetInteger("Action", -1);
    }
}