using System;
using System.Collections;
using System.Collections.Generic;
using cn.bmob.io;
using UnityEngine;

public class RankingModel : BaseModel
{
    private static RankingModel instance;
    public static RankingModel Instance
    {
        get
        {
            if (instance == null)
                instance = new RankingModel();
            return instance;
        }
    }
    /// <summary>
    /// AppID
    /// </summary>
    public const string ApplicationId = "c0b7596adad84aa74504e21aa4a80040";

    /// <summary>
    /// 表格数据集合
    /// </summary>
    public List<BmobTab_RankingList> mPlayerInfoList = new List<BmobTab_RankingList>();

    /// <summary>
    /// 获取表格数据"PlayerScore"
    /// </summary>
    public void GetDataList(string score, Action action = null)
    {
        GameStart.instance.StartCoroutine(FindByCondition(score, action));
    }
    /// <summary>
    /// 寻找数据根据条件
    /// </summary>
    private IEnumerator FindByCondition(string score, Action action = null)
    {
        BmobQuery query = new BmobQuery();
        //设置最多返回20条记录
        query.Limit(20);
//        //条件 大于500的
//        query.WhereGreaterThan(score, 500);
        //降序排列
        query.OrderByDescending(score);

        bool isFindOver = false;
        GameStart.instance.StartCoroutine(BmobManager.Instance.ConditionFind<BmobTab_RankingList>(query, BmobTab_RankingList.TABLENAME,
            (List<BmobTab_RankingList> list, bool isOver) =>
            {
                mPlayerInfoList = list;
                isFindOver = true;
                Debug.Log("找到了数据");

            }));
        while (!isFindOver)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (action != null)
        {
            action();
        }
        if (mPlayerInfoList == null)
        {
            mPlayerInfoList = new List<BmobTab_RankingList>();
        }
    }

    /// <summary>
    /// 更新排行榜
    /// </summary>
    /// <param name="data"></param>
    public void UpdateRankingList(BmobTab_RankingList data)
    {
        if (data == null || mPlayerInfoList == null)
            return;
        BmobTab_RankingList ss = mPlayerInfoList.Find(a => a.objectId == data.objectId);
        if (ss != null)
        {
            //更新
            BmobManager.Instance.updateData<BmobTab_RankingList>(data, ss.objectId, BmobTab_RankingList.TABLENAME);
            ss = data;
        }
        else
        {
            BmobManager.Instance.InsertData<BmobTab_RankingList>(data, BmobTab_RankingList.TABLENAME);
            mPlayerInfoList.Add(data);
        }
        //判断是否多了
        if (RankingModel.Instance.mPlayerInfoList.Count > Setting.RankingListMax)
        //删除最后一个
        {
            int minScore = int.Parse(mPlayerInfoList[mPlayerInfoList.Count - 1].Money.ToString());
            int curScore = int.Parse(data.Money.ToString());
            if (minScore < curScore)
            {
                BmobTab_RankingList dd = mPlayerInfoList[mPlayerInfoList.Count - 1];
                BmobManager.Instance.deleteData(dd.objectId, BmobTab_RankingList.TABLENAME);
                mPlayerInfoList.Remove(mPlayerInfoList.Find(a => a.Money == dd.Money));
            }
        }
        ///更新界面显示
        RankingListView.instance.UpdateView();
    }


    /// <summary>
    /// 获取末尾值
    /// </summary>
    /// <returns></returns>
    public BmobTab_RankingList GetLast()
    {
        if (mPlayerInfoList != null && mPlayerInfoList.Count > 0)
        {
            return mPlayerInfoList[mPlayerInfoList.Count - 1];
        }
        return null;
    }
}
