using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipView : MonoBehaviour
{
    public Text tipLabel;

//    void Start()
//    {
//        StartCoroutine(HideTip());
//    }
    public void Show(string tip)
    {
        tipLabel.text = tip;
        StartCoroutine(HideTip());
    }

    private IEnumerator HideTip()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject.Destroy(gameObject);
    }

    void Destroy()
    {
        Debug.Log("释放所有数据");
        tipLabel.text = null;
    }
}
