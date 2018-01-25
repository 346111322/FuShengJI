using System;
using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using BH.Core.Configs;
using UnityEngine;
using UnityEngine.UI;

public class PostofficeView : BaseView
{

    public static PostofficeView instance;
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
    /// <summary>
    /// 邮局事件集合
    /// </summary>
    Hashtable postofficeDir = new Hashtable();
    void Awake()
    {
        instance = this;
        postofficeDir = DataTable.Instance.GetPostoffice();
//        GetPostofficeEventList();
        //加载确定方法
        AddButtonEvent(mConfirmObj, OnClickConfirm);
        AddButtonEvent(mCancelObj, OnClickCancel);
    }

    /// <summary>
    /// 展示界面
    /// </summary>
    public void ShoePostofficeView()
    {
        GetPostofficeEventList();
        if (listPost == null || listPost.Count <= 0)
        {
            TipProxy.ShowTip(Localization.Get("出大事了"));
            return;
        }
        int idx = UnityEngine.Random.Range(0, listPost.Count);
        Tab_Postoffice tab = listPost[idx];
        //显示通知
        TextWrop.Instance.ShowContent(mNoticeText, String.Format(tab.Introduce,PlayerData.Instance.Debt));
        //处理按钮
        mConfirmObj.transform.Find("Text").GetComponent<Text>().text = PlayerData.Instance.Debt > 0 ? Localization.Get("Btn_Repay") : Localization.Get("Btn_Confirm");

    }

    List<Tab_Postoffice> listPost = new List<Tab_Postoffice>();
    /// <summary>
    /// 邮局方法 获取相关事件
    /// </summary>
    private void GetPostofficeEventList()
    {
        listPost.Clear();
        //判断玩家金钱 进行邮局的嘲讽
        foreach (var item in postofficeDir.Values)
        {
            Tab_Postoffice tab = item as Tab_Postoffice;
            if (tab == null)
                continue;
            //是否欠钱
            if (PlayerData.Instance.Debt > 0 && tab.IsDebt < 0)
            {
                //是否能换
                if (PlayerData.Instance.Money >= 0 && tab.MoneyMin > 0)
                    listPost.Add(tab);
                if (PlayerData.Instance.Money < 0 && tab.MoneyMin < 0)
                    listPost.Add(tab);
            }
            if (PlayerData.Instance.Debt <= 0 && tab.IsDebt > 0)
            {
                if (tab.MoneyMin < PlayerData.Instance.Money && tab.MoneyMax > PlayerData.Instance.Money)
                    listPost.Add(tab);
            }
        }
    }


    private void OnClickConfirm(GameObject btn, object sender)
    {
        if (PlayerData.Instance.Cash >= PlayerData.Instance.Debt)
        {
            PlayerData.Instance.Cash -= PlayerData.Instance.Debt;
            PlayerData.Instance.Debt = 0;
        }
        UIManager.Instance.Hide(View.PostofficeView);
        DetailDataView.instance.UpdatePlayData();
    }

    private void OnClickCancel(GameObject btn, object sender)
    {
        UIManager.Instance.Hide(View.PostofficeView);
        //        DetailDataView.instance.UpdatePlayData();
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowFunction(FunctionType type, int idx, Action<int> action = null)
    {
        ClearView();
        //根据人物属性进行判断 选择相应内容
        switch (type)
        {
            case FunctionType.Bank:

                break;
            case FunctionType.Hospital:
                break;
            case FunctionType.Internet:
                break;
            case FunctionType.Postoffice:
                break;
            case FunctionType.Rental:
                break;
        }
        if (action != null)
        {
            //            int num = int.Parse(inputFiled.text);
            //            action(num);
        }
    }

    private void ClearView()
    {
        //        inputFiled.text = null;
        //        noticeText.text = null;
        //        introduceText.text = null;

    }
}
