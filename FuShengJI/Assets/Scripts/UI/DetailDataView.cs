using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public class DetailDataView : BaseView
{
    public static DetailDataView instance;

    void Awake()
    {
        instance = this;
    }
    public Text mNameText;
    public Texture mIconImg;
    public Text mCashText;
    public Text mDepositText;
    public Text mDebtText;
    public Text mHealthText;
    public Text mReputeText;
    public Text mTimeText;


    public void UpdatePlayData()
    {
        mCashText.text = Localization.Get("现金： ") + PlayerData.Instance.Cash;
        mDepositText.text = Localization.Get("存款： ") + PlayerData.Instance.Deposit;
        mDebtText.text = Localization.Get("欠债： ") + PlayerData.Instance.Debt;
        mHealthText.text = Localization.Get("健康： ") + PlayerData.Instance.Health;
        mReputeText.text = Localization.Get("名声： ") + PlayerData.Instance.Repute;
        //更新倒计时
        TimeManager.Instance.ShowTime(mTimeText);
    }
}
