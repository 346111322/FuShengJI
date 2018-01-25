using UnityEngine;
using System.Collections;
using cn.bmob.io;
//*******************
//2017年12月27日 12:22:40 Wednesday//
//*******************
public class PlayerInformation : BmobTable
{
    /// <summary>
    /// Bmob服务器端我们定义的表名
    /// </summary>
    public const string TABLENAME = "PlayerInformation";


    public string objectId { get; set; }
    public string playerName { get; set; }
    public BmobInt score { get; set; }
    public BmobInt id { get; set; }

    /// <summary>
    /// 成员函数
    /// </summary>
    /// <param name="input"></param>
    public override void readFields(BmobInput input)
    {
        base.readFields(input);
        //读取相应的字段
        this.objectId = input.getString("objectId");
        this.playerName = input.getString("PlayerName");
        this.score = input.getInt("PlayerScore");
        this.id = input.getInt("Id");

    }
    public override void write(BmobOutput output, bool all)
    {
        base.write(output, all);
        //写入到数据库对应的字段
//        output.Put("objectId", this.objectId);
        output.Put("PlayerName", this.playerName);
        output.Put("PlayerScore", this.score);
//        output.Put("Id", this.id);
    }
}
