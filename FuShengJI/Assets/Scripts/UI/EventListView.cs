using System.Collections.Generic;
using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public class EventListView : BaseView
{
    public static EventListView instance;
    public GameObject mConfirmBtn;
    public Text mEventContent;

    static EventListView()
    {

    }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AddButtonEvent(mConfirmBtn, OnClickConfirm);
    }

    private void OnClickConfirm(GameObject btn, object sender = null)
    {
        eventIdx++;
        ShowEvent();
    }
    /// <summary>
    /// 事件集合
    /// </summary>
    private List<string> eventList = new List<string>();
    private int eventIdx = 0;
    public void ShowEvent(List<string> eventList)
    {
        if(eventList == null || eventList.Count <= 0)
        {
            UIManager.Instance.Hide(View.EventView);
            return;
        }
        this.eventList = eventList;
        eventIdx = 0;
        ShowEvent();
    }

    public void ShowEvent()
    {
        if (eventList.Count > eventIdx && eventList[eventIdx] != null)
        {
            mEventContent.text = eventList[eventIdx];
            UIManager.Instance.ShowUIView(View.EventView);
        }
        else
        {
            UIManager.Instance.Hide(View.EventView);
        }
    }
}
