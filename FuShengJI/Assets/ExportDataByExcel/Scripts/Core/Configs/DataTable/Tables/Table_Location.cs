// This code created by tools automatically, you would not modify it.
using System;
using System.Collections.Generic;
using System.Collections;
using BH.Core.Utils;
  
namespace BH.Core.Configs
{
    	[Serializable]
    	public  class Tab_Location : ITableOperate
	{

    		private const string TAB_FILE_DATA = "DataTable/Location";

    		private const int VALUE_NUM_PER_ROW = 2;// not contains id
    		public static bool IsTableLoaded = false;
    		public enum _ID
    		{
    			INVLAID_INDEX = -1,
			ID_ID,
			ID_CONTENT,
			ID_GOODNUMMAX,
			MAX_RECORD
    		}
    		public static string GetInstanceFile(){return TAB_FILE_DATA; }
  
		private  string  m_Content;
		public   string Content { get{ return m_Content;}}
  
		private  Int32  m_GoodNumMax;
		public   Int32 GoodNumMax { get{ return BasicUtil.DecryptInt32Value(m_GoodNumMax);}}
  
		private  Int32  m_ID;
		public   Int32 ID { get{ return m_ID;}}
  
		public  static bool LoadTable(Hashtable _tab, DataTableStructure _tabStructure, bool _isDeseriaAll)
		{

			if(!DataTable.ReaderPList(GetInstanceFile(), VALUE_NUM_PER_ROW, SerializableTable, _tab, _tabStructure, _isDeseriaAll))

			{
				throw  TableException.ErrorReader("Load File{0} Fail!!!",GetInstanceFile());
			}
			IsTableLoaded = true;
			return true;
		}

		public static Tab_Location LoadTableItem(int nIdx, Hashtable _tab, DataTableStructure _tabStructure)
		{
			string[] rows = _tabStructure.GetRow(nIdx);
			if (rows != null)
			{
				return SerializableTableItem(rows, nIdx, _tab);
			}
			return null;
		}

		public static void SerializableTable(string[] values,int nKey,Hashtable _hash)
		{
			if (values == null)
    			{
    				throw TableException.ErrorReader("values is null. Table: {0}  Key:{1}.", GetInstanceFile(), nKey);
    			}
			if (values.Length != VALUE_NUM_PER_ROW)
			{
				throw TableException.ErrorReader("Load {0}  error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(), VALUE_NUM_PER_ROW, values.Length);
			}
			Tab_Location tabData = new Tab_Location();
			tabData.m_Content = values[0];
			tabData.m_GoodNumMax = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[1]));
			tabData.m_ID = nKey;
			
			_hash.Add(nKey,tabData);
		}

		public static Tab_Location SerializableTableItem(string[] values,int nKey,Hashtable _hash)
		{
			if (values == null)
    			{
    				throw TableException.ErrorReader("values is null. Table: {0}  Key:{1}.", GetInstanceFile(), nKey);
    			}
			if (values.Length != VALUE_NUM_PER_ROW)
			{
				throw TableException.ErrorReader("Load {0}  error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(), VALUE_NUM_PER_ROW, values.Length);
			}
			Tab_Location tabData = new Tab_Location();
			tabData.m_Content = values[0];
			tabData.m_GoodNumMax = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[1]));
			tabData.m_ID = nKey;
			
			_hash.Add(nKey,tabData);
			return tabData;
		}
  	}
}