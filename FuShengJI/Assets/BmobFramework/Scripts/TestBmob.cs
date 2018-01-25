using System;
using System.Collections;
using System.Collections.Generic;
using cn.bmob.io;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TestBmob : MonoBehaviour
{
    public const string ApplicationId = "c0b7596adad84aa74504e21aa4a80040";

    public Button mInstertBtn;
    public Button mDeleteBtn;
    public Button mChangeBtn;
    public Button mCheckBtn;

    BmobTab_RankingList data = new BmobTab_RankingList();
    private List<BmobTab_RankingList> mPlayerInfoList = new List<BmobTab_RankingList>();

    void Start()
    {
        mInstertBtn.onClick.AddListener(delegate
        {
            data.Money = Random.Range(1000,2000);
            BmobManager.Instance.InsertData<BmobTab_RankingList>(data, BmobTab_RankingList.TABLENAME);
        });
        mDeleteBtn.onClick.AddListener(delegate
        {
            for (int i = 0; i < mPlayerInfoList.Count; i++)
            {
                BmobManager.Instance.deleteData(mPlayerInfoList[i].objectId, BmobTab_RankingList.TABLENAME);
            }
        });
        mChangeBtn.onClick.AddListener(delegate
        {
            BmobManager.Instance.updateData<BmobTab_RankingList>(data, "", BmobTab_RankingList.TABLENAME);
        });
        mCheckBtn.onClick.AddListener(delegate
        {
            BmobManager.Instance.StartCoroutine(FindByCondition());
        });
//        data.objectId = "0001";
//        data.playerName = "001Tab";
//        data.score = 1000;
    }


    /// <summary>
    /// 寻找数据根据条件
    /// </summary>
    private IEnumerator FindByCondition()
    {
        BmobQuery query = new BmobQuery();
        query.WhereGreaterThan("PlayerScore", 500);
        query.OrderByDescending("PlayerScore");
        bool isFindOver = false;
        StartCoroutine(BmobManager.Instance.ConditionFind<BmobTab_RankingList>(query, BmobTab_RankingList.TABLENAME,
        (List<BmobTab_RankingList> list, bool isOver) =>
        {
            mPlayerInfoList = list;
            isFindOver = isOver;
        }));


        while (!isFindOver)
        {
            Debug.Log("list is null");
            yield return 0;
        }
        if (isFindOver)
        {
            for (int i = 0; i < mPlayerInfoList.Count; i++)
            {
//                Debug.Log(mPlayerInfoList[i].objectId + "  " + mPlayerInfoList[i].id + "  " + mPlayerInfoList[i].playerName + "  " + mPlayerInfoList[i].score);
            }
        }
    }
}
