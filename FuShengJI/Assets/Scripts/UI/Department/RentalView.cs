using System;
using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using BH.Core.Configs;
using UnityEngine;
using UnityEngine.UI;

public class RentalView : BaseView
{

    public static RentalView instance;
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
    /// 事件集合
    /// </summary>
    Hashtable hospitalDir = new Hashtable();
    List<Tab_Rental> listEvent = new List<Tab_Rental>();
    void Awake()
    {
        instance = this;
        hospitalDir = DataTable.Instance.GetHospial();

        AddButtonEvent(mCancelObj, OnClickCancel);
    }

    /// <summary>
    /// 展示界面
    /// </summary>
    public void ShowRentalView()
    {
        GetHospitalEventList();
        if (listEvent == null || listEvent.Count <= 0)
        {
            TipProxy.ShowTip(Localization.Get("UI_Function_Warn"));
            return;
        }
        int idx = UnityEngine.Random.Range(0, listEvent.Count);
        Tab_Rental tab = listEvent[idx];
        int price = Mathf.Max(tab.PriceMin, PlayerData.Instance.Cash/2);
        string msg = string.Format(tab.Introduce, PlayerData.Instance.RoomMax, price, tab.Size);
        //显示通知
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
        for (int i = 0; i < hospitalDir.Count; i++)
        {
            Tab_Rental tab = DataTable.Instance.GetRentalById(5001+i) as Tab_Rental;
            if (tab == null)
                continue;
            if (PlayerData.Instance.RoomMax < tab.Size)
            {
                listEvent.Add(tab);
                break;
            }
        }
    }


    private void OnClickConfirm(GameObject btn, object sender)
    {
        Tab_Rental tab = sender as Tab_Rental;
        if (tab == null)
            return;
        if (PlayerData.Instance.Cash >= tab.PriceMin)
        {
            PlayerData.Instance.Cash -= Mathf.Max(tab.PriceMin, PlayerData.Instance.Cash/2);
            PlayerData.Instance.RoomMax = tab.Size;
        }
        else
        {
            TipProxy.ShowTip(Localization.Get("UI_Hospital_Warn"));
        }
        UIManager.Instance.Hide(View.RentalView);
        DetailDataView.instance.UpdatePlayData();
    }

    private void OnClickCancel(GameObject btn, object sender)
    {
        UIManager.Instance.Hide(View.RentalView);
        //        DetailDataView.instance.UpdatePlayData();
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }


    private void ClearView()
    {
        //        inputFiled.text = null;
        //        noticeText.text = null;
        //        introduceText.text = null;

    }
}
