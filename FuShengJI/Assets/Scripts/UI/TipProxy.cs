using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipProxy
{
    public static void ShowTip(string tip)
    {
        var tipGo = UIManager.Instance.ShowUIView(View.TipView);
        if(tipGo!=null)
            tipGo.GetComponent<TipView>().Show(tip);
    }
}
