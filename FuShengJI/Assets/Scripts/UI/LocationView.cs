using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using BH.Core.Configs;
using UnityEngine;
using UnityEngine.UI;

public class LocationView : BaseView
{
    public GameObject mLocaleObj;
    public Transform mContent;
    private List<GameObject> mLocationObjList = new List<GameObject>();

    public static LocationView instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ShowLocaleView(GoodData.Instance.mLocationList);
    }

    /// <summary>
    /// 展示地点界面
    /// </summary>
    /// <param name="locationList">地点列表</param>
    public void ShowLocaleView(List<Tab_Location> locationList)
    {
        mLocationObjList.Clear();
        for (int i = 0; i < locationList.Count; i++)
        {
            if (i >= mLocationObjList.Count)
            {
                GameObject obj = GameObject.Instantiate(mLocaleObj);
                obj.SetActive(true);
                obj.transform.SetParent(mContent);
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                obj.GetComponentInChildren<Text>().text = locationList[i].Content;
                mLocationObjList.Add(obj);
                AddButtonEvent(obj, OnClickLocation, locationList[i].GoodNumMax);
            }
            else
            {
                mLocationObjList[i].GetComponentInChildren<Text>().text = locationList[i].Content;
            }
        }
        for (int i = locationList.Count; i < mLocationObjList.Count; i++)
        {
            mLocationObjList[i].SetActive(false);
        }
    }

    private void OnClickLocation(GameObject btn, object sender)
    {
        if (MainView.instacne != null)
        {
            if (TimeManager.Instance.IsOver())
            {
                var lastData = RankingModel.Instance.GetLast();
                int curScore = PlayerData.Instance.Money;
                //判断是否上榜
                //if上班 调用榜单
                if (lastData != null && RankingModel.Instance.mPlayerInfoList.Count > 20 && int.Parse(lastData.Money.ToString())> curScore )
                {
                    //显示结束界面
                    TipProxy.ShowTip("GameOver");
                }
                else 
                {
                    UIManager.Instance.ShowUIView(View.RankingTipView);
                    //显示界面
                }
                return;
            }
            //更新欠债
            PlayerData.Instance.Debt = (int)(PlayerData.Instance.Debt * 1.1f);
            //倒计时
            TimeManager.Instance.CountDown();
            //更新商品 事件
            MainView.instacne.UpdateMarket((int)sender, TimeManager.Instance.TimeCount == 0);
            UIManager.Instance.Hide(View.LocationSelectView);



        }
    }
}
