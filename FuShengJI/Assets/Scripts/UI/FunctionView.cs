using System;
using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public enum FunctionType
{
    None,
    /// <summary>
    /// 银行
    /// </summary>
    Bank,
    /// <summary>
    /// 中介
    /// </summary>
    Rental,
    /// <summary>
    /// 医院
    /// </summary>
    Hospital,
    /// <summary>
    /// 邮局
    /// </summary>
    Postoffice,
    /// <summary>
    /// 网吧
    /// </summary>
    Internet,
}

public class FunctionView : BaseView
{
    public static FunctionView instance;

    public GameObject mConfirmObj;
    public InputField inputFiled;
    public Text noticeText;
    public Text introduceText;
    public FunctionType curFunctionType = FunctionType.None;
    void Awake()
    {
        instance = this;
        inputFiled.gameObject.SetActive(true);
        noticeText.gameObject.SetActive(true);
        introduceText.gameObject.SetActive(true); 
        AddButtonEvent(mConfirmObj,OnClickConfirm);
        gameObject.SetActive(false);
    }

    private void OnClickConfirm(GameObject btn, object sender)
    {

    }

    public void ShowFunction(FunctionType type, int idx,Action<int> action = null)
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
        if(action!= null)
        {
            int num = int.Parse(inputFiled.text);
            action(num);
        }
    }

    private void ClearView()
    {
        inputFiled.text = null;
        noticeText.text = null;
        introduceText.text = null;

    }
}
