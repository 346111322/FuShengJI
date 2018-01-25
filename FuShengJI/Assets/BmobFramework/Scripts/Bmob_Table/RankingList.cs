using UnityEngine;
using System.Collections;
using cn.bmob.io;
//*******************
//2018年01月22日 15:19:18 Monday//
//*******************
public class BmobTab_RankingList : BmobTable
{
    /// <summary>
    /// Bmob服务器端我们定义的表名
    /// </summary>
    public const string TABLENAME = "RankingList";

    /// <summary>
    /// 名誉
    /// </summary>
    public BmobInt Fame { get; set; }
    /// <summary>
    /// 健康值
    /// </summary>
    public BmobInt Health { get; set; }
    /// <summary>
    /// 留言
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 金钱
    /// </summary>
    public BmobInt Money { get; set; }
    /// <summary>
    /// 玩家名
    /// /// </summary>
    public string PlayerName { get; set; }

    public BmobTab_RankingList()
    {
        
    }
    public BmobTab_RankingList(string playerName,int fame,int health,int money, string message)
    {
        this.PlayerName = playerName;
        this.Fame = fame;
        this.Health = health;
        this.Money = money;
        this.message = message;
    }

    /// <summary>
    /// 成员函数
    /// </summary>
    /// <param name="input"></param>
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        //读取相应的字段
        this.Fame = input.getInt("Fame");
        this.Health = input.getInt("Health");
        this.message = input.getString("message");
        this.Money = input.getInt("Money");
        this.PlayerName = input.getString("PlayerName");

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        //写入到数据库对应的字段
        output.Put("Fame", this.Fame);
        output.Put("Health", this.Health);
        output.Put("message", this.message);
        output.Put("Money", this.Money);
        output.Put("PlayerName", this.PlayerName);
    }
}
