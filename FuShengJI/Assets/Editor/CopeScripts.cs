﻿using UnityEngine;
using System.Collections;
using UnityEditor.ProjectWindowCallback;
using System.IO;
using UnityEditor;

public class CopySelectScript
{
    static string path = "";
    public static string oldFileName = "";
    [MenuItem("Assets/CopySelectScript", false, 0)]//Create/
    public static void CreateBmobTab()
    {
        var file = Selection.activeObject;
        if (file!= null && file.GetType() != typeof(File))
        {
            path = AssetDatabase.GetAssetPath(file);
            oldFileName = file.name;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,ScriptableObject.CreateInstance<CopyScriptAssetAction>(),
                GetSelectedPathOrFallback() + "/NewCloneScript.cs",
                null,
                path);
        }
    }
    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}

class CopyScriptAssetAction : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        //创建资源
        UnityEngine.Object obj = CreateAssetFromTemplate(pathName, resourceFile);
        //高亮显示该资源
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }
    internal static UnityEngine.Object CreateAssetFromTemplate(string pahtName, string resourceFile)
    {
        //获取要创建的资源的绝对路径
        string fullName = Path.GetFullPath(pahtName);
        //读取本地模板文件
        StreamReader reader = new StreamReader(resourceFile);
        string content = reader.ReadToEnd();
        reader.Close();

        //获取资源的文件名
        string fileName = Path.GetFileNameWithoutExtension(pahtName);
        string oldFileName = Path.GetFileNameWithoutExtension(resourceFile);
        //替换默认的文件名
        //content = content.Replace("#TIME", System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss dddd"));
        content = content.Replace(oldFileName, fileName);

        //写入新文件
        StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
        writer.Write(content);
        writer.Close();

        //刷新本地资源
        AssetDatabase.ImportAsset(pahtName);
        AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath(pahtName, typeof(UnityEngine.Object));
    }
}
