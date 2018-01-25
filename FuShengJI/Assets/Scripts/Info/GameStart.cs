using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 初始化人物信息
/// 初始化设置
/// 初始化商品
/// 初始化情节
/// 初始化时间
/// </summary>
public class GameStart : MonoBehaviour
{
    public static GameStart instance;

    void Awake()
    {
        instance = this;

        //        return;
        //        StartActionDelay(delegate
        //        {
        GoodData.Instance.GoodDataInfo();
        PlayerData.Instance.PlayerDataInfo();
        GoodData.Instance.LocationDataInfo();
        TimeManager.Instance.TimeInfo();
        //获取排行榜信息
        RankingModel.Instance.GetDataList("PlayerScore");
        //        },2);

        //全为竖屏
        //        Screen.orientation = ScreenOrientation.Portrait;
    }


    void Start()
    {
        if (DetailDataView.instance)
        {
            DetailDataView.instance.UpdatePlayData();
        }
    }

    public void StartActionDelay(Action action, float delayTime)
    {
        StartCoroutine(DelayAction(action, delayTime));
    }

    private IEnumerator DelayAction(Action action, float delayTime = 0)
    {
        yield return new WaitForSeconds(delayTime);
        if (action != null)
            action();
    }


}
