/// name: 客户端导表工具
/// author: 朱孔春
/// usedto: 用于将excel表格转换成对应的txt和脚本，转换过程中对表内数据进行有效性检查
/// 支持单表导出！！！
/// 
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using BH.Core.Utils;

namespace UnityEditor
{
    public class TableInfo
    {
        public string m_ExportClassName;
        public string m_RealName;
        public string m_FullName;
        public bool m_IsSelected = false;
    }

    public class DataTableConvertor
    {
        // 有效类型
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
        public class TypeInfo
        {
            public TypeInfo(DataType type)
            {
                switch (type)
                {
                    case DataType.STRING:
                        {
                            m_TypeStr = "string";
                            m_DefaultValStr = "null";
//                            m_ReaderCodeTemplate = "values[(int)_ID.${ENUMID}];\n\t\t\t";
                            m_ReaderCodeTemplate = "values[${VALUE_IDX}];\n\t\t\t";
                        }
                        break;
                    case DataType.INT:
                        {
                            m_TypeStr = "Int32";
                            m_DefaultValStr = "-1";
//                            m_ReaderCodeTemplate = "Convert.ToInt32(values[(int)_ID.${ENUMID}]);\n\t\t\t";
                            m_ReaderCodeTemplate = "BasicUtil.EncryptInt32Value(Convert.ToInt32(values[${VALUE_IDX}]));\n\t\t\t";
                        }
                        break;
                    case DataType.FLOAT:
                        {
                            m_TypeStr = "Single";
                            m_DefaultValStr = "0";
                            //m_ReaderCodeTemplate = "Convert.ToSingle(values[(int)_ID.${ENUMID}]);\n\t\t\t";
                            m_ReaderCodeTemplate = "BasicUtil.EncryptSingleValue(Convert.ToSingle(values[${VALUE_IDX}]));\n\t\t\t";
                        }
                        break;
                    case DataType.SHORT:
                        {
                            m_TypeStr = "Int16";
                            m_DefaultValStr = "-1";
//                            m_ReaderCodeTemplate = "Convert.ToInt16(values[(int)_ID.${ENUMID}]);\n\t\t\t";
                            m_ReaderCodeTemplate = "Convert.ToInt16(values[${VALUE_IDX}]);\n\t\t\t";
                        }
                        break;
                    case DataType.BOOL:
                        {
                            m_TypeStr = "Boolean";
                            m_DefaultValStr = "false";
//                            m_ReaderCodeTemplate = "Convert.ToBoolean(values[(int)_ID.${ENUMID}]);\n\t\t\t";
                            m_ReaderCodeTemplate = "Convert.ToBoolean(values[${VALUE_IDX}]);\n\t\t\t";
                        }
                        break;
                    case DataType.BYTE:
                        {
                            m_TypeStr = "Byte";
                            m_DefaultValStr = "0";
//                            m_ReaderCodeTemplate = "Convert.ToByte(values[(int)_ID.${ENUMID}]);\n\t\t\t";
                            m_ReaderCodeTemplate = "Convert.ToByte(values[${VALUE_IDX}]);\n\t\t\t";
                        }
                        break;
                }
                m_DataType = type;
            }
            public DataType m_DataType;
            public string m_TypeStr;                // 该类型字符串
            public string m_DefaultValStr;          // 该类型默认值字符串
            public string m_ReaderCodeTemplate;     // 对应数据读取代码模板
        }

        public enum VariableType
        {
            Single,         // 单独变量
            Repeat          // 数组变量
        }
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

        public Dictionary<DataType, TypeInfo> m_TypeInfoDict;   // 类型信息
        public Dictionary<string, DataType> m_TypeDict;
        public TableInfo m_TabInfo;
        IWorkbook m_WorkBook;
        ISheet m_Sheet;
        IRow m_NameRow;
        IRow m_TypeRow;

        List<string> m_Names = new List<string>();
        List<DataType> m_Types = new List<DataType>();
        List<VariableInfo> m_Variables = new List<VariableInfo>();
        List<string> m_EndNumList = new List<string>();  // 结尾数字 "0" "1" "2" ...
        // 列起始索引和结束索引；行起始索引和结束索引
        public int m_FirstColumIdx, m_LastColumIdx, m_FirstRowIdx, m_LastRowIdx;

        public string m_ScriptExportPath;       // 脚本导出目录
        public string m_TableExportPath;        // 表格数据导出目录
        public int m_SkipColumn = 1;            // 需要跳过的列【这一列用于策划填写一般注释】

        public class NodeInfo
        {
            public string m_Name;
            public string m_Content;
        }
        public enum HandleType
        {
            DirectWrite,

        }
        public enum TemplateType
        {
            DataTable,      // DataTable.cs
            SingleTable,    // 每个表格对应的脚本
        }
        public List<NodeInfo> m_TableNodes;     // 数据表脚本模板节点信息
        public Dictionary<string, NodeInfo> m_TableNodeDict;

        public List<NodeInfo> m_TableMngNodes;   // 数据管理类脚本模板节点信息
        public Dictionary<string, NodeInfo> m_TableMngNodeDict;
        Dictionary<string, string> m_ReplaceKeyDict;
        string m_LocalFileName;
        string m_TabWillDeseriaAllFile;
        Dictionary<string, string> m_LocalDict;     // 本地化字典，用于替换表格中的数据
        HashSet<string> m_TabsWillDeseriaAll;

        public DataTableConvertor()
        {
            m_TypeInfoDict = new Dictionary<DataType, TypeInfo>();
            m_TypeInfoDict.Add(DataType.STRING, new TypeInfo(DataType.STRING));
            m_TypeInfoDict.Add(DataType.INT, new TypeInfo(DataType.INT));
            m_TypeInfoDict.Add(DataType.FLOAT, new TypeInfo(DataType.FLOAT));
            m_TypeInfoDict.Add(DataType.SHORT, new TypeInfo(DataType.SHORT));
            m_TypeInfoDict.Add(DataType.BOOL, new TypeInfo(DataType.BOOL));
            m_TypeInfoDict.Add(DataType.BYTE, new TypeInfo(DataType.BYTE));

            m_TypeDict = new Dictionary<string, DataType>();
            m_TypeDict.Add(DataType.STRING.ToString(), DataType.STRING);
            m_TypeDict.Add(DataType.INT.ToString(), DataType.INT);
            m_TypeDict.Add(DataType.FLOAT.ToString(), DataType.FLOAT);
            m_TypeDict.Add(DataType.SHORT.ToString(), DataType.SHORT);
            m_TypeDict.Add(DataType.BOOL.ToString(), DataType.BOOL);
            m_TypeDict.Add(DataType.BYTE.ToString(), DataType.BYTE);

            m_ReplaceKeyDict = new Dictionary<string, string>();
            m_ReplaceKeyDict.Add("${N}", "\n");
            m_ReplaceKeyDict.Add("${T}", "\t");
            m_ReplaceKeyDict.Add("${L}", "<");
            m_ReplaceKeyDict.Add("${R}", ">");
            m_ReplaceKeyDict.Add("${C}", "&");
            //m_ReplaceKeyDict.Add("_", "");

            m_LocalFileName = "/策划资源/Configs/GameTables/Localization.txt";
            m_TabWillDeseriaAllFile = "/ExportDataByExcel/Editor/DataTable/TabWillDeseriaAll.txt";
            m_LocalDict = new Dictionary<string, string>();

            m_EndNumList.Clear();
            string endNum = "";
            for (int idx = 35; idx >= 0; --idx)
            {
                if (idx >= 10)
                    m_EndNumList.Add(idx.ToString());
                else
                {
                    endNum = "0" + idx.ToString();
                    m_EndNumList.Add(endNum);
                    m_EndNumList.Add(idx.ToString());
                }
            }
            m_ScriptExportPath = "/ExportDataByExcel/Scripts/Core/Configs/DataTable/";
            m_TableExportPath = "/Resources/DataTable/";
            ParseScriptTemplate(TemplateType.DataTable);
            ParseScriptTemplate(TemplateType.SingleTable);
            ParseLocalizationFile();
            ParseDeseriaAllTabFile();
        }

        void ParseScriptTemplate(TemplateType type)
        {
            string templateName = "";
            if (type == TemplateType.DataTable)
            {
                m_TableMngNodes = new List<NodeInfo>();
                m_TableMngNodeDict = new Dictionary<string, NodeInfo>();
                templateName = Application.dataPath + "/ExportDataByExcel/Editor/DataTable/CSharpTableManagerTemplate.xml";
            }
            else if (type == TemplateType.SingleTable)
            {
                m_TableNodes = new List<NodeInfo>();
                m_TableNodeDict = new Dictionary<string, NodeInfo>();
                templateName = Application.dataPath + "/ExportDataByExcel/Editor/DataTable/CSharpCodeTemplate.xml";
            }
            FileStream fs = File.Open(templateName, FileMode.Open);
            if (fs != null)
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                string content = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);
                XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
                int nodeCnt = nodeList.Count;
                XmlElement elem = null;
                for (int idx = 0; idx < nodeCnt; ++idx)
                {
                    elem = nodeList[idx] as XmlElement;
                    if (elem != null)
                    {
                        NodeInfo nodeInfo = new NodeInfo();
                        nodeInfo.m_Name = elem.Name;
                        nodeInfo.m_Content = elem.InnerText;
                        // 先清除模板中的制表符
                        nodeInfo.m_Content = nodeInfo.m_Content.Replace("\t", "");
                        foreach (KeyValuePair<string, string> pair in m_ReplaceKeyDict)
                        {
                            nodeInfo.m_Content = nodeInfo.m_Content.Replace(pair.Key, pair.Value);
                        }
                        if (type == TemplateType.DataTable)
                        {
                            m_TableMngNodes.Add(nodeInfo);
                            m_TableMngNodeDict.Add(nodeInfo.m_Name, nodeInfo);
                        }
                        else
                        {
                            m_TableNodes.Add(nodeInfo);
                            m_TableNodeDict.Add(nodeInfo.m_Name, nodeInfo);
                        }
                    }
                }
                fs.Close();
            }
            else
            {
                Debug.LogError("The file named '" + templateName + "' doesn't exist!");
            }
        }
        // 读取解析本地化文件（用于替换表格中的数据：${...}）
        void ParseLocalizationFile()
        {
            if (File.Exists(Application.dataPath + m_LocalFileName))
            {
                StreamReader reader = File.OpenText(Application.dataPath + m_LocalFileName);
                m_LocalDict = StringUtils.ReadDictionary(reader.ReadToEnd());
                reader.Close();
            }
        }
        // 读取需要全部反序列化的表信息
        void ParseDeseriaAllTabFile()
        {
            m_TabsWillDeseriaAll = new HashSet<string>();
            StreamReader reader = File.OpenText(Application.dataPath + m_TabWillDeseriaAllFile);
            string tabName = "";
            while (reader.Peek() >= 0)
            {
                tabName = reader.ReadLine();
                m_TabsWillDeseriaAll.Add(tabName);
            }
            reader.Close();
        }
        public bool Convert(TableInfo info)
        {
            Debug.Log(">>转换表格：【" + info.m_RealName + "】开始！！！" + info.m_FullName);
            m_TabInfo = info;
            m_WorkBook = ExcelFileReader.ReadExcelFile(m_TabInfo.m_FullName, "");
            if (m_WorkBook == null)
            {
                string infoStr = "表格：【" + m_TabInfo.m_RealName + "】无法打开. 请检查该表是否被其他程序占用！！！";
                Debug.LogError(infoStr);
                if (EditorUtility.DisplayDialog("提示", infoStr, "确定", "停止"))
                    return true;
                else
                    return false;
            }
            //return true;
            m_Sheet = m_WorkBook.GetSheetAt(0);
            if (m_Sheet == null)
            {
                string infoStr = "表格：【" + m_TabInfo.m_RealName + "】数据为空. 请检查内部是否有数据！！！\n";
                Debug.LogError(infoStr);
                if (EditorUtility.DisplayDialog("提示", infoStr, "确定", "停止"))
                    return true;
                else
                    return false;
            }
            m_FirstRowIdx = m_Sheet.FirstRowNum;
            m_LastRowIdx = m_Sheet.LastRowNum;
            m_NameRow = m_Sheet.GetRow(0);
            m_TypeRow = m_Sheet.GetRow(1);
            m_FirstColumIdx = m_NameRow.FirstCellNum;
            m_LastColumIdx = m_NameRow.LastCellNum;

            // 确定有效行索引和列索引
            IRow rowData = null;
            for (int row = 3; row <= m_LastRowIdx; ++row)
            {
                rowData = m_Sheet.GetRow(row);
                // 遇到空行则跳过
                if (rowData == null)
                {
                    m_LastRowIdx = row - 1;
                    break;
                }
                // 如果该行无数据则跳过
                if (rowData.GetCell(0) == null)
                {
                    m_LastRowIdx = row - 1;
                    break;
                }
                if (string.IsNullOrEmpty(rowData.GetCell(0).ToString()))
                {
                    m_LastRowIdx = row - 1;
                    break;
                }
            }
            ICell cellData = null;
            for (int cell = m_FirstColumIdx; cell <= m_LastColumIdx; ++cell)
            {
                cellData = m_NameRow.GetCell(cell);
                if (cellData == null)
                {
                    m_LastColumIdx = cell - 1;
                    break;
                }
                string varName = cellData.ToString();
                if (string.IsNullOrEmpty(varName))
                {
                    m_LastColumIdx = cell - 1;
                    break;
                }
            }

            // 填充数据
            m_Names.Clear();
            m_Types.Clear();
            HashSet<string> nameSet = new HashSet<string>();
            string dataStr = "";
            for (int cell = m_FirstColumIdx; cell <= m_LastColumIdx; ++cell)
            {
                cellData = m_NameRow.GetCell(cell);
                string varName = cellData.ToString();
                if (string.IsNullOrEmpty(varName))
                    break;
                if (nameSet.Contains(varName))
                {
                    string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + cell + "列数据名称重复和前面的重复：【" + varName + "】. 请检查！！！\n注意，行列索引从0开始！！！";
                    Debug.LogError(infoStr);
                    EditorUtility.DisplayDialog("提示", infoStr, "确定");
                    return false;
                }
                else
                {
                    if (cell == m_SkipColumn)
                    {
                        varName += "_Skipped";
                    }
                    varName = HandleName(varName);
                    nameSet.Add(varName);
                    m_Names.Add(varName);
                }

                // 处理类型
                cellData = m_TypeRow.GetCell(cell);
                dataStr = cellData.ToString();
                // 去掉可能的空格
                dataStr = dataStr.Replace(" ", "");
                // 全部转为大写
                dataStr = dataStr.ToUpper();
                DataType type = GetValidType(dataStr);
                if (type == DataType.NONE)
                {
                    string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + cell + "列数据类型不对：【" + cellData.ToString() + "】. 请检查！！！\n注意，行列索引从0开始！！！";
                    Debug.LogError(infoStr);
                    EditorUtility.DisplayDialog("提示", infoStr, "确定");
                    return false;
                }
                m_Types.Add(type);
            }
            // 导出数据
            if (!ExportTXT())
                return false;
            // 导出脚本
            if (!ExportScript())
                return false;
            Debug.Log(">>转换表格：【" + info.m_RealName + "】完成！！！");
            return true;
        }
        // 导出DataTable.cs
        public bool ExportDataTableScript(List<TableInfo> allTables)
        {
            string exportPath = Application.dataPath + m_ScriptExportPath;
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }
            FileStream file = File.Create(exportPath + "DataTable.cs");
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
            int nodeCnt = m_TableMngNodes.Count;
            NodeInfo nodeInfo = null;
            string fullInit = "";
            for (int nodeIdx = 0; nodeIdx < nodeCnt; ++nodeIdx)
            {
                nodeInfo = m_TableMngNodes[nodeIdx];
                if (nodeInfo.m_Name.Equals("descript"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("import"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("namespace"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("classhead") || nodeInfo.m_Name.Equals("tail"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("managerdata") || nodeInfo.m_Name.Equals("manageropt"))
                {
                    int cnt = allTables.Count;
                    TableInfo info = null;
                    string content = "";
                    for (int idx = 0; idx < cnt; ++idx)
                    {
                        info = allTables[idx];
                        content = nodeInfo.m_Content.Replace("${CodeName}", info.m_ExportClassName);
                        if (m_TabsWillDeseriaAll.Contains(info.m_RealName))
                        {
                            content = content.Replace("${IsDeseriaAll}", "true");
                        }
                        else
                        {
                            content = content.Replace("${IsDeseriaAll}", "false");
                        }
                        writer.Write(content);
                    }
                }
                else if (nodeInfo.m_Name.Equals("initsingle"))
                {
                    int cnt = allTables.Count;
                    TableInfo info = null;
                    fullInit = "";
                    string content = "";
                    for (int idx = 0; idx < cnt; ++idx)
                    {
                        info = allTables[idx];
                        content = nodeInfo.m_Content.Replace("${CodeName}", info.m_ExportClassName);
                        if (m_TabsWillDeseriaAll.Contains(info.m_RealName))
                        {
                            content = content.Replace("${IsDeseriaAll}", "true");
                        }
                        else
                        {
                            content = content.Replace("${IsDeseriaAll}", "false");
                        }
                        fullInit += content;
                    }
                }
                else if (nodeInfo.m_Name.Equals("inittable"))
                {
                    string content = nodeInfo.m_Content.Replace("${FULLINIT}", fullInit);
                    writer.Write(content);
                }
            }

            writer.Flush();
            writer.Close();
            //         file.Flush();
            //         file.Close();
            return true;
        }

        // 导出每个表格所对应的脚本
        bool ExportScript()
        {
            // 生成变量信息
            if (!GenerateVariableInfo())
                return false;

            // 导出脚本
            string exportPath = Application.dataPath + m_ScriptExportPath + "Tables/";
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }
            string exportFileName = m_TabInfo.m_RealName;
            string exportFileNameWithExt = exportFileName;// + ".txt";
            FileStream file = File.Create(exportPath + "Table_" + m_TabInfo.m_ExportClassName + ".cs");
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
            int nodeCnt = m_TableNodes.Count;
            NodeInfo nodeInfo = null;
            string fullReader = "";
            for (int nodeIdx = 0; nodeIdx < nodeCnt; ++nodeIdx)
            {
                nodeInfo = m_TableNodes[nodeIdx];
                if (nodeInfo.m_Name.Equals("descript"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("import"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("namespace"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
                else if (nodeInfo.m_Name.Equals("classhead"))
                {
                    string content = nodeInfo.m_Content.Replace("${CodeName}", m_TabInfo.m_ExportClassName);
                    content = content.Replace("${ValueNum}", (m_Names.Count - 2).ToString());       // -2：去掉Id列和注释列
                    content = content.Replace("${FileNameWithExt}", exportFileNameWithExt);
                    content = content.Replace("${FileName}", exportFileName);
                    string fullEnum = "INVLAID_INDEX = -1,\n";
                    int cnt = m_Names.Count;
                    for (int idx = 0; idx < cnt; ++idx)
                    {
                        if (idx == m_SkipColumn)
                            continue;
                        fullEnum += "\t\t\tID_" + m_Names[idx].ToUpper() + ",\n";
                    }
                    fullEnum += "\t\t\tMAX_RECORD";
                    content = content.Replace("${FULLENUM}", fullEnum);
                    writer.Write(content);
                }
                else if (nodeInfo.m_Name.Equals("variables"))
                {
                    string singleContent = m_TableNodeDict["single"].m_Content;
                    string repeatContent = m_TableNodeDict["repeat"].m_Content;
                    int cnt = m_Variables.Count;
                    VariableInfo info = null;
                    for (int idx = 0; idx < cnt; ++idx)
                    {
                        info = m_Variables[idx]; 
                        TypeInfo typeInfo = m_TypeInfoDict[info.m_DataType];
                        if (!info.m_IsKey && (typeInfo.m_DataType == DataType.INT || typeInfo.m_DataType == DataType.FLOAT))
                        {
                            singleContent = m_TableNodeDict["singleEncrypt"].m_Content;
                            repeatContent = m_TableNodeDict["repeatEncrypt"].m_Content;
                        }
                        else
                        {
                            singleContent = m_TableNodeDict["single"].m_Content;
                            repeatContent = m_TableNodeDict["repeat"].m_Content;
                        }
                        if (info.m_Type == VariableType.Single)
                        {
                            string content = singleContent.Replace("${type}", typeInfo.m_TypeStr);
                            content = content.Replace("${Variable}", info.m_Name);
                            writer.Write(content);
                        }
                        else if (info.m_Type == VariableType.Repeat)
                        {
                            if (info.m_Count == 1)
                            {
                                string content = singleContent.Replace("${type}", typeInfo.m_TypeStr);
                                content = content.Replace("${Variable}", info.m_BakName);
                                writer.Write(content);
                            }
                            else
                            {
                                string content = repeatContent.Replace("${type}", typeInfo.m_TypeStr);
                                content = content.Replace("${Variable}", info.m_Name);
                                content = content.Replace("${COUNT}", info.m_Count.ToString());
                                content = content.Replace("${defaultvalue}", typeInfo.m_DefaultValStr);
                                writer.Write(content);
                            }

                        }

                        fullReader += GenerateVariableReaderCode("tabData", info);
                    }
                }
                else if (nodeInfo.m_Name.Equals("inittable"))
                {
                    string content = nodeInfo.m_Content.Replace("${CodeName}", m_TabInfo.m_ExportClassName);
                    content = content.Replace("${FULLREADER}", fullReader);
                    writer.Write(content);
                }
                else if (nodeInfo.m_Name.Equals("tail"))
                {
                    writer.Write(nodeInfo.m_Content);
                }
            }

            writer.Flush();
            writer.Close();

            return true;
        }
        public static string HandleName(string name)
        {
            // 首字母大写
            string tmpName = (new string(name[0], 1)).ToUpper() + name.Substring(1);
            // 下划线后面首字母大写
            List<int> idxList = new List<int>();    // 存储下划线索引
            int length = tmpName.Length;
            for (int idx = 0; idx < length; ++idx)
            {
                char c = tmpName[idx];
                if (c == '_')
                {
                    idxList.Add(idx);
                }
            }
            length = idxList.Count;
            string handleStr = "";
            for (int idx = 0; idx < length; ++idx)
            {
                int charIdx = idxList[idx] + 1;
                if (charIdx < tmpName.Length)
                {
                    handleStr = new string(tmpName[charIdx], 1);
                    tmpName = tmpName.Substring(0, charIdx) + handleStr.ToUpper() + tmpName.Substring(charIdx + 1);
                }
            }

            // 去掉下划线
            tmpName = tmpName.Replace("_", "");
            return tmpName;
        }
        bool GenerateVariableInfo()
        {
            Dictionary<string, int> variableIdxDict = new Dictionary<string, int>();
            int cnt = m_Names.Count;
            string realName = "";
            int index = -1;
            m_Variables.Clear();
            for (int idx = 0; idx < cnt; ++idx)
            {
                if (idx == m_SkipColumn)
                    continue;
                string varName = m_Names[idx];
                if (IsEndWithNumber(varName, out index, out realName))
                {
                    if (variableIdxDict.ContainsKey(realName))
                    {
                        int varIdx = variableIdxDict[realName];
                        VariableInfo info = m_Variables[varIdx];
                        DataType dataType = m_Types[idx];
                        if (info.m_DataType != dataType)
                        {
                            string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + idx + "列数据类型和第" + varIdx + "列数据类型不一致. 请检查！！！\n注意，行列索引从0开始！！！";
                            Debug.LogError(infoStr);
                            EditorUtility.DisplayDialog("提示", infoStr, "确定");
                            return false;
                        }
                        info.m_EnumStr.Add("ID_" + varName.ToUpper());
                        info.m_ValueId.Add(idx);
                        info.m_Count++;
                    }
                    else
                    {
                        VariableInfo info = new VariableInfo();
                        info.m_Type = VariableType.Repeat;
                        info.m_DataType = m_Types[idx];
                        info.m_Name = realName;
                        info.m_BakName = varName;
                        info.m_Count = 1;
                        info.m_EnumStr = new List<string>();
                        info.m_EnumStr.Add("ID_" + varName.ToUpper());
                        info.m_ValueId = new List<int>();
                        info.m_ValueId.Add(idx);
                        m_Variables.Add(info);
                        variableIdxDict.Add(realName, m_Variables.Count - 1);
                    }
                }
                else
                {
                    if (variableIdxDict.ContainsKey(realName))
                    {
                        string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + idx + "列数据名称和前面重复：【" + varName + "】-> 转换后名字【" + realName + "】. 请检查！！！\n注意，行列索引从0开始！！！";
                        Debug.LogError(infoStr);
                        EditorUtility.DisplayDialog("提示", infoStr, "确定");
                        return false;
                    }
                    else
                    {
                        VariableInfo info = new VariableInfo();
                        info.m_Type = VariableType.Single;
                        info.m_DataType = m_Types[idx];
                        info.m_Name = realName;
                        info.m_Count = 0;
                        info.m_EnumStr = new List<string>();
                        info.m_EnumStr.Add("ID_" + varName.ToUpper());
                        info.m_ValueId = new List<int>();
                        info.m_ValueId.Add(idx);
                        if (idx==0)
                        {
                            info.m_IsKey = true;
                        }
                        m_Variables.Add(info);
                        variableIdxDict.Add(realName, m_Variables.Count - 1);
                    }
                }
            }
            m_Variables.Sort((VariableInfo a, VariableInfo b) =>
            {
                return a.m_Name.CompareTo(b.m_Name);
            });
            return true;
        }
        // 生成变量读取代码 instName 代码中实例对象的名字（必须和模板中的名字一致）
        string GenerateVariableReaderCode(string instName, VariableInfo info)
        {
            string ret = instName;
            TypeInfo typeInfo = m_TypeInfoDict[info.m_DataType];
            if (info.m_Type == VariableType.Single)
            {
                if (info.m_ValueId[0] == 0)   // 如果是第0列（Id）
                    ret += ".m_" + info.m_Name + " = nKey;\n\t\t\t";
                else
                    ret += ".m_" + info.m_Name + " = " + typeInfo.m_ReaderCodeTemplate.Replace(/*"${ENUMID}"*/"${VALUE_IDX}", /*info.m_EnumStr[0]*/(info.m_ValueId[0] - 2).ToString());   // 值是从第2列开始的（第0列是id，第1列是注释），所以减2
            }
            else if (info.m_Type == VariableType.Repeat)
            {
                if (info.m_Count == 1)
                {
                    ret += ".m_" + info.m_BakName + " = " + typeInfo.m_ReaderCodeTemplate.Replace(/*"${ENUMID}"*/"${VALUE_IDX}", /*info.m_EnumStr[0]*/(info.m_ValueId[0] - 2).ToString());
                }
                else
                {
                    int cnt = info.m_EnumStr.Count;
                    for (int idx = 0; idx < cnt; ++idx)
                    {
                        if (idx == cnt - 1)
                            ret += ".m_" + info.m_Name + "[" + idx + "]" + " = " + typeInfo.m_ReaderCodeTemplate.Replace(/*"${ENUMID}"*/"${VALUE_IDX}", /*info.m_EnumStr[idx]*/(info.m_ValueId[idx] - 2).ToString()) + "\n\t\t\t";
                        else
                            ret += ".m_" + info.m_Name + "[" + idx + "]" + " = " + typeInfo.m_ReaderCodeTemplate.Replace(/*"${ENUMID}"*/"${VALUE_IDX}", /*info.m_EnumStr[idx]*/(info.m_ValueId[idx] - 2).ToString()) + "\n\t\t\t" + instName;
                    }
                }
            }
            return ret;
        }

        bool IsEndWithNumber(string valueStr, out int index, out string variableName)
        {
            int cnt = m_EndNumList.Count;
            for (int idx = 0; idx < cnt; ++idx)
            {
                if (valueStr.EndsWith(m_EndNumList[idx]))
                {
                    index = idx;
                    int zeroIdx = valueStr.IndexOf('0');
                    if (zeroIdx != -1)
                    {
                        if (zeroIdx + 1 == valueStr.LastIndexOf(m_EndNumList[idx]))
                        {
                            variableName = valueStr.Substring(0, zeroIdx);
                            return true;
                        }
                    }
                    variableName = valueStr.Substring(0, valueStr.LastIndexOf(m_EndNumList[idx]));
                    return true;
                }
            }
            index = -1;
            variableName = valueStr;
            return false;
        }
        // 导出每个表格数据为txt格式
        bool ExportTXT()
        {
            StreamWriter file = File.CreateText(Application.dataPath + m_TableExportPath + m_TabInfo.m_RealName + ".txt");
            string dataStr = "";
            Dictionary<string, int> idDict = new Dictionary<string, int>();
            for (int rowIdx = 3; rowIdx <= m_LastRowIdx; ++rowIdx)
            {
                IRow rowData = m_Sheet.GetRow(rowIdx);
                if (rowData == null)
                {
                    string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + rowIdx + "行数据为空. 请检查！！！\n注意，行列索引从0开始！！！";
                    Debug.LogError(infoStr);
                    EditorUtility.DisplayDialog("提示", infoStr, "确定");
                    continue;
                }
                if (rowData.GetCell(0) == null)
                {
                    string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + rowIdx + "行第0列数据为空. 请检查！！！\n注意，行列索引从0开始！！！";
                    Debug.LogError(infoStr);
                    EditorUtility.DisplayDialog("提示", infoStr, "确定");
                    continue;
                }

                if (rowData.GetCell(0).ToString().StartsWith("#"))
                    continue;

                // 检查Id是否重复
                dataStr = GetCellData(rowData, rowIdx, m_FirstColumIdx);
                if (idDict.ContainsKey(dataStr))
                {
                    string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + rowIdx + "行Id和第" + idDict[dataStr] + "行Id重复. 请检查！！！\n注意，行列索引从0开始！！！";
                    Debug.LogError(infoStr);
                    EditorUtility.DisplayDialog("提示", infoStr, "确定");
                    continue;
                }
                else
                {
                    idDict.Add(dataStr, rowIdx);
                }

                for (int cellIdx = m_FirstColumIdx; cellIdx <= m_LastColumIdx; ++cellIdx)
                {
                    if (cellIdx == m_SkipColumn)
                        continue;
                    dataStr = GetCellData(rowData, rowIdx, cellIdx);
                    if (!CheckDataType(rowIdx, cellIdx, dataStr))
                    {
                        file.Flush();
                        file.Close();
                        return false;
                    }
                    file.Write(dataStr);
                    if (cellIdx != m_LastColumIdx)
                    {
                        file.Write('\t');
                    }
                }
                if (rowIdx != m_LastRowIdx)
                    file.Write('\n');
            }
            file.Flush();
            file.Close();
            return true;
        }

        string GetCellData(IRow rowData, int rowIdx, int cellIdx)
        {
            string dataStr = "";
            ICell cellData = rowData.GetCell(cellIdx);
            if (cellData == null)
            {
                string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + rowIdx + "行第" + cellIdx + "列数据为空. 请检查！！！\n注意，行列索引从0开始！！！";
                Debug.LogError(infoStr);
                EditorUtility.DisplayDialog("提示", infoStr, "确定");
            }
            if (cellData.CellType == CellType.Formula)
            {
                if (cellData.CachedFormulaResultType == CellType.Numeric)
                {
                    dataStr = cellData.NumericCellValue.ToString();
                }
                else if (cellData.CachedFormulaResultType == CellType.String)
                {
                    dataStr = cellData.StringCellValue;
                }
            }
            else
            {
                dataStr = cellData.ToString();
            }
            if (dataStr.StartsWith("${") && dataStr.EndsWith("}"))
            {
                dataStr = dataStr.Substring(dataStr.IndexOf("{") + 1, dataStr.Length - 3);
                if (m_LocalDict.ContainsKey(dataStr))
                {
                    dataStr = m_LocalDict[dataStr];
                }
                else
                {
                    string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + rowIdx + "行第" + cellIdx + "列数据没有在本地化字典中. 请检查字典文件:\n" + Application.dataPath + m_LocalFileName + "！！！\n注意，行列索引从0开始！！！";
                    Debug.LogError(infoStr);
                    EditorUtility.DisplayDialog("提示", infoStr, "确定");
                }
            }
            return dataStr;
        }
        DataType GetValidType(string typeStr)
        {
            if (m_TypeDict.ContainsKey(typeStr))
            {
                return m_TypeDict[typeStr];
            }
            return DataType.NONE;
        }
        // 检查数据类型
        bool CheckDataType(int row, int col, string dataStr)
        {
            bool ret = true;
            if (col < m_Types.Count)
            {
                DataType type = m_Types[col];
                switch (type)
                {
                    case DataType.STRING:
                        {
                            if (string.IsNullOrEmpty(dataStr))
                                ret = false;
                        }
                        break;
                    case DataType.INT:
                        {
                            int value = 0;
                            if (!int.TryParse(dataStr, out value))
                                ret = false;
                        }
                        break;
                    case DataType.FLOAT:
                        {
                            float value = 0;
                            if (!float.TryParse(dataStr, out value))
                                ret = false;
                        }
                        break;
                    case DataType.SHORT:
                        {
                            short value = 0;
                            if (!short.TryParse(dataStr, out value))
                                ret = false;
                        }
                        break;
                    case DataType.BOOL:
                        {
                            bool value = false;
                            if (!bool.TryParse(dataStr, out value))
                                ret = false;
                        }
                        break;
                    case DataType.BYTE:
                        {
                            byte value = 0;
                            if (!byte.TryParse(dataStr, out value))
                                ret = false;
                        }
                        break;
                }
            }
            if (!ret)
            {
                string infoStr = "表格：【" + m_TabInfo.m_RealName + "】第" + row + "行第" + col + "列数据无效：【" + dataStr + "】. 请检查！！！\n注意，行列索引从0开始！！！";
                Debug.LogError(infoStr);
                EditorUtility.DisplayDialog("提示", infoStr, "确定");
            }
            return ret;
        }
    }
}