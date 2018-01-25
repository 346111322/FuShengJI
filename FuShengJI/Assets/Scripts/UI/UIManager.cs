using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using UnityEngine;

public enum View
{
    /// <summary>
    /// 开始介绍界面
    /// </summary>
    InitialView,
    /// <summary>
    /// 主界面
    /// </summary>
    MainView,
    /// <summary>
    /// 商品界面
    /// </summary>
    GoodView,
    /// <summary>
    /// 事件显示界面
    /// </summary>
    EventView,
    /// <summary>
    /// 奇遇显示界面
    /// </summary>
    FunctionView,
    /// <summary>
    /// 地点选择
    /// </summary>
    LocationSelectView,
    /// <summary>
    /// 提示
    /// </summary>
    TipView,
    /// <summary>
    /// 银行
    /// </summary>
    BankView,
    /// <summary>
    /// 邮局
    /// </summary>
    PostofficeView,
    /// <summary>
    /// 医院
    /// </summary>
    HospitalView,
    /// <summary>
    /// 网 吧
    /// </summary>
    WangbaView,
    /// <summary>
    /// 中介
    /// </summary>
    RentalView,
    /// <summary>
    /// 排行榜
    /// </summary>
    RankingListView,
    /// <summary>
    /// 排行榜提示框
    /// </summary>
    RankingTipView
}

public class UIManager : BaseView
{
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public GameObject ShowUIView(View view)
    {
        string path = view.ToString();
        var obj = transform.Find(path);
        if (obj != null)
        {
            obj.gameObject.SetActive(true);
        }
        else
        {
            path = "UI/" + view.ToString();
            GameObject a = Resources.Load(path) as GameObject;
            if (a == null)
            {
                return null;
            }
            GameObject go = GameObject.Instantiate(a);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.name = view.ToString();
            obj = go.transform;
        }
        return obj.gameObject;
    }


    public void Hide(View view)
    {
        string path = view.ToString();
        var obj = transform.Find(path);
        if (obj != null)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
