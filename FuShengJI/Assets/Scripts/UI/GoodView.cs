using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public class GoodView : BaseView
{
    public static GoodView instance;
    public GameObject closeBtn;
    public GameObject confirmBtn;
    void Awake()
    {
        instance = this;
        AddButtonEvent(closeBtn, OnClickClose);
        AddButtonEvent(confirmBtn,OnClickConfirm);
    }

    private GoodItemData goodData;
    private void OnClickConfirm(GameObject btn, object sender)
    {
        int goodNum = int.Parse(inputText.text);
        if(!goodData.isDepot)
            PlayerData.Instance.BuyGoods(goodData, goodNum);
        else
            PlayerData.Instance.SellGoods(goodData, goodNum);
        gameObject.SetActive(false);
        DetailDataView.instance.UpdatePlayData();
    }

    private void OnClickClose(GameObject btn, object sender)
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame

    public Text titleText;
    public InputField inputText;
    public Text goodNameText;
    public Text curPriceText;
    public Text monseText;
    public Text goodNumText;

    public void Show(GoodItemData data)
    {
        gameObject.SetActive(true);
        goodData = data;
        goodNameText.text = Localization.Get("商品名称： ") + data.goodName;

        if (!data.isDepot)
        {
            //假如是仓库显示
            // 标题
            titleText.text = Localization.Get("买进");
            // 当前价格
            curPriceText.text = Localization.Get("当前价格： ") + data.goodPrice.ToString();
            // 剩余资金
            monseText.text = Localization.Get("剩余资金： ") + PlayerData.Instance.Cash.ToString();
            // 最多购买 剩余空间
            int leftNum = PlayerData.Instance.Cash / data.goodPrice;
            goodNumText.text = Localization.Get("最多购买： ") + (leftNum < PlayerData.Instance.LeftRoom ? leftNum : PlayerData.Instance.LeftRoom).ToString();
            inputText.text = (leftNum < PlayerData.Instance.LeftRoom ? leftNum : PlayerData.Instance.LeftRoom).ToString();
        }
        else
        {
            //标题
            titleText.text = Localization.Get("出售");
            // 当前价格
            GoodItemData item = GoodData.Instance.mMarketGoods.Find(a => a.goodId == data.goodId);
            if(item == null)
            {
                //表示没卖的
                TipProxy.ShowTip(Localization.Get("没卖的"));
                gameObject.SetActive(false);
                return;
            }
            curPriceText.text = Localization.Get("当前价格： ") + item.goodPrice.ToString();
            // 买进均价
            monseText.text = Localization.Get("买进均价： ") + data.goodPrice.ToString();
            // 最大盈亏
            goodNumText.text = Localization.Get("最大盈亏： ") + ((item.goodPrice -data.goodPrice) * data.goodNum).ToString();
            inputText.text = data.goodNum.ToString();
        }

    }
}
