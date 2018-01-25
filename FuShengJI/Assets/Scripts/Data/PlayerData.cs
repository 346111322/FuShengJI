using System.Collections.Generic;
using System.Linq;
using BH.Core.Configs;
using UnityEngine;

/// <summary>
/// 人物属性类型
/// </summary>
public enum PlayerPropeType
{
    None = -1,
    /// <summary>
    /// 现金
    /// </summary>
    Cash = 1,
    /// <summary>
    /// 存款
    /// </summary>
    Deposit = 2,
    /// <summary>
    /// 债务
    /// </summary>
    Debt = 3,
    /// <summary>
    /// 健康
    /// </summary>
    Health = 4,
    /// <summary>
    /// 名声
    /// </summary>
    Repute = 5,
    /// <summary>
    /// 剩余空间
    /// </summary>
    LeftRoom = 6,
}
public class PlayerData
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string mName;
    /// <summary>
    /// 头像
    /// </summary>
    public string Icon;
    /// <summary>
    /// 现金
    /// </summary>
    public int Cash;
    /// </summary>
    public int AddCach { set { Cash =  Mathf.Max(Cash + value,0); } }
    /// <summary>
    /// 存款
    /// </summary>
    public int Deposit;
    public int AddDeposit { set { Deposit = Mathf.Max(Deposit + value, 0); } }
    /// <summary>
    /// 欠债
    /// </summary>
    public int Debt;
    public int AddDebt { set { Debt = Mathf.Max(Debt + value, 0); } }
    /// <summary>
    /// 健康
    /// </summary>
    public int Health;

    public int Money { get { return (Cash + Deposit - Debt); } }

    public int AddHealth
    {
        set
        {
            Health = Mathf.Clamp(Health + value, 1, 100);
        }
    }

    /// <summary>
    /// 名声
    /// </summary>
    public int Repute;
    public int AddRepute { set { Repute = Mathf.Max(Repute + value, 0); } }
    /// <summary>
    /// 剩余空间
    /// </summary>
    public int LeftRoom { get { return (RoomMax - GoodRoom); } }
    public int GoodRoom;
    /// <summary>
    /// 最大空间
    /// </summary>
    public int RoomMax;


    private static PlayerData instance;
    public static PlayerData Instance
    {
        get { if (instance == null) instance = new PlayerData(); return instance; }
    }
    /// <summary>
    /// 仓库存储的商品
    /// </summary>
    public Dictionary<int, GoodItemData> mDepotGoods = new Dictionary<int, GoodItemData>();

    /// <summary>
    /// 初始化玩家信息
    /// </summary>
    public void PlayerDataInfo()
    {
        Tab_PlayerData palyerData = DataTable.Instance.GetPlayerDataById(1);
        this.mName = palyerData.Name;
        this.Icon = palyerData.Icon;
        this.Cash = palyerData.Cash;
        this.Deposit = palyerData.Deposit;
        this.Debt = palyerData.Debt;
        this.Health = palyerData.Health;
        this.Repute = palyerData.Repute;
        this.GoodRoom = 0;
        this.RoomMax = Setting.InfoRoom;
    }
    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="goodData">商品</param>
    public void BuyGoods(GoodItemData goodData, int buyNum)
    {
        if (goodData == null)
            return;
        //判断商品是否超出承受范围
        if (buyNum > PlayerData.Instance.LeftRoom)
        {
            TipProxy.ShowTip("GoodNumOver");
            return;
        }
        GoodItemData depotGood = mDepotGoods[goodData.goodId];
        int allNum = (depotGood.goodNum + buyNum);
        depotGood.goodPrice = allNum > 0 ? (depotGood.goodPrice * depotGood.goodNum + goodData.goodPrice * buyNum) / allNum : 0;
        depotGood.goodNum = depotGood.goodNum + buyNum;
        mDepotGoods[goodData.goodId] = depotGood;
        this.Cash -= goodData.goodPrice * buyNum;
        this.GoodRoom += buyNum;

    }
    /// <summary>
    /// 出售商品
    /// </summary>
    /// <param name="goodData">商品</param>
    public void SellGoods(GoodItemData goodData, int sellNum)
    {
        GoodItemData depotGood = mDepotGoods[goodData.goodId];
        GoodItemData marketGood = GoodData.Instance.mMarketGoods.Find(a => a.goodId == goodData.goodId);
        sellNum = sellNum < depotGood.goodNum ? sellNum : depotGood.goodNum;
        depotGood.goodNum = depotGood.goodNum - sellNum;
        mDepotGoods[goodData.goodId] = depotGood;
        //更新仓库显示
        MainView.instacne.CreatGoodItem(mDepotGoods.Values.ToList(), true);
        this.Cash += marketGood.goodPrice * sellNum;
        this.GoodRoom -= sellNum;
    }
}
