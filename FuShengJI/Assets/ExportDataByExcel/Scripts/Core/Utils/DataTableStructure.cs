using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DataTableStructure
{
#region data pool
    //数据缓冲
    public string[] pool;
    //内容索引
    public ushort[,] contents;
    //Id -> 行id
    public Dictionary<int, int> idIndices;// = new Dictionary<int, int>();
//#if UNITY_EDITOR
//    public int[] ids;
//#endif

#endregion
#region setter & getter
    public int RowCount;
//     {
//         get { return contents.GetLength(0);}
//     }
    public int ColumnCount;
//     {
//         get { return contents.GetLength(1); }
//     }
#endregion
#region Data Getter
    public string GetCell(int i, int j)
    {
        return pool[contents[i, j]];
    }

    public string[] GetRow(int rowKey)
    {
        if (idIndices != null && idIndices.ContainsKey(rowKey))
        {
            int rowIndex = idIndices[rowKey];
            string[] cells = new string[ColumnCount];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = GetCell(rowIndex, i);
            }
            return cells;
        }
        return null;
    }
    
#endregion

	public string ReadString(BinaryReader br){
        // 直接使用 br.ReadInt32();在真机上会出错（使用IL2CPP打版本，如果使用Mono打版本，没有问题）
        byte[] bytes = br.ReadBytes(4);
        int len = System.BitConverter.ToInt32(bytes, 0);
		string str = System.Text.Encoding.UTF8.GetString(br.ReadBytes(len));
		//Debug.Log(str);
		return str;
	}
	public void WriteString(BinaryWriter bw,string str){
		byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
		bw.Write((int)bytes.Length);
		bw.Write(bytes);
	}

#region Serialize & Deserialize
//#if UNITY_EDITOR
//    public void Serialize(string path)
//    {
//        // [int](pool length)[pools][ids]
//        using (FileStream fs = File.OpenWrite(path))
//        {
//            Serialize(fs);
//            fs.Close();
//        }
//    }
//
//    public void Serialize(Stream s)
//    {
//        BinaryWriter bw = new BinaryWriter(s);
//        //1 write pool length;
//        bw.Write((ushort)pool.Length);
//        foreach (var item in pool)
//        {
//            //bw.Write(item);
//			WriteString(bw,item);
//        }
//
//        //2 write lineCount,columnCount;
//        int lineCnt = contents.GetLength(0);
//        int colCnt = contents.GetLength(1);
//        bw.Write((ushort)lineCnt);
//        bw.Write((ushort)(colCnt-1));
//
//        for (int i = 0; i < lineCnt; i++)
//        {
//            // write id indices
//            bw.Write(ids[i]);
//            // 从1开始
//            for (int j = 1; j < colCnt; j++)
//            {
//                bw.Write(contents[i, j]);
//            }
//        }
//
//        bw.Close();
//    }
//#endif
    public void Deserialize(string path)
    {
        using (FileStream fs = File.OpenRead(path))
        {
            Deserialize(fs, null, null);
        }
    }

    public void Deserialize(Stream s, BH.Core.Configs.SerializableTable _fun, Hashtable _hash, bool isDeseriLoadAll = false)
    {
        BinaryReader br = new BinaryReader(s);
        // read pool length
        byte[] bytes = null;
        bytes = br.ReadBytes(2);
        int poolLen = System.BitConverter.ToUInt16(bytes, 0);// br.ReadUInt16();
        //Debug.Log(poolLen);
        int index = 0;
        pool = new string[poolLen];
        //Profiler.BeginSample("test");
        while (index < poolLen)
        {
            //1 read string
            pool[index++] = /*br.ReadString();*/ReadString(br);
        }
        //Profiler.EndSample();
        //2 read id indices and contents
        bytes = br.ReadBytes(2);
        RowCount = System.BitConverter.ToUInt16(bytes, 0); //br.ReadUInt16();
        bytes = br.ReadBytes(2);
        ColumnCount = System.BitConverter.ToUInt16(bytes, 0); //br.ReadUInt16();
        contents = new ushort[RowCount, ColumnCount];
//#if UNITY_EDITOR
//        ids = new int[RowCount];
//#endif
        int idx = 0;
        string[] contentStr = null;
        if (isDeseriLoadAll)
        {
            contentStr = new string[ColumnCount];
        }
        idIndices = new Dictionary<int, int>(RowCount);
        for (int i = 0; i < RowCount; i++)
        {
            // read id indices
            bytes = br.ReadBytes(4);
            idx = System.BitConverter.ToInt32(bytes, 0); //br.ReadInt32();
            idIndices.Add(idx, i);
//#if UNITY_EDITOR
//            ids[i] = idx;
//#endif
            for (int j = 0; j < ColumnCount; j++)
            {
                bytes = br.ReadBytes(2);
                contents[i, j] = System.BitConverter.ToUInt16(bytes, 0); //br.ReadUInt16();
                if (isDeseriLoadAll)
                {
                    contentStr[j] = pool[contents[i, j]];
                }
            }
            if (isDeseriLoadAll && _fun != null && _hash != null)
                _fun(contentStr, idx, _hash);
        }
        br.Close();
    }

    public void Deserialize(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            Deserialize(ms, null, null);
        }
    }
//#if UNITY_EDITOR
//    public void GenerateTableFile(string fullPath)
//    {
//        //==================================================
//        // 1.UTF8有BOM
////         FileStream file = File.Create(fullPath);
////         StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
//        // 2.UTF8无BOM
//        StreamWriter writer = File.CreateText(fullPath);
//        //==================================================
//        int cnt = ids.Length;
//        string valueStr;
//        for (int idx = 0; idx < cnt; ++idx)
//        {
//            int id = ids[idx];
//            writer.Write(id);
//            writer.Write("\t");
//            string[] values = GetRow(id);
//            int length = values.Length;
//            for (int vIdx = 0; vIdx < length; ++vIdx)
//            {
//                valueStr = values[vIdx];
//                writer.Write(valueStr);
//                if (vIdx != length-1)
//                    writer.Write("\t");
//            }
//            if (idx != cnt-1)
//                writer.Write("\n");
//        }
//        writer.Flush();
//        writer.Close();
//    }
//#endif
#endregion
}
