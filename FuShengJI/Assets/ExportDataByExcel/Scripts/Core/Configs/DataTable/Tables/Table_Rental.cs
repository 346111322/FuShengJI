// This code created by tools automatically, you would not modify it.
using System;
using System.Collections.Generic;
using System.Collections;
using BH.Core.Utils;
  
namespace BH.Core.Configs
{
    	[Serializable]
    	public  class Tab_Rental : ITableOperate
	{

    		private const string TAB_FILE_DATA = "DataTable/Rental";

    		private const int VALUE_NUM_PER_ROW = 3;// not contains id
    		public static bool IsTableLoaded = false;
    		public enum _ID
    		{
    			INVLAID_INDEX = -1,
			ID_ID,
			ID_INTRODUCE,
			ID_SIZE,
			ID_PRICEMIN,
			MAX_RECORD
    		}
    		public static string GetInstanceFile(){return TAB_FILE_DATA; }
  
		private  Int32  m_ID;
		public   Int32 ID { get{ return m_ID;}}
  
		private  string  m_Introduce;
		public   string Introduce { get{ return m_Introduce;}}
  
		private  Int32  m_PriceMin;
		public   Int32 PriceMin { get{ return BasicUtil.DecryptInt32Value(m_PriceMin);}}
  
		private  Int32  m_Size;
		public   Int32 Size { get{ return BasicUtil.DecryptInt32Value(m_Size);}}
  
		public  static bool LoadTable(Hashtable _tab, DataTableStructure _tabStructure, bool _isDeseriaAll)
		{

			if(!DataTable.ReaderPList(GetInstanceFile(), VALUE_NUM_PER_ROW, SerializableTable, _tab, _tabStructure, _isDeseriaAll))

			{
				throw  TableException.ErrorReader("Load File{0} Fail!!!",GetInstanceFile());
			}
			IsTableLoaded = true;
			return true;
		}

		public static Tab_Rental LoadTableItem(int nIdx, Hashtable _tab, DataTableStructure _tabStructure)
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
			Tab_Rental tabData = new Tab_Rental();
			tabData.m_ID = nKey;
			tabData.m_Introduce = values[0];
			tabData.m_PriceMin = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			tabData.m_Size = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[1]));
			
			_hash.Add(nKey,tabData);
		}

		public static Tab_Rental SerializableTableItem(string[] values,int nKey,Hashtable _hash)
		{
			if (values == null)
    			{
    				throw TableException.ErrorReader("values is null. Table: {0}  Key:{1}.", GetInstanceFile(), nKey);
    			}
			if (values.Length != VALUE_NUM_PER_ROW)
			{
				throw TableException.ErrorReader("Load {0}  error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(), VALUE_NUM_PER_ROW, values.Length);
			}
			Tab_Rental tabData = new Tab_Rental();
			tabData.m_ID = nKey;
			tabData.m_Introduce = values[0];
			tabData.m_PriceMin = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			tabData.m_Size = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[1]));
			
			_hash.Add(nKey,tabData);
			return tabData;
		}
  	}
}