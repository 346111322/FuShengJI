using System;
using UnityEngine;
using System.Collections;
using cn.bmob.api;
using cn.bmob.tools;
using cn.bmob.io;
using System.Collections.Generic;
using cn.bmob.exception;
using UnityEngine.UI;

public class BmobManager : MonoBehaviour
{
    public static BmobManager Instance;
    private BmobUnity Bmob;

    static BmobManager()
    {
        GameObject go = new GameObject("BmobManager");
        DontDestroyOnLoad(go);
        Instance = go.GetComponent<BmobManager>() ?? go.AddComponent<BmobManager>();
    }
    void Awake()
    {
        BmobDebug.Register(print);
        Bmob = gameObject.GetComponent<BmobUnity>() ?? gameObject.AddComponent<BmobUnity>();// gameObject.GetComponent<BmobUnity>();
        Bmob.ApplicationId = TestBmob.ApplicationId;
    }

    #region
    /// <summary>
    /// 插入数据
    /// </summary>
    public void InsertData<T>(T mg, string tabName) where T : BmobTable
    {
        Bmob.Create(tabName, mg, (resp, exception) =>
        {
            if (exception != null)
            {
                Debug.Log("保存失败，原因： " + exception.Message);
            }
            else
            {
                Debug.Log("保存成功" + resp.createdAt);
            }
        });
    }

//    public bool isGetAllOver = false;
//    /// <summary>
//    /// 获取表所以信息 查询条件符合的所有数据：
//    /// </summary>
//    public void getAllInfo<T>(BmobQuery query, string tabName,List<T> list) where T : MyGameTable
//    {
//        isGetAllOver = false;
//        Bmob.Find<T>(tabName, query, (resp, exception) =>
//        {
//            if (exception != null)
//            {
//                print("查询失败, 失败原因为： " + exception.Message);
//                return;
//            }
//            //对返回结果进行处理
//            isGetAllOver = true;
//            list = resp.results;
//        });
//    }



    public IEnumerator ConditionFind<T>(BmobQuery query, string tabName,Action<List<T>, bool> onOk)
    {
        Bmob.Find<T>(tabName, query, (resp, exception) =>
        {
            if (exception != null)
            {
                print("查询失败, 失败原因为： " + exception.Message);
                onOk(null, true);
                return;
            }
            //对返回结果进行处理
            onOk(resp.results, true);
        });
        yield break;
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    public T getRecoard<T>(string objectId, string tabName) where T : BmobTable
    {
        T game = null;
        Bmob.Get<T>(tabName, objectId, (resp, exception) =>
        {
            if (exception != null)
            {
                Debug.Log("查询失败, 失败原因为： " + exception.Message);
                return;
            }
            game = resp;
        });
        return game;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void updateData<T>(T mg, string objectId, string tabName) where T : BmobTable
    {
        Bmob.Update(tabName, objectId, mg, (resp, exception) =>
        {
            if (exception != null)
            {
                Debug.Log("保存失败, 失败原因为： " + exception.Message);
                return;
            }
            Debug.Log("保存成功, @" + resp.updatedAt);
        });
    }
    /// <summary>
    /// 删除数据
    /// </summary>
    public void deleteData(string objectId, string tabName)
    {
        Bmob.Delete(tabName, objectId, (resp, exception) =>
        {
            if (exception != null)
            {
                Debug.Log("删除失败, 失败原因为： " + exception.Message);
                return;
            }
            else
            {
                Debug.Log("删除成功, @" + resp.msg);
            }
        });
    }
    #endregion

}
