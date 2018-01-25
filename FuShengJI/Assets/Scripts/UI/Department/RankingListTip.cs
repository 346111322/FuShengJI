using System.Collections;
using System.Collections.Generic;
using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public class RankingListTip : BaseView
{
    public InputField inputName;
    public InputField inputMsg;
    public GameObject confirmObj;

    void Awake()
    {
        AddButtonEvent(confirmObj, OnClickConfirm);
    }

    private void OnClickConfirm(GameObject btn, object sender)
    {
        if(string.IsNullOrEmpty(inputName.text))
        {
            TipProxy.ShowTip(Localization.Get("请输入你的名字"));
            return;
        }
        PlayerData.Instance.mName = inputName.text;
        UIManager.Instance.ShowUIView(View.RankingListView);
        RankingModel.Instance.UpdateRankingList(new BmobTab_RankingList(PlayerData.Instance.mName,
            PlayerData.Instance.Repute, PlayerData.Instance.Health, PlayerData.Instance.Money, inputMsg.text));

        UIManager.Instance.Hide(View.RankingTipView);
    }
}
