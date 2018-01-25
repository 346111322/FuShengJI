using System;
using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public class BankView : BaseView
{

    public static BankView instance;
    /// <summary>
    /// 确定键
    /// </summary>
    public GameObject mConfirmObj;
    /// <summary>
    /// 钱财数量
    /// </summary>
    public InputField mInputMoney;
    private int money = 0;
    /// <summary>
    /// 取钱
    /// 存钱
    /// </summary>
    public Toggle mSaveToggle;
    public Toggle mTakeToggle;
    /// <summary>
    /// 通知
    /// </summary>
    public Text mNoticeText;
    void Awake()
    {
        instance = this;
        ShoeBankView();
//        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void ShoeBankView()
    {
        //显示通知
        TextWrop.Instance.ShowContent(mNoticeText, Localization.Get("Bank_Notice"));
        //加载确定方法
        AddButtonEvent(mConfirmObj, OnClickConfirm);
    }

    /// <summary>
    /// 银行方法
    /// </summary>
    private void BankFuncion()
    {
        if(mSaveToggle.isOn)
        {
            //执行存款方法
            SaveMoney(mInputMoney.text);
        }
        else if(mTakeToggle.isOn)
        {
            //执行取款方法
            TakeMoney(mInputMoney.text);
        }
    }

    /// <summary>
    /// 取钱
    /// </summary>
    private void TakeMoney(string text)
    {
        if(int.TryParse(text, out money))
        {
            if(PlayerData.Instance.Deposit >= money)
            {
                PlayerData.Instance.AddCach = money;
                PlayerData.Instance.AddDeposit = -money;
            }
            else
            {
                TipProxy.ShowTip(Localization.Get("Money_NotEnough"));
            }
        }
    }
    /// <summary>
    /// 存钱
    /// </summary>
    private void SaveMoney(string text)
    {
        if (int.TryParse(text, out money))
        {
            if (PlayerData.Instance.Cash >= money)
            {
                PlayerData.Instance.AddCach = -money;
                PlayerData.Instance.AddDeposit = money;
            }
            else
            {
                TipProxy.ShowTip(Localization.Get("Money_NotEnough"));
            }
        }
    }

    private void OnClickConfirm(GameObject btn, object sender)
    {
        BankFuncion();
        UIManager.Instance.Hide(View.BankView);
        DetailDataView.instance.UpdatePlayData();
//        Hide();
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
