// This code created by tools automatically, you would not modify it.
using System;
using System.Collections.Generic;
using System.Collections;
using BH.Core.Utils;
  
namespace BH.Core.Configs
{
    	[Serializable]
    	public  class Tab_Postoffice : ITableOperate
	{

    		private const string TAB_FILE_DATA = "DataTable/Postoffice";

    		private const int VALUE_NUM_PER_ROW = 4;// not contains id
    		public static bool IsTableLoaded = false;
    		public enum _ID
    		{
    			INVLAID_INDEX = -1,
			ID_ID,
			ID_INTRODUCE,
			ID_MONEYMIN,
			ID_MONEYMAX,
			ID_ISDEBT,
			MAX_RECORD
    		}
    		public static string GetInstanceFile(){return TAB_FILE_DATA; }
  
		private  Int32  m_ID;
		public   Int32 ID { get{ return m_ID;}}
  
		private  string  m_Introduce;
		public   string Introduce { get{ return m_Introduce;}}
  
		private  Int32  m_IsDebt;
		public   Int32 IsDebt { get{ return BasicUtil.DecryptInt32Value(m_IsDebt);}}
  
		private  Int32  m_MoneyMax;
		public   Int32 MoneyMax { get{ return BasicUtil.DecryptInt32Value(m_MoneyMax);}}
  
		private  Int32  m_MoneyMin;
		public   Int32 MoneyMin { get{ return BasicUtil.DecryptInt32Value(m_MoneyMin);}}
  
		public  static bool LoadTable(Hashtable _tab, DataTableStructure _tabStructure, bool _isDeseriaAll)
		{

			if(!DataTable.ReaderPList(GetInstanceFile(), VALUE_NUM_PER_ROW, SerializableTable, _tab, _tabStructure, _isDeseriaAll))

			{
				throw  TableException.ErrorReader("Load File{0} Fail!!!",GetInstanceFile());
			}
			IsTableLoaded = true;
			return true;
		}

		public static Tab_Postoffice LoadTableItem(int nIdx, Hashtable _tab, DataTableStructure _tabStructure)
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
			Tab_Postoffice tabData = new Tab_Postoffice();
			tabData.m_ID = nKey;
			tabData.m_Introduce = values[0];
			tabData.m_IsDebt = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[3]));
			tabData.m_MoneyMax = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			tabData.m_MoneyMin = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[1]));
			
			_hash.Add(nKey,tabData);
		}

		public static Tab_Postoffice SerializableTableItem(string[] values,int nKey,Hashtable _hash)
		{
			if (values == null)
    			{
    				throw TableException.ErrorReader("values is null. Table: {0}  Key:{1}.", GetInstanceFile(), nKey);
    			}
			if (values.Length != VALUE_NUM_PER_ROW)
			{
				throw TableException.ErrorReader("Load {0}  error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(), VALUE_NUM_PER_ROW, values.Length);
			}
			Tab_Postoffice tabData = new Tab_Postoffice();
			tabData.m_ID = nKey;
			tabData.m_Introduce = values[0];
			tabData.m_IsDebt = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[3]));
			tabData.m_MoneyMax = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			tabData.m_MoneyMin = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[1]));
			
			_hash.Add(nKey,tabData);
			return tabData;
		}
  	}
}