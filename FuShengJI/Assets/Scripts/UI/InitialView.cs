using BH.Core.Base;
using UnityEngine;

public class InitialView : BaseView {

    public GameObject mStartBtn;
	void Start () 
    {
		AddButtonEvent(mStartBtn,StartGame);
	}

    private void StartGame(GameObject btn, object sender = null)
    {
        //开始初始化游戏
        Debug.Log("开始初始化游戏");
        UIManager.Instance.ShowUIView(View.MainView);
//        TipProxy.Instance.ShowTip("提示提示出现！！！！！！！！！");
    }

}
