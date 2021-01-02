/****************************************************
    文件：PlayerController.cs
	作者：Happy-11
    日期：2020/11/22 21:51:36
	功能：玩家角色控制器
*****************************************************/

using UnityEngine;

public class PlayerController : Controller
{
    
    private Vector3 camOffset;
    public CharacterController ctrl;

    public Transform camTrans;

    private float targetBlend;
    private float currenBlend;

    
    
    public void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

    }

    public void Update()
    {
        #region Input
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        ////向量规格化
        //Vector2 _dir = new Vector2(h, v).normalized;
        //if (_dir!=Vector2.zero)
        //{
        //    Dir = _dir;2
        //    SetBlend(Constants.BlendWalk);
        //}
        //else
        //{
        //    Dir = Vector2.zero;
        //    SetBlend(Constants.BlendIdel);
        //}
        #endregion
        if (currenBlend!=targetBlend)
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
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1))+camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (camTrans!=null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
        
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currenBlend-targetBlend)<Constants.AccelerSpeed*Time.deltaTime)
        {
            currenBlend = targetBlend;
        }
        else if(currenBlend>targetBlend)
        {
            currenBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currenBlend += Constants.AccelerSpeed * Time.deltaTime;
        }

        ani.SetFloat("Blend", currenBlend);
    }

}
