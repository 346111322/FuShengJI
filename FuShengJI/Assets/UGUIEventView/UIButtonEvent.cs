using UnityEngine;
using UnityEngine.EventSystems;


//[RequireComponent(typeof(Collider))]
//[AddComponentMenu("UI/Button")]

public class UIButtonEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static AudioClip DefaultClickSound = null;
    public static AudioClip DefaultCancelSound = null;
    public static AudioClip DefaultSelectSound = null;
    public static AudioClip DefaultPopUpSound = null;


    public delegate void OnClickEvent(GameObject btn, object sender = null);
    public delegate void OnPressEvent(GameObject btn, object sender = null);

    public OnClickEvent Callback { get; set; }
    public OnPressEvent PressDownCallback { get; set; }
    public OnPressEvent PressUpCallback { get; set; }
    public object SenderParam { get; set; }

    public AudioClip ClickSound;
    [System.NonSerialized]
    public bool IsNeedSound = true;

    void Awake()
    {
        //加载音效
        //        if (DefaultClickSound == null)
        //        {
        //            AssetLoader.Instance.StartCoroutine(AssetLoader.Instance.WaitForLoadAssetOpti(AssetType.AudioUI,
        //             SettingReader.Instance.Get("Button_Click_Default_Sound"), -1,
        //              (data, param) =>
        //              {
        //                  DefaultClickSound = (AudioClip)data;
        //              }, null, false, null));
        //        }
        //
        //        if (DefaultCancelSound == null)
        //        {
        //            AssetLoader.Instance.StartCoroutine(AssetLoader.Instance.WaitForLoadAssetOpti(AssetType.AudioUI,
        //            SettingReader.Instance.Get("Button_Cancel_Default_Sound"), -1,
        //             (data, param) =>
        //             {
        //                 DefaultCancelSound = (AudioClip)data;
        //             }, null, false, null));
        //        }
        //
        //        if (DefaultSelectSound == null)
        //        {
        //            AssetLoader.Instance.StartCoroutine(AssetLoader.Instance.WaitForLoadAssetOpti(AssetType.AudioUI,
        //                SettingReader.Instance.Get("Button_select_Default_Sound"), -1,
        //                (data, param) =>
        //                {
        //                    DefaultSelectSound = (AudioClip)data;
        //                }, null, false, null));
        //        }
        //
        //        if (DefaultPopUpSound == null)
        //        {
        //            AssetLoader.Instance.StartCoroutine(AssetLoader.Instance.WaitForLoadAssetOpti(AssetType.AudioUI,
        //                SettingReader.Instance.Get("Button_pop_Default_Sound"), -1,
        //                (data, param) =>
        //                {
        //                    DefaultPopUpSound = (AudioClip)data;
        //                }, null, false, null));
        //        }
    }
    void SetSoundClip(string name, AudioClip sound)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPressUp();
    }

    void PlaySound()
    {
        //        if (ClickSound == null)
        //        {
        //            ClickSound = DefaultSelectSound;
        //            if (!IsNeedSound)
        //            {
        //                ClickSound = null;
        //            }
        //        }
        //
        //        if (ClickSound != null)
        //        {
        //            AudioUtil.PlaySound(ClickSound);
        //        }   
    }

    void OnClick()
    {
        if (null != Callback)
        {
            Callback(this.gameObject, SenderParam);
        }
    }
    void OnPressDown()
    {
        if (null != PressDownCallback)
        {
            PressDownCallback(this.gameObject, SenderParam);
        }
        PlaySound();
    }

    void OnPressUp()
    {
        if (null != PressUpCallback)
        {
            PressUpCallback(this.gameObject, SenderParam);
        }
    }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    public bool isTest;
    public string eventName;

    void Update()
    {
        if (isTest)
        {
            isTest = false;
            eventName = Callback.ToString() + "->" + Callback.Target;
        }
    }
#endif

}
