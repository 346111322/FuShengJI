using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(TextWrop))]
public class LocalizationUI : MonoBehaviour
{
    public string Key;
    public bool isFit = false;
    void Start()
    {
        if (GetComponent<Text>() != null)
        {
            string str = LocalizationManager.GetTextUI(Key);
            //            var text = GetComponent<Text>();
            //            if (text == null)
            //            {
            //                text = gameObject.AddComponent<Text>();
            //            }
            //            text.text = str;
            //            textWrop.isChange = true;
            //            Debug.Log(str);
            if (!string.IsNullOrEmpty(str))
                TextWrop.Instance.ShowContent(GetComponent<Text>(), str, isFit);
        }
    }
}
