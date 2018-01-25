using System;
using BH.Core.Base;
using UnityEngine;
using UnityEngine.UI;

public class GoodItem : BaseView
{
    public Image icon;
    public Text goodName;
    public Text goodPrice;
    public Text goodDetails;
    public GoodItemData goodData;

    void Start()
    {
        AddButtonEvent(gameObject, ShowGoodInfo);
    }

    private void ShowGoodInfo(GameObject btn, object sender)
    {
        UIManager.Instance.ShowUIView(View.GoodView);
        if (GoodView.instance != null)
        {
            GoodView.instance.Show(goodData);
        }
    }


    public void ShowGoodByMarket(GoodItemData data)
    {
        goodData = data;
        icon.sprite = data.iconSprite;
        goodName.text = data.goodName;
        goodPrice.text = "<color=#B25D5D64>￥</color>" + data.goodPrice.ToString();
        goodDetails.text = String.Format("名声影响：{0} 价格范围：￥{1}至￥{2}", data.goodData.Repute, data.goodData.PriceMin, data.goodData.PriceMax);
    }

    public void ShowGoodByDepot(GoodItemData data)
    {
        goodData = data;
        icon.sprite = data.iconSprite;
        goodName.text = data.goodName;
        goodPrice.text = "￥" + data.goodPrice.ToString();
        goodDetails.text = String.Format("持有数量：{0}个，商品总值：￥{1}", data.goodNum, data.goodPrice * data.goodNum);
    }

}
