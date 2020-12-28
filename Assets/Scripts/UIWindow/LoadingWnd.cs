/****************************************************
    文件：LoadingWnd.cs
	作者：Happy-11
    日期：2020/11/12 11:6:26
	功能：加载界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    public Text txtTips;
    public Image imgFG;
    public Image imgPoint;
    public Text txtPrg;

    private float fgWidth;
    /// <summary>
    ///初始化
    /// </summary>
    protected override void InitWnd()
    {
        
        base.InitWnd();

        //获取宽度
        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;

        SetText(txtTips, "我是一个小小的Tips!!");
        SetText(txtPrg, "0%");

        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector3(-550f, 0, 0);
    }


    /// <summary>
    /// 进度条变化
    /// </summary>
    /// <param name="prg"></param>
    public void SetProgress(float prg)
    {
        SetText(txtPrg, (int)(prg * 100) + "%");
        imgFG.fillAmount = prg;

        float posX = prg * fgWidth - 570;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }
}