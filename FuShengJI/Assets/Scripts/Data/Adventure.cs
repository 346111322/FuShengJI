using System.Collections.Generic;
using BH.Core.Configs;
using UnityEngine;

public class Adventure
{
    private static Adventure instance;
    public static Adventure Instance
    {
        get
        {
            if (instance == null)
                instance = new Adventure();
            return instance;
        }
    }
    public List<string> advAndEveList = new List<string>();
    public List<Tab_Adventure> adventureList = new List<Tab_Adventure>();
    public Adventure()
    {
        adventureList.Clear();
        foreach (var item in DataTable.Instance.GetAdventure().Values)
        {
            adventureList.Add((Tab_Adventure)item);
        }
    }

    /// <summary>
    /// 获取商品事件列表
    /// </summary>
    public void GetEventList(List<GoodItemData> marketGoods)
    {
        //开始事件队列显示
        for (int i = 0; i < marketGoods.Count; i++)
        {
            if (marketGoods[i].mEvent != null)
            {
                Adventure.Instance.advAndEveList.Add(marketGoods[i].mEvent.Content);
            }
        }
    }


    /// <summary>
    /// 触发奇遇
    /// </summary>
    public void OnTriggerAdventure()
    {
        //循环判断
        for (int i = 0; i < adventureList.Count; i++)
        {
            // 根据奇遇进行检查
            Tab_Adventure adventure = adventureList[i];
            if (IsAdventurePass(adventure))
            {
                // 假如合格 进入随机阶段
                if (Random.Range(0, 100) < adventure.Ratio)
                {
                    // 假如ok 属性添加 事件通知开启
                    PlayerData.Instance.AddCach = adventure.Cash;
                    PlayerData.Instance.AddDeposit = adventure.Deposit;
                    PlayerData.Instance.AddDebt = adventure.Debt;
                    PlayerData.Instance.AddHealth = adventure.Health;
                    PlayerData.Instance.AddRepute = adventure.Repute;
                    Adventure.Instance.advAndEveList.Add(adventure.Content);
                    if(adventure.GoodId > 0)
                    {
                        PlayerData.Instance.BuyGoods(new GoodItemData(DataTable.Instance.GetGoodDataById(adventure.GoodId)), adventure.GoodNum);
                    }
                }
            }
            // 假如不合格 跳过
        }

       
    }
    /// <summary>
    /// 判断奇遇是否满足条件
    /// </summary>
    /// <param name="adventure">奇遇事件</param>
    public bool IsAdventurePass(Tab_Adventure adventure)
    {
        if (adventure == null)
            return false;
        if (adventure.Condition < 0)
            return true;
        PlayerPropeType type = (PlayerPropeType)(adventure.Condition / 1000);
        int value = (adventure.Condition + 1000) % 1000;
        switch (type)
        {
            case PlayerPropeType.Cash:
                switch (value)
                {
                    case 1:
                        return PlayerData.Instance.Cash < adventure.Cash;
                        break;
                    case 2:
                        return PlayerData.Instance.Cash > adventure.Cash;
                        break;
                    case 3:
                        return PlayerData.Instance.Cash == adventure.Cash;
                        break;
                }
                break;
            case PlayerPropeType.Deposit:
                switch (value)
                {
                    case 1:
                        return PlayerData.Instance.Deposit < adventure.Deposit;
                        break;
                    case 2:
                        return PlayerData.Instance.Deposit > adventure.Deposit;
                        break;
                    case 3:
                        return PlayerData.Instance.Deposit == adventure.Deposit;
                        break;
                }
                break;
            case PlayerPropeType.Debt:
                switch (value)
                {
                    case 1:
                        return PlayerData.Instance.Debt < adventure.Debt;
                        break;
                    case 2:
                        return PlayerData.Instance.Debt > adventure.Debt;
                        break;
                    case 3:
                        return PlayerData.Instance.Debt == adventure.Debt;
                        break;
                }
                break;
            case PlayerPropeType.Health:
                switch (value)
                {
                    case 1:
                        return PlayerData.Instance.Health < adventure.Health;
                        break;
                    case 2:
                        return PlayerData.Instance.Health > adventure.Health;
                        break;
                    case 3:
                        return PlayerData.Instance.Health == adventure.Health;
                        break;
                }
                break;
            case PlayerPropeType.Repute:
                switch (value)
                {
                    case 1:
                        return PlayerData.Instance.Repute < adventure.Repute;
                        break;
                    case 2:
                        return PlayerData.Instance.Repute > adventure.Repute;
                        break;
                    case 3:
                        return PlayerData.Instance.Repute == adventure.Repute;
                        break;
                }
                break;
            case PlayerPropeType.LeftRoom:
                switch (value)
                {
                    case 1:
                        return PlayerData.Instance.LeftRoom < adventure.GoodNum;
                        break;
                    case 2:
                        return PlayerData.Instance.LeftRoom > adventure.GoodNum;
                        break;
                    case 3:
                        return PlayerData.Instance.LeftRoom == adventure.GoodNum;
                        break;
                }
                break;
            default:
                return false;
        }
        return false;
    }
}
