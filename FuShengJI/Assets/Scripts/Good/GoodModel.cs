using BH.Core.Configs;
using UnityEngine;

public class GoodModel : BaseModel
{
    private static GoodModel instance;
    public static GoodModel Instance
    {
        get
        {
            if (instance == null)
                instance = new GoodModel();
            return instance;
        }
    }
    /// <summary>
    /// 事件发生概率
    /// </summary>
    private int eventRatio = 20;


    /// <summary>
    /// 获取市场商品信息
    /// </summary>
    public void GetMarketGoods(int maxGoods ,bool isAll = false)
    {
        ///获取商品id列表
        /// 随机id数量
        /// 随机得到id列表
        /// 根据列表得到商品信息列表
        int[] goodsArr = new int[GoodData.Instance.mGoodsList.Length];
        int goodNub = goodsArr.Length / 2;
        GoodData.Instance.mGoodsList.CopyTo(goodsArr, 0);
        if (!isAll)
        {
            int sum = Random.Range(goodNub, goodsArr.Length);
            sum = Mathf.Max(goodNub, maxGoods);
            int[] marGoodsArr = new int[sum];
            for (int i = 0; i < sum; i++)
            {
                int a = Random.Range(0, goodsArr.Length - i);
                marGoodsArr[i] = goodsArr[a];
                goodsArr[a] = goodsArr[goodsArr.Length - i - 1];
            }
            GetMarketGoods(marGoodsArr);
        }
        else
        {
            GetMarketGoods(goodsArr);
        }
    }

    /// <summary>
    /// 根据商品集合id获取市场商品集合
    /// </summary>
    /// <param name="goodsArr">商品集合id</param>
    public void GetMarketGoods(int[] goodsArr)
    {
        GoodData.Instance.mMarketGoods.Clear();
        for (int i = 0; i < goodsArr.Length; i++)
        {
            GoodData.Instance.mMarketGoods.Add(GoodData.Instance.mAllGoodsDir[goodsArr[i]]);
        }
        for (int i = 0; i < GoodData.Instance.mMarketGoods.Count; i++)
        {
            GoodData.Instance.mMarketGoods[i] = GetRandomGoodData(GoodData.Instance.mMarketGoods[i]);
        }
        GoodData.Instance.mMarketGoods.Sort((a, b) =>
        {
            return a.goodPrice < b.goodPrice ? -1 : 1;
        });
    }

    /// <summary>
    /// 获得随机商品信息
    /// </summary>
    /// <param name="item">商品</param>
    /// <returns>随机后的商品</returns>
    public GoodItemData GetRandomGoodData(GoodItemData item)
    {
        int randomNum = Random.Range(1, 101);
        item.mEvent = null;
        item.eventEfftct = 1.0f;

        int mEventDropNum = 0;
        int mEventRiseNum = 0;

        for (int i = 0; i < item.goodData.GetEventDropIdCount(); i++)
        {
            if(item.goodData.GetEventDropIdbyIndex(i) > 0)
            {
                mEventDropNum++;
            }
        }
        for (int i = 0; i < item.goodData.GetEventRiseIdCount(); i++)
        {
            if (item.goodData.GetEventRiseIdbyIndex(i) > 0)
            {
                mEventRiseNum++;
            }
        }
        Tab_Event tabEve = null;
        if (randomNum <= item.goodData.EventRatioDrop && mEventDropNum > 0)
        {
            tabEve = DataTable.Instance.GetEventById(item.goodData.GetEventDropIdbyIndex(Random.Range(0, mEventDropNum)));
            if(tabEve != null)
            {
                item.eventEfftct = (float)tabEve.EventEffect / 10.0f;
                item.mEvent = tabEve;
            } 
        }
        if (randomNum > (100 - item.goodData.EventRatioRise) && mEventRiseNum > 0)
        {
            tabEve = DataTable.Instance.GetEventById(item.goodData.GetEventRiseIdbyIndex(Random.Range(0, mEventRiseNum)));
            if (tabEve != null)
            {
                item.eventEfftct = (float)tabEve.EventEffect / 10.0f;
                item.mEvent = tabEve;
            }
        }
        //随机一下 获得事件值
        item.goodPrice = (int)(Random.Range(item.goodData.PriceMin, item.goodData.PriceMax) * item.eventEfftct);
//        Debug.Log("商品名称： "+ item.goodName + " 最大值："+ item.goodData.PriceMax + " 最小值："+ item.goodData.PriceMin +  " 当前值："+ item.goodPrice);
        item.isDepot = false;
        return item;
    }


    public override void Clear()
    {
        base.Clear();
    }
}
