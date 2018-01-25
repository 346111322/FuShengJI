using BH.Core.Base;
using UnityEngine;

public class FunctionInfo : BaseView
{
    public static FunctionInfo instance;

    public GameObject mBankObj;
    public GameObject mRentalObj;
    public GameObject mHospitalObj;
    public GameObject mPostofficeObj;
    public GameObject mInternetObj;
    public GameObject mRankingObj;
    void Awake()
    {
        instance = this;
        AddButtonEvent(mBankObj, OnClickBank);
        AddButtonEvent(mRentalObj, OnClickRental);
        AddButtonEvent(mHospitalObj, OnClickHospital);
        AddButtonEvent(mPostofficeObj, OnClickPostoffice);
        AddButtonEvent(mInternetObj, OnClickInternet);
        AddButtonEvent(mRankingObj, OnClickRanking);
    }

    /// <summary>
    /// 开启排行榜
    /// </summary>
    private void OnClickRanking(GameObject btn, object sender)
    {
        Debug.Log("开启排行榜");
        UIManager.Instance.ShowUIView(View.RankingListView);
        RankingListView.instance.UpdateView();
    }

    /// <summary>
    /// 开启网吧
    /// </summary>
    private void OnClickInternet(GameObject btn, object sender)
    {
        Debug.Log("开启网吧");
        UIManager.Instance.ShowUIView(View.WangbaView);
    }
    /// <summary>
    /// 开启邮局 还钱
    /// </summary>
    private void OnClickPostoffice(GameObject btn, object sender)
    {
        Debug.Log("开启邮局 还钱");
        UIManager.Instance.ShowUIView(View.PostofficeView);
        if(PostofficeView.instance != null)
            PostofficeView.instance.ShoePostofficeView();
    }
    /// <summary>
    /// 开启医院
    /// </summary>
    private void OnClickHospital(GameObject btn, object sender)
    {
        Debug.Log("开启医院");
        UIManager.Instance.ShowUIView(View.HospitalView);
        if (HospitalView.instance != null)
            HospitalView.instance.ShowHospitalView();
    }
    /// <summary>
    /// 开启中间 找房
    /// </summary>
    private void OnClickRental(GameObject btn, object sender)
    {
        Debug.Log("开启中间 找房");
        UIManager.Instance.ShowUIView(View.RentalView);
        if (RentalView.instance != null)
            RentalView.instance.ShowRentalView();
    }
    /// <summary>
    /// 开启银行
    /// </summary>
    private void OnClickBank(GameObject btn, object sender)
    {
        Debug.Log("开启银行");
        UIManager.Instance.ShowUIView(View.BankView);
    }
}
