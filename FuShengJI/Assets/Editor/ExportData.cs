//********************************************************************
// 文件名: ExportData.cs
// 描述: 
// 作者: ZWB
// 创建时间: 
//
// 修改历史:
// ZWB创建,实现主要功能
//********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// 
/// </summary>
public class ExportData : EditorWindow
{
    public enum ExportStep
    {
        STEP_BEGIN,

        // Export Hero
        STEP_EXPORT_HERO_BEGIN,
        STEP_DEL_RESLIB_HERO,
        STEP_DEL_BATTLE_HEROES,
        STEP_CPY_HEROES_TEX,
        STEP_CPY_HEROES_FBX,
        STEP_SET_HEROES_TEX,
        STEP_SET_HERO_ANIM,
        STEP_CREATE_HEROES_PREFAB,
        STEP_DEL_EXPORT_PATH_HEROES,
        STEP_EXPORT_HEROES,
        STEP_EXPORT_HEROES_TEX,
        STEP_EXPORT_WINGS_TEX,
        STEP_EXPORT_HERO_END,

        // Weapon
        STEP_EXPORT_WEAPON_BEGIN,
        STEP_DEL_RESLIB_WEAPONS,
        STEP_DEL_BATTLE_SKILL_WEAPONS,
        STEP_CPY_WEAPONS_TEX,
        STEP_SET_WEAPONS_TEX,
        STEP_CPY_WEAPONS_FBX,
        STEP_SET_WEAPON_ANIM,
        STEP_CREATE_WEAPONS_PREFAB,
        STEP_DEL_EXPORT_PATH_WEAPONS,
        STEP_EXPORT_WEAPONS,
        STEP_EXPORT_WEAPONS_TEX,
        STEP_EXPORT_WEAPON_END,

        // Skill(Anim)
        STEP_EXPORT_SKILL_BEGIN,
        STEP_DEL_RESLIB_SKILL,
        STEP_DEL_BATTLE_SKILL_ANIM,
        STEP_CPY_SKILL_FBX,
        STEP_SET_SKILL_ANIM,
        STEP_EXPORT_SKILL_FBX,
        STEP_DEL_EXPORT_PATH_SKILL_ANIM,
        STEP_EXPORT_SKILL_ANIM,
        STEP_EXPORT_SKILL_END,

        // SkillConfig(xml)
        STEP_EXPORT_SKILL_CFG_BEGIN,
        STEP_DEL_EXPORT_PATH_SKILL_CFG,
        STEP_EXPORT_SKILL_CFG,
        STEP_EXPORT_SKILL_CFG_END,

        // Effect
        STEP_EXPORT_EFFECT_BEGIN,
        STEP_DEL_EXPORT_PATH_EFFECT,
        STEP_SET_EFFECT_LAYER,
        STEP_EXPORT_EFFECT,
        STEP_EXPORT_EFFECT_END,

        // Lightning
        STEP_EXPORT_LIGHTNING_BEGIN,
        STEP_DEL_EXPORT_PATH_LIGHTNING,
        STEP_SET_LIGHTNING_LAYER,
        STEP_EXPORT_LIGHTNING,
        STEP_EXPORT_LIGHTNING_END,

        // DataTable
        STEP_EXPORT_DATA_TABLE_BEGIN,
        STEP_DEL_EXPORT_PATH_DATA_TABLE,
        STEP_EXPORT_DATA_TABLE,
        STEP_EXPORT_DATA_TABLE_END,

        // Icon
        STEP_EXPORT_ICON_BEGIN,
        STEP_DEL_EXPORT_PATH_ICON,
        STEP_EXPORT_HALFPORTRAIT_SMALL,
        STEP_EXPORT_HALFPORTRAIT_BIG,
        STEP_EXPORT_ITEMICON,
        STEP_EXPORT_HEROICON,
        STEP_EXPORT_ICON_END,

        // BgIcon
        STEP_EXPORT_BG_ICON_BEGIN,
        STEP_DEL_EXPORT_PATH_BG_ICON,
        STEP_EXPORT_BG_ICON,
        STEP_EXPORT_BG_ICON_END,


        //Audio
        STEP_EXPORT_MUSIC_COMMON_BEGIN,
        STEP_DEL_EXPORT_PATH_MUSIC_COMMON,
        STEP_EXPORT_MUSIC_COMMON,
        STEP_EXPORT_AUDIO_UI_COMMON,
        STEP_EXPORT_AUDIO_DRAMMA_COMMON,
        STEP_EXPORT_AUDIO_VOICE_COMMON,
        STEP_EXPORT_AUDIO_NEWBIE_COMMON,
        STEP_EXPORT_AUDIO_BATTLE,
        STEP_EXPORT_SKILL_VOICE_BATTLE,
        STEP_EXPORT_MUSIC_COMMON_END,

        //LanguageTable
        STEP_EXPORT_LANGUAGE_TABLE_BEGIN,
        STEP_DEL_EXPORT_PATH_LANGUAGE_TABLE,
        STEP_EXPORT_LANGUAGE_TABLE,
        STEP_EXPORT_LANGUAGE_TABLE_END,

        // Campaign
        STEP_EXPORT_CAMPAIGN_BEGIN,
        STEP_DEL_EXPORT_PATH_CAMPAIGN,
        STEP_EXPORT_CAMPAIGNSCENE,
        STEP_EXPORT_CAMPAIGN_END,

        // Scene
        STEP_EXPORT_SCENE_BEGIN,
        STEP_DEL_EXPORT_PATH_BATTLE_FIELD,
        STEP_EXPORT_BATTLE_FIELD_CFG,

        STEP_EXPORT_SCENE_WEIGUO,
        STEP_EXPORT_SCENE_HAIBIAN001,
        STEP_EXPORT_SCENE_HAIBIAN002,
        STEP_EXPORT_SCENE_XUEDI,
        STEP_EXPORT_SCENE_SHAMO001,
        STEP_EXPORT_SCENE_SHAMO002,
        STEP_EXPORT_SCENE_FuDao001,
        STEP_EXPORT_SCENE_MingJie,
        STEP_EXPORT_SCENE_ZhongGuoFeng001,
        STEP_EXPORT_SCENE_ZhongGuoFeng002,
        STEP_EXPORT_SCENE_SHUGUO,
        STEP_EXPORT_SCENE_WUGUO,
        STEP_EXPORT_SCENE_WUSHUANG01,
        STEP_EXPORT_SCENE_WUSHUANG02,
        STEP_EXPORT_SCENE_WUSHUANG03,
        STEP_EXPORT_SCENE_FangShou,
        STEP_EXPORT_SCENE_HuLaoGuan,
        STEP_EXPORT_SCENE_END,

        STEP_END
    };

    public enum ExportType
    {
        EXPORT_HERO,
        EXPORT_WEAPON,
        EXPORT_SKILL_ANIM,
        EXPORT_SKILL_CFG,
        EXPORT_EFFECT,
        EXPORT_LIGHTNING,
        EXPORT_DATA_TABLE,
        EXPORT_ICON,
        EXPORT_BG_ICON,
        EXPORT_AUDIO,
        EXPORT_LANGUAGE_TABLE,
        EXPORT_CAMPAIGN,
        EXPORT_SCENE,
    };

    public class ExportTypeInfo
    {
        public string mName;
        public bool mIsSelect;
        public ExportType mType;
        public bool mIsExport;

        private ExportTypeInfo()
        {
        }

        public ExportTypeInfo(string name, bool isSelect, ExportType type)
        {
            mName = name;
            mIsSelect = isSelect;
            mType = type;
            mIsExport = false;
        }
    }
    #region static
    static ExportData m_CurExportPanel;
    #endregion
    //根据Excel生成脚本和TXT（用于外部调用）
    [MenuItem("Tools/DataTable")]
    static void OpenExportPanelForGenerateDataTable()
    {
        //        Debug.Log("读取文件夹内表格");
        //        DataModel.Instance.Init();
        //        //获取了文件夹内所有的表格
        //        //读取所有表格
        //        //通过xml进行cs的生成
        //        Debug.Log("根据表格生成txt");
        //        Debug.Log("根据表格生成脚本");

        m_CurExportPanel = GetWindow<ExportData>();
        m_CurExportPanel.Init();
        m_CurExportPanel.Show();
        //            m_CurExportPanel.m_IsBuildForVersion = false;
    }

    void Init()
    {
        m_GUIStyle = new GUIStyle();

        #region AssetBundle
        m_CurStep = ExportStep.STEP_BEGIN;
        m_TotalStep = ExportStep.STEP_END;

        RefreshExportInfo();
        #endregion

        #region DataTable
        m_IsStartExport = false;
        m_ExportTabNum = 0;
        m_TableConvertor = new DataTableConvertor();

        // 枚举出所有的配表
        m_DataFullPath = Application.dataPath + "/" + m_DataTablePath;
//        m_OtherDataFullPath = Application.dataPath + "/../" + m_OtherDataTablePath;
        RefreshTableData();
        #endregion
    }
    void RefreshExportInfo()
    {
        int idx = 0;
        foreach (string name in Enum.GetNames(typeof(ExportType)))
        {
            ExportTypeInfo info = new ExportTypeInfo(name, false, (ExportType)idx++);
        }
        mNextExportTypeInfo = null;
        m_IsBeginExport = false;
    }

    private int m_SelectTool = 0;
    GUIStyle m_GUIStyle;
    private bool m_IsBuildForEditor = false;            // 是否打编辑器资源，编辑器资源会直接打到FinalVersion下面
    private ExportTypeInfo mNextExportTypeInfo = null;     // 下一个资源的导出类型
    bool m_IsBeginExport = false;        // 是否开始导出
    public ExportStep m_BeginStep = ExportStep.STEP_BEGIN;   // 起始导出步骤
    public ExportStep m_CurStep = ExportStep.STEP_BEGIN;   // 当前导出步骤
    public ExportStep m_TotalStep = ExportStep.STEP_END;   // 导出的最终步骤
    public bool m_IsLastExportStepDone = true;             // 上一次导出步骤是否已经结束 
    public bool m_IsBuildForVersion = false;                // 标记当前导出是否是打版本


    // DataTable
    #region DataTable
    const string m_DataTablePath = "策划资源/Configs/GameTables/";
//    const string m_OtherDataTablePath = "美术资源/3D角色资源/导出资源/";
    string m_DataFullPath;
    string m_OtherDataFullPath;

    bool m_IsExportAllTable = false;
    Vector2 m_ScrollPos = Vector2.zero;
    Vector2 m_ExportScrollPos = Vector2.zero;
    bool m_IsExpanReverse = false;
    bool m_IsExpand = false;
    bool m_IsExportExpand = false;
    Dictionary<string, string> m_FilesDict;
    List<TableInfo> m_TableInfos;           // 所有的配表信息
    List<TableInfo> m_ExportTabInfos;       // 需要导出的配表信息

    TableInfo m_CurExportTabInfo;           // 当前导出表信息
    bool m_IsStartExport = false;           // 是否开始导出
    bool m_IsDoNextExport = true;           // 是否导出下一个
    int m_ExportTabNum = 0;                 // 导出表的数量
    int m_CurExportTabIdx = 0;              // 当前导表索引

    DataTableConvertor m_TableConvertor;
    #endregion

    void DrawExportDataTableWindow()
    {
        titleContent.text = "导出DataTable";
        // 出错了
        if (m_ExportTabInfos == null)
        {
            m_GUIStyle.fontSize = 30;
            m_GUIStyle.fontStyle = FontStyle.Bold;
            m_GUIStyle.normal.textColor = Color.red;
            GUILayout.Label("请重新扫描表格文件！！！", m_GUIStyle);
            if (GUILayout.Button("重新扫描表格文件", GUILayout.Width(500), GUILayout.Height(30)))
            {
                if (m_TableConvertor == null)
                    m_TableConvertor = new DataTableConvertor();
                RefreshTableData();
            }
            return;
        }

        EditorGUILayout.Separator();
        GUI.contentColor = Color.white;
        m_IsExportAllTable = EditorGUILayout.Toggle("是否要导出全部表格", m_IsExportAllTable, GUILayout.Width(300));
        EditorGUILayout.Separator();
        GUI.contentColor = Color.green;
        GUILayout.TextArea("【导出】：将所选Excel表全部导出为txt格式，并生成对应的脚本如Table_HeroAttr.cs。如果是全部导出，则在导出完毕后会自动生成DataTable.cs文件；如果是部分导出且增加了表格文件，则需要【导出DataTable.cs】。", GUILayout.MaxWidth(500));
        GUI.contentColor = Color.white;
        if (GUILayout.Button("导出", GUILayout.Width(500), GUILayout.Height(50)))
        {
            if (!m_IsExportAllTable)
            {
                if (m_ExportTabInfos.Count == 0)
                {
                    EditorUtility.DisplayDialog("提示", "请选择需要导出的表格", "关闭");
                    return;
                }
                if (EditorUtility.DisplayDialog("提示!!!", "确定要导出所选表格？", "确定", "取消"))
                    StartExport();
            }
            else
            {
                if (EditorUtility.DisplayDialog("提示!!!", "确定要导出所有表格？", "确定", "取消"))
                {
                    StartExport();
                }
            }
        }

        EditorGUILayout.Separator();
        GUI.contentColor = Color.green;
        GUILayout.TextArea("【转为二进制】：将所选Excel表导出的txt文件全部转出二进制格式，此功能仅用于测试。", GUILayout.MaxWidth(500));
        GUI.contentColor = Color.white;
        if (GUILayout.Button("转为二进制", GUILayout.Width(500), GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("提示!!!", "确定要将所选表格转为二进制？", "确定", "取消"))
            {
                string[] convertFiles;
                int fileCnt = 0;
                string fileName = "";
                if (m_IsExportAllTable)
                {
                    fileCnt = m_TableInfos.Count;
                    convertFiles = new string[fileCnt];
                    for (int idx = 0; idx < fileCnt; ++idx)
                    {
                        fileName = Path.GetFileNameWithoutExtension(m_TableInfos[idx].m_FullName);
                        fileName += ".txt";
                        fileName = Application.dataPath + m_TableConvertor.m_TableExportPath + fileName;
                        convertFiles[idx] = fileName;
                    }
                }
                else
                {
                    fileCnt = m_ExportTabInfos.Count;
                    convertFiles = new string[fileCnt];
                    for (int idx = 0; idx < fileCnt; ++idx)
                    {
                        fileName = Path.GetFileNameWithoutExtension(m_ExportTabInfos[idx].m_FullName);
                        fileName += ".txt";
                        fileName = Application.dataPath + m_TableConvertor.m_TableExportPath + fileName;
                        convertFiles[idx] = fileName;
                    }
                }
                //                TxtConvertTool.ConvertSelectedTxt(convertFiles);
            }
        }
        EditorGUILayout.Separator();
        if (GUILayout.Button("重新扫描表格文件", GUILayout.Width(500), GUILayout.Height(30)))
        {
            RefreshTableData();
        }
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打开脚本导出目录", GUILayout.MaxWidth(240), GUILayout.Height(30)))
        {
            string exportPath = Application.dataPath + "/ExportDataByExcel/Scripts/Core/Configs/DataTable/";
            Process.Start(exportPath);
        }
        GUILayout.Space(20);
        if (GUILayout.Button("打开TXT导出目录", GUILayout.MaxWidth(240), GUILayout.Height(30)))
        {
            string exportPath = Application.dataPath + "/Resources/DataTable/";
            Process.Start(exportPath);
        }
        EditorGUILayout.EndHorizontal();
        DrawReverseUI();

        DrawSelectTableUI();

        if (m_IsStartExport)
        {
            float percent = 0;
            if (m_IsExportAllTable)
            {
                percent = (m_CurExportTabIdx + 1) / (float)m_TableInfos.Count;
            }
            else
            {
                percent = (m_CurExportTabIdx + 1) / (float)m_ExportTabInfos.Count;
            }
            EditorUtility.DisplayProgressBar("当前导出表格：" + m_CurExportTabInfo.m_RealName + "...", percent.ToString("0%"), percent);
        }
    }

    public void StartExport()
    {

        m_IsStartExport = true;
        m_IsDoNextExport = true;
        if (m_IsExportAllTable)
        {
            m_ExportTabNum = m_TableInfos.Count;
        }
        else
        {
            m_ExportTabNum = m_ExportTabInfos.Count;
        }
        m_CurExportTabIdx = -1;
        Next();
    }

    void Next()
    {
        m_CurExportTabIdx++;
        if (m_IsExportAllTable)
        {
            if (m_CurExportTabIdx >= m_TableInfos.Count)
            {
                m_TableConvertor.ExportDataTableScript(m_TableInfos);
                EndExport();
                return;
            }
            m_CurExportTabInfo = m_TableInfos[m_CurExportTabIdx];
        }
        else
        {
            if (m_CurExportTabIdx >= m_ExportTabInfos.Count)
            {
                EndExport();
                return;
            }
            m_CurExportTabInfo = m_ExportTabInfos[m_CurExportTabIdx];
        }
        Repaint();
        m_IsDoNextExport = true;
    }

    void EndExport()
    {
        m_IsStartExport = false;
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    void DrawReverseUI()
    {
        return;
        //        GUI.contentColor = Color.white;
        //        EditorGUILayout.Separator();
        //        m_IsExpanReverse = EditorGUILayout.Foldout(m_IsExpanReverse, "文件反解");
        //        if (m_IsExpanReverse)
        //        {
        //            GUILayout.BeginHorizontal();
        //            GUI.contentColor = Color.green;
        //            GUILayout.Space(30);
        //            m_CustomTarget = (CustomTarget)EditorGUILayout.EnumPopup("选择平台", m_CustomTarget, GUILayout.Width(300));
        //            GUILayout.EndHorizontal();
        //            EditorGUILayout.Separator();
        //            GUI.contentColor = Color.green;
        //            if (m_CustomTarget == CustomTarget.None)
        //            {
        //                m_StrBuilder.Remove(0, m_StrBuilder.Length);
        //                m_StrBuilder.Append("请选择平台！！！\n");
        //                m_GUIStyle.fontSize = 20;
        //                m_GUIStyle.normal.textColor = Color.red;
        //                GUILayout.BeginHorizontal();
        //                GUILayout.Space(30);
        //                GUILayout.Label(m_StrBuilder.ToString(), m_GUIStyle);
        //                GUILayout.EndHorizontal();
        //                return;
        //            }
        //            DrawVersionUI();
        //            EditorGUILayout.Separator();
        //            GUILayout.BeginHorizontal();
        //            GUILayout.Space(30);
        //            if (GUILayout.Button("反解AssetBundle", GUILayout.Width(300), GUILayout.Height(30)))
        //            {
        //                if (EditorUtility.DisplayDialog("提示!!!", "确定要将所选表格从AssetBundle格式转为txt格式？", "确定", "取消"))
        //                    InverseAssetBundle();
        //            }
        //            GUILayout.EndHorizontal();
        //
        //            EditorGUILayout.Separator();
        //            GUILayout.BeginHorizontal();
        //            GUILayout.Space(30);
        //            if (GUILayout.Button("反解二进制文件", GUILayout.Width(300), GUILayout.Height(30)))
        //            {
        //                if (EditorUtility.DisplayDialog("提示!!!", "确定要将所选表格从二进制格式转为txt格式？", "确定", "取消"))
        //                    InverseBinary();
        //            }
        //            GUILayout.EndHorizontal();
        //        }
    }

    void DrawSelectTableUI()
    {
        int cnt = 0;
        TableInfo info = null;
        GUI.contentColor = Color.white;
        EditorGUILayout.Separator();
        if (!m_IsExportAllTable)
        {
            EditorGUILayout.Separator();
            if (GUILayout.Button("导出DataTable.cs", GUILayout.Width(500), GUILayout.Height(30)))
            {
                m_TableConvertor.ExportDataTableScript(m_TableInfos);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(240));
            m_IsExpand = EditorGUILayout.Foldout(m_IsExpand, "客户端配表");
            if (m_IsExpand)
            {
                cnt = m_TableInfos.Count;

                for (int idx = 0; idx < cnt; ++idx)
                {
                    info = m_TableInfos[idx];
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(240));
                    GUILayout.Space(30);
                    info.m_IsSelected = EditorGUILayout.ToggleLeft(info.m_RealName, info.m_IsSelected, GUILayout.Width(240));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
            cnt = m_ExportTabInfos.Count;
            if (cnt > 0)
            {
                GUI.contentColor = Color.green;
                m_ExportScrollPos = EditorGUILayout.BeginScrollView(m_ExportScrollPos, GUILayout.Width(240));
                m_IsExportExpand = EditorGUILayout.Foldout(m_IsExportExpand, "导出配表");
                if (m_IsExportExpand)
                {
                    GUI.contentColor = Color.green;
                    for (int idx = 0; idx < cnt; ++idx)
                    {
                        info = m_ExportTabInfos[idx];
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(240));
                        GUILayout.Space(30);
                        EditorGUILayout.LabelField(info.m_RealName);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndHorizontal();

            if (!m_IsStartExport)
            {
                m_ExportTabInfos.Clear();
                cnt = m_TableInfos.Count;
                for (int idx = 0; idx < cnt; ++idx)
                {
                    info = m_TableInfos[idx];
                    if (info.m_IsSelected)
                    {
                        m_ExportTabInfos.Add(info);
                    }
                }
            }
        }
    }
    // 反解AssetBundle
    void InverseAssetBundle()
    {

    }
    // 反解二进制文件
    void InverseBinary()
    {

    }
    void OnGUI()
    {
        DrawExportDataTableWindow();
    }


    /// <summary>
    ///  有效类型
    /// </summary>
    public enum DataType
    {
        NONE,
        STRING,  // 字符串
        INT,     // 整型           [-2147483648~2147483647]
        FLOAT,   // 单精度浮点型    [-3.40282e+038f~3.40282e+038f]
        SHORT,   // 短整型         [-32768~32767]
        BOOL,    // 布尔           [0/1]
        BYTE     // 单字节         [0~255]
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum VariableType
    {
        Single,         // 单独变量
        Repeat          // 数组变量
    }

    /// <summary>
    /// 变量信息
    /// </summary>
    public class VariableInfo
    {
        public VariableType m_Type;     // 变量类型
        public DataType m_DataType;     // 数据类型
        public List<string> m_EnumStr;  // 枚举
        public List<int> m_ValueId;     // 值索引
        public string m_Name;           // 变量名
        public string m_BakName;        // 如果是数组变量，该成员用于记录第一个原始名字
        public int m_Count;
        public bool m_IsKey = false;    // 是否为Id
    }

    public void RefreshTableData()
    {
        m_FilesDict = new Dictionary<string, string>();
        m_TableInfos = new List<TableInfo>();
        m_ExportTabInfos = new List<TableInfo>();
        string[] files = null;
        if (!Directory.Exists(m_DataFullPath))
            return;
        files = Directory.GetFiles(m_DataFullPath, "*.xlsx", SearchOption.AllDirectories);
        int cnt = files.Length;
        TableInfo info = null;
        for (int idx = 0; idx < cnt; ++idx)
        {
            string file = files[idx];
            if (file.Contains("Server"))
                continue;
            string fileName = file.Substring(file.LastIndexOf('\\') + 1);
            fileName = fileName.Substring(0, fileName.IndexOf("."));
            if (m_FilesDict.ContainsKey(fileName))
            {
                Debug.LogError("The table named '" + fileName + "' has existed!");
                continue;
            }
            if (fileName.Contains("~"))
                continue;
            m_FilesDict.Add(fileName, file);

            info = new TableInfo();
            info.m_ExportClassName = DataTableConvertor.HandleName(fileName);
            info.m_RealName = fileName;
            info.m_FullName = file;
            info.m_IsSelected = false;
            m_TableInfos.Add(info);
        }

        // 对表格按字典排序
        m_TableInfos.Sort((TableInfo a, TableInfo b) =>
        {
            return a.m_RealName.CompareTo(b.m_RealName);
        });

        for (int i = 0; i < m_TableInfos.Count; i++)
        {
            m_TableConvertor.Convert(m_TableInfos[i]);
        }
        //        m_TableConvertor.ExportDataTableScript(m_TableInfos);
    }

    void Update()
    {
        if (m_TableInfos == null || m_ExportTabInfos == null || m_TableConvertor == null)
        {
            EndExport();
            return;
        }
        if (m_IsStartExport && m_IsDoNextExport)
        {
            Export();
        }
    }

    void Export()
    {
        m_IsDoNextExport = false;
        if (!m_TableConvertor.Convert(m_CurExportTabInfo))
        {
            EndExport();
            return;
        }
        Next();
    }
}

