using System.Collections;
using System.Collections.Generic;
using BH.Core.Configs;
using UnityEngine;
 
public class GoodItemData
{
    public int goodId;
    public string goodName;
    public Sprite iconSprite;
    public int goodNum = 0;
    public int goodPrice = 0;
    public float eventEfftct = 1.0f;
    public bool isDepot = false;
    public Tab_Event mEvent;
    public Tab_GoodData goodData;

    public Sprite[] IconSprites;

    public GoodItemData(Tab_GoodData data)
    {
        if(data == null)
            return;
        this.goodId = data.ID;
        this.goodName = data.Name;
        this.goodData = data;
        this.iconSprite = GetSpriteByIcon(data.Icon);
    }
    private Sprite GetSpriteByIcon(string icon)
    {
//        Sprite[]  emojSprite= Resources.LoadAll<Sprite>("Sprite/GoodIcon");
        string path = "Sprite/GoodIcon/" + icon;
        Sprite sprite = Resources.Load<Sprite>(path);
        return sprite;
    }
}

public class GoodData
{
    private static GoodData instance;
    public static GoodData Instance
    {
        get
        {
            if (instance == null)
                instance = new GoodData();
            return instance;
        }
    }

    public Dictionary<int, GoodItemData> mAllGoodsDir = new Dictionary<int, GoodItemData>();
    public List<GoodItemData> mMarketGoods = new List<GoodItemData>();
    public List<Tab_Location> mLocationList = new List<Tab_Location>();

    public int mMaxGoodNum = 0;
    public int[] mGoodsList;

    /// <summary>
    /// 全部商品信息初始化
    /// </summary>
    public void GoodDataInfo()
    {
        mMaxGoodNum = DataTable.Instance.GetGoodData().Count;
        mGoodsList = new int[DataTable.Instance.GetGoodData().Count];
        int idx = 0;
        mAllGoodsDir.Clear();
        PlayerData.Instance.mDepotGoods.Clear();
        Hashtable dirData = DataTable.Instance.GetGoodData();
        //遍历方法二：遍历哈希表中的值
        foreach (Tab_GoodData item in dirData.Values)
        {
//            Debug.Log(item.Name);
            mAllGoodsDir.Add(item.ID, new GoodItemData(item));
            GoodItemData goodItem = new GoodItemData(item);
            goodItem.isDepot = true;
            PlayerData.Instance.mDepotGoods.Add(item.ID, goodItem);
            if (idx < mGoodsList.Length)
            {
                mGoodsList[idx] = item.ID;
                idx++;
            }
        }
    }

    /// <summary>
    /// 获取地点集合
    /// </summary>
    public void LocationDataInfo()
    {
        mLocationList.Clear();
        Hashtable dirData = DataTable.Instance.GetLocation();
        //遍历方法二：遍历哈希表中的值
        foreach (Tab_Location item in dirData.Values)
        {
            mLocationList.Add(item);
        }
    }
}
