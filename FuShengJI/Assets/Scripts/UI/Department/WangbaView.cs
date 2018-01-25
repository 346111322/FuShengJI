using System;
using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using BH.Core.Configs;
using UnityEngine;
using UnityEngine.UI;

public class WangbaView : BaseView
{

    public static WangbaView instance;
    /// <summary>
    /// 确定键
    /// </summary>
    public GameObject mConfirmObj;
    /// <summary>
    /// 取消键
    /// </summary>
    public GameObject mCancelObj;
    /// <summary>
    /// 通知
    /// </summary>
    public Text mNoticeText;
    void Awake()
    {
        instance = this;
//        GetPostofficeEventList();
        //加载确定方法
        AddButtonEvent(mConfirmObj, OnClickConfirm);
        AddButtonEvent(mCancelObj, OnClickCancel);
    }

    /// <summary>
    /// 展示界面
    /// </summary>
    public void ShoeWangbaView()
    {

    }

    private void OnClickConfirm(GameObject btn, object sender)
    {
        UIManager.Instance.Hide(View.WangbaView);
        DetailDataView.instance.UpdatePlayData();
    }

    private void OnClickCancel(GameObject btn, object sender)
    {
        UIManager.Instance.Hide(View.WangbaView);
    }

    private void ClearView()
    {
        //        inputFiled.text = null;
        //        noticeText.text = null;
        //        introduceText.text = null;

    }
}
