// This code created by tools automatically, you would not modify it.
using System;
using System.Collections.Generic;
using System.Collections;
using BH.Core.Utils;
  
namespace BH.Core.Configs
{
    	[Serializable]
    	public  class Tab_PlayerData : ITableOperate
	{

    		private const string TAB_FILE_DATA = "DataTable/PlayerData";

    		private const int VALUE_NUM_PER_ROW = 7;// not contains id
    		public static bool IsTableLoaded = false;
    		public enum _ID
    		{
    			INVLAID_INDEX = -1,
			ID_ID,
			ID_NAME,
			ID_ICON,
			ID_CASH,
			ID_DEPOSIT,
			ID_DEBT,
			ID_HEALTH,
			ID_REPUTE,
			MAX_RECORD
    		}
    		public static string GetInstanceFile(){return TAB_FILE_DATA; }
  
		private  Int32  m_Cash;
		public   Int32 Cash { get{ return BasicUtil.DecryptInt32Value(m_Cash);}}
  
		private  Int32  m_Debt;
		public   Int32 Debt { get{ return BasicUtil.DecryptInt32Value(m_Debt);}}
  
		private  Int32  m_Deposit;
		public   Int32 Deposit { get{ return BasicUtil.DecryptInt32Value(m_Deposit);}}
  
		private  Int32  m_Health;
		public   Int32 Health { get{ return BasicUtil.DecryptInt32Value(m_Health);}}
  
		private  string  m_Icon;
		public   string Icon { get{ return m_Icon;}}
  
		private  Int32  m_ID;
		public   Int32 ID { get{ return m_ID;}}
  
		private  string  m_Name;
		public   string Name { get{ return m_Name;}}
  
		private  Int32  m_Repute;
		public   Int32 Repute { get{ return BasicUtil.DecryptInt32Value(m_Repute);}}
  
		public  static bool LoadTable(Hashtable _tab, DataTableStructure _tabStructure, bool _isDeseriaAll)
		{

			if(!DataTable.ReaderPList(GetInstanceFile(), VALUE_NUM_PER_ROW, SerializableTable, _tab, _tabStructure, _isDeseriaAll))

			{
				throw  TableException.ErrorReader("Load File{0} Fail!!!",GetInstanceFile());
			}
			IsTableLoaded = true;
			return true;
		}

		public static Tab_PlayerData LoadTableItem(int nIdx, Hashtable _tab, DataTableStructure _tabStructure)
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
			Tab_PlayerData tabData = new Tab_PlayerData();
			tabData.m_Cash = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			tabData.m_Debt = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[4]));
			tabData.m_Deposit = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[3]));
			tabData.m_Health = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[5]));
			tabData.m_Icon = values[1];
			tabData.m_ID = nKey;
			tabData.m_Name = values[0];
			tabData.m_Repute = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[6]));
			
			_hash.Add(nKey,tabData);
		}

		public static Tab_PlayerData SerializableTableItem(string[] values,int nKey,Hashtable _hash)
		{
			if (values == null)
    			{
    				throw TableException.ErrorReader("values is null. Table: {0}  Key:{1}.", GetInstanceFile(), nKey);
    			}
			if (values.Length != VALUE_NUM_PER_ROW)
			{
				throw TableException.ErrorReader("Load {0}  error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(), VALUE_NUM_PER_ROW, values.Length);
			}
			Tab_PlayerData tabData = new Tab_PlayerData();
			tabData.m_Cash = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			tabData.m_Debt = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[4]));
			tabData.m_Deposit = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[3]));
			tabData.m_Health = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[5]));
			tabData.m_Icon = values[1];
			tabData.m_ID = nKey;
			tabData.m_Name = values[0];
			tabData.m_Repute = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[6]));
			
			_hash.Add(nKey,tabData);
			return tabData;
		}
  	}
}