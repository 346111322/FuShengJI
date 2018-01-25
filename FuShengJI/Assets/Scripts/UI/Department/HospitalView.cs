using System;
using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using BH.Core.Configs;
using UnityEngine;
using UnityEngine.UI;

public class HospitalView : BaseView
{

    public static HospitalView instance;
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
    Hashtable hospitalDir = new Hashtable();
    List<Tab_Hospial> listEvent = new List<Tab_Hospial>();
    void Awake()
    {
        instance = this;
        hospitalDir = DataTable.Instance.GetHospial();

        AddButtonEvent(mCancelObj, OnClickCancel);
    }

    /// <summary>
    /// 展示界面
    /// </summary>
    public void ShowHospitalView()
    {
        GetHospitalEventList();
        if (listEvent == null || listEvent.Count <= 0)
        {
            TipProxy.ShowTip(Localization.Get("UI_Function_Warn"));
            return;
        }
        int idx = UnityEngine.Random.Range(0, listEvent.Count);
        Tab_Hospial tab = listEvent[idx];
        //显示通知
        string msg = string.Format(tab.Introduce, tab.Cost);
        TextWrop.Instance.ShowContent(mNoticeText, msg);
        //加载确定方法
        AddButtonEvent(mConfirmObj, OnClickConfirm, tab);
    }

    /// <summary>
    /// 邮局方法 获取相关事件
    /// </summary>
    private void GetHospitalEventList()
    {
        listEvent.Clear();
        //判断玩家健康情况
        foreach (var item in hospitalDir.Values)
        {
            Tab_Hospial tab = item as Tab_Hospial;
            if (tab == null)
                continue;
            //判断玩家健康值在哪个区间
            if (PlayerData.Instance.Health <= tab.HealtheMax && PlayerData.Instance.Health > tab.HealtheMin)
            {
                listEvent.Add(tab);
            }
        }
    }


    private void OnClickConfirm(GameObject btn, object sender)
    {
        Tab_Hospial tab = sender as Tab_Hospial;
        if (tab == null)
            return;
        if (PlayerData.Instance.Cash >= tab.Cost)
        {
            PlayerData.Instance.Cash -= tab.Cost;
            PlayerData.Instance.Health = 100;
        }
        else
        {
            TipProxy.ShowTip(Localization.Get("UI_Hospital_Warn"));
        }
        UIManager.Instance.Hide(View.HospitalView);
        DetailDataView.instance.UpdatePlayData();
    }

    private void OnClickCancel(GameObject btn, object sender)
    {
        UIManager.Instance.Hide(View.HospitalView);
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
