using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : BaseModel
{
    private static TimeManager instance;
    public static TimeManager Instance
    {
        get
        {
            if(instance == null)
                instance = new TimeManager();
            return instance;
        }
    }
    private int mTimeCount = 0;

    public int TimeCount
    {
        get { return mTimeCount; }
    }

    public void TimeInfo()
    {
        int time = 0;
        if (int.TryParse(Localization.Get("TimeCount"), out time))
        {
            mTimeCount = time;
        }
    }

    public void CountDown()
    {
        mTimeCount--;

    }

    public bool IsOver()
    {
        return mTimeCount < 0;
    }
    /// <summary>
    /// 倒计时
    /// </summary>
    public void ShowTime(Text text)
    {
        text.text = string.Format(Localization.Get("TimeShow") ,TimeCount);
    }
}
