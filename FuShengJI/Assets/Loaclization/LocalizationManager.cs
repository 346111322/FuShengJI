using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.Core.Utils;

public enum LanguageType
{
    Chinese = 0,
    English = 1,
}

public class LocalizationManager
{
    static Dictionary<string, string> mLanguageDict = new Dictionary<string, string>(); //存储语言字典
    static string resourceDir = "LoaclizationData";//解析的二级目录
    static string m_LocalFileName = "/策划资源/Configs/GameTables/Localization.txt";//"/策划资源/Configs/GameTables/Localization.txt";//
    static LanguageType curLanguage = LanguageType.Chinese;

    /// <summary>
    /// 初始化字典
    /// </summary>
    static LocalizationManager()
    {
        ParseLocalizationFile();
    }

    /// <summary>
    /// 读取并储存为字典
    /// </summary> 
    static void ParseLocalizationFile()
    {
        switch (curLanguage)
        {
            case LanguageType.Chinese:
                m_LocalFileName = resourceDir + "/Chinese";
                break;
            case LanguageType.English:
                m_LocalFileName = resourceDir + "/English";
                break;
        }
        Debug.Log("地址：" + (m_LocalFileName));
        TextAsset text = Resources.Load<TextAsset>(m_LocalFileName);
        if (text!= null)
        {
            Debug.Log("hahahahahahahh");
            mLanguageDict = ReadDictionary(text.text);
        }
        else
        {
            Debug.Log("不存在这个地址");
        }
    }
    /// <summary>
    /// 生成语言字典
    /// </summary>
    public static Dictionary<string, string> ReadDictionary(string str)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] lines = str.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("//"))
            {
                continue;
            }
            string[] splites = line.Split(new char[] { '=' }, 2);
            if (2 == splites.Length)
            {
                try
                {
                    dic.Add(splites[0].Trim(), splites[1].Trim());
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError("The key '" + splites[0].Trim() + "' " + ex.ToString());
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Parse Error: " + line);
            }
        }
        return dic;
    }


    /// <summary>
    /// 根据键值设置ui文字
    /// </summary>
    /// <param name="varKey"></param>
    /// <returns></returns>
    public static string GetTextUI(string varKey)
    {
        if (string.IsNullOrEmpty(varKey))
        {
            return null;
        }
        string tempText;
        if (mLanguageDict.TryGetValue(varKey, out tempText))
        {
            return tempText;
        }
        return varKey;
    }


}
