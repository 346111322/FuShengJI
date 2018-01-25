using System;
using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using cn.bmob.io;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 这个类是用来进行排列的
/// 1.通过Bmob获得表格数据，
/// 2.将其进行排列
/// 3.删除某条信息
/// 4.增加某条信息
/// 5.组装消息
/// </summary>
public class RankingListView : BaseView
{
    public static RankingListView instance;

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public GameObject m_BtnCloseObj;

    void Awake()
    {
        instance = this;
        AddButtonEvent(m_BtnCloseObj, OnClickClose);
    }

    private void OnClickClose(GameObject btn, object sender)
    {
        UIManager.Instance.Hide(View.RankingListView);
    }
    //    void Start()
    //    {
    //        ShowRankingView();
    //    }

    /// <summary>
    /// 显示排行榜
    /// </summary>
    public void ShowRankingView()
    {
        RankingModel.Instance.GetDataList("Money", UpdateView);
    }


    public Transform parentTra;
    public GameObject rankingListItem;
    public List<GameObject> rankingList = new List<GameObject>();
    /// <summary>
    /// 更新界面显示
    /// </summary>
    public void UpdateView()
    {
        Debug.Log("更新界面显示");
        string msg = "";
        RankingModel.Instance.mPlayerInfoList.Sort(ScoreCompare);
        List<BmobTab_RankingList> list = RankingModel.Instance.mPlayerInfoList;
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            if (i < rankingList.Count)
            {
                rankingList[i].SetActive(true);
                msg = string.Format(Localization.Get("RankingListMsg"), list[i].PlayerName, list[i].Health, list[i].Money);
                TextWrop.Instance.ShowContent(rankingList[i].GetComponentInChildren<Text>(), msg, false, false);
                //                rankingList[i].GetComponentInChildren<Text>().text = msg;
            }
            else
            {
                GameObject obj = Instantiate(rankingListItem);
                obj.SetActive(true);
                obj.transform.SetParent(parentTra);
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;
                rankingList.Add(obj);
                msg = string.Format(Localization.Get("RankingListMsg"), list[i].PlayerName, list[i].Health, list[i].Money);
                TextWrop.Instance.ShowContent(obj.GetComponentInChildren<Text>(), msg, false, false);
//                obj.GetComponentInChildren<Text>().text = msg;
            }
        }
        for (int i = list.Count; i < rankingList.Count; i++)
        {
            rankingList[i].SetActive(false);
        }
    }

    private int ScoreCompare(BmobTab_RankingList x, BmobTab_RankingList y)
    {
        int xMoney = int.Parse(x.Money.ToString());
        int yMoney = int.Parse(y.Money.ToString());
        if (xMoney > yMoney)
            return -1;
        return 1;
    }

    //"PlayerScore"
}
