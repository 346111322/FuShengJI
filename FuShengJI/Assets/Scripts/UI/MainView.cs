using System.Collections.Generic;
using System.Linq;
using BH.Core.Base;
using UnityEngine;
/// <summary>
/// 主界面
/// </summary>
public class MainView : BaseView
{
    /// <summary>
    /// 商品父物体PS：用于显示商品列表格
    /// </summary>
    public Transform parent;
    /// <summary>
    /// 商品列表格
    /// </summary>
    public GameObject goodItem;
    /// <summary>
    /// 商品列表集合
    /// </summary>
    public List<GameObject> goodObjList = new List<GameObject>();
    /// <summary>
    /// 商店按钮
    /// </summary>
    public GameObject mMarketBtn;
    /// <summary>
    /// 仓库按钮
    /// </summary>
    public GameObject mDepotBtn;
    /// <summary>
    /// 交通站
    /// </summary>
    public GameObject mTrafficBtn;

    public static MainView instacne;

    void Awake()
    {
        instacne = this;
    }
    void Start()
    {
        AddButtonEvent(mTrafficBtn, OnClickTraffic);
        AddButtonEvent(mMarketBtn, OnClickMarket);
        AddButtonEvent(mDepotBtn, OnClickDepot);
        //更新商品
        UpdateMarket(-1);
    }

    private void OnClickTraffic(GameObject btn, object sender)
    {
        UIManager.Instance.ShowUIView(View.LocationSelectView);
    }
    /// <summary>
    /// 点击商店
    /// </summary>
    private void OnClickMarket(GameObject btn, object sender)
    {
        List<GoodItemData> marketGoods = GoodData.Instance.mMarketGoods;
        CreatGoodItem(marketGoods);
    }
    /// <summary>
    /// 点击仓库
    /// </summary>
    private void OnClickDepot(GameObject btn, object sender)
    {
        List<GoodItemData> goodList = PlayerData.Instance.mDepotGoods.Values.ToList();
        CreatGoodItem(goodList, true);
    }

    /// <summary>
    /// 更新商品
    /// </summary>
    public void UpdateMarket(int maxGoods, bool isAll = false)
    {
        if (TimeManager.Instance.TimeCount < 0)
        {
            Debug.Log("结束游戏了");
            return;
        }
        GoodModel.Instance.GetMarketGoods(maxGoods,isAll);
        List<GoodItemData> marketGoods = GoodData.Instance.mMarketGoods;
        CreatGoodItem(marketGoods);
        //清理事件数据
        Adventure.Instance.advAndEveList.Clear();
        //获取商品事件
        Adventure.Instance.GetEventList(marketGoods);
        //触发奇遇事件
        Adventure.Instance.OnTriggerAdventure();

        UIManager.Instance.ShowUIView(View.EventView);

        EventListView.instance.ShowEvent(Adventure.Instance.advAndEveList);

        //更新数据   
        DetailDataView.instance.UpdatePlayData();

    }

    /// <summary>
    /// 显示商品列表
    /// </summary>
    /// <param name="goodList"></param>
    public void CreatGoodItem(List<GoodItemData> goodList, bool isDepot = false)
    {
        for (int i = 0; i < goodObjList.Count; i++)
        {
            goodObjList[i].SetActive(false);
        }
        if (isDepot)
        {
            goodList = goodList.FindAll((a) => a.goodNum > 0);
        }
        for (int i = 0; i < goodList.Count; i++)
        {
            if (i < goodObjList.Count)
            {
                if (isDepot)
                    goodObjList[i].GetComponent<GoodItem>().ShowGoodByDepot(goodList[i]);
                else
                    goodObjList[i].GetComponent<GoodItem>().ShowGoodByMarket(goodList[i]);
            }
            else
            {
                GameObject obj = GameObject.Instantiate(goodItem);
                obj.transform.SetParent(parent);
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                if (isDepot)
                    obj.GetComponent<GoodItem>().ShowGoodByDepot(goodList[i]);
                else
                    obj.GetComponent<GoodItem>().ShowGoodByMarket(goodList[i]);
                goodObjList.Add(obj);
            }
            goodObjList[i].SetActive(true);
        }
        parent.localPosition = Vector3.zero;
    }


}
