// This code created by tools automatically, you would not modify it.
using System;
using System.Collections.Generic;
using System.Collections;
using BH.Core.Utils;
  
namespace BH.Core.Configs
{
    	[Serializable]
    	public  class Tab_GoodData : ITableOperate
	{

    		private const string TAB_FILE_DATA = "DataTable/GoodData";

    		private const int VALUE_NUM_PER_ROW = 13;// not contains id
    		public static bool IsTableLoaded = false;
    		public enum _ID
    		{
    			INVLAID_INDEX = -1,
			ID_ID,
			ID_NAME,
			ID_ICON,
			ID_REPUTE,
			ID_PRICEMIN,
			ID_PRICEMAX,
			ID_EVENTRATIODROP,
			ID_EVENTRATIORISE,
			ID_EVENTDROPID1,
			ID_EVENTRISEID1,
			ID_EVENTDROPID2,
			ID_EVENTRISEID2,
			ID_EVENTDROPID3,
			ID_EVENTRISEID3,
			MAX_RECORD
    		}
    		public static string GetInstanceFile(){return TAB_FILE_DATA; }
  
		public   int GetEventDropIdCount() { return 3; } 
		private   Int32[]  m_EventDropId = new Int32[3];
		public    Int32 GetEventDropIdbyIndex(int idx)
		{
			if(idx>=0 && idx<3) return BasicUtil.DecryptInt32Value(m_EventDropId[idx]);
			return -1;
		}
  
		private  Int32  m_EventRatioDrop;
		public   Int32 EventRatioDrop { get{ return BasicUtil.DecryptInt32Value(m_EventRatioDrop);}}
  
		private  Int32  m_EventRatioRise;
		public   Int32 EventRatioRise { get{ return BasicUtil.DecryptInt32Value(m_EventRatioRise);}}
  
		public   int GetEventRiseIdCount() { return 3; } 
		private   Int32[]  m_EventRiseId = new Int32[3];
		public    Int32 GetEventRiseIdbyIndex(int idx)
		{
			if(idx>=0 && idx<3) return BasicUtil.DecryptInt32Value(m_EventRiseId[idx]);
			return -1;
		}
  
		private  string  m_Icon;
		public   string Icon { get{ return m_Icon;}}
  
		private  Int32  m_ID;
		public   Int32 ID { get{ return m_ID;}}
  
		private  string  m_Name;
		public   string Name { get{ return m_Name;}}
  
		private  Int32  m_PriceMax;
		public   Int32 PriceMax { get{ return BasicUtil.DecryptInt32Value(m_PriceMax);}}
  
		private  Int32  m_PriceMin;
		public   Int32 PriceMin { get{ return BasicUtil.DecryptInt32Value(m_PriceMin);}}
  
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

		public static Tab_GoodData LoadTableItem(int nIdx, Hashtable _tab, DataTableStructure _tabStructure)
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
			Tab_GoodData tabData = new Tab_GoodData();
			tabData.m_EventDropId[0] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[7]));
			
			tabData.m_EventDropId[1] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[9]));
			
			tabData.m_EventDropId[2] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[11]));
			
			tabData.m_EventRatioDrop = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[5]));
			tabData.m_EventRatioRise = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[6]));
			tabData.m_EventRiseId[0] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[8]));
			
			tabData.m_EventRiseId[1] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[10]));
			
			tabData.m_EventRiseId[2] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[12]));
			
			tabData.m_Icon = values[1];
			tabData.m_ID = nKey;
			tabData.m_Name = values[0];
			tabData.m_PriceMax = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[4]));
			tabData.m_PriceMin = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[3]));
			tabData.m_Repute = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			
			_hash.Add(nKey,tabData);
		}

		public static Tab_GoodData SerializableTableItem(string[] values,int nKey,Hashtable _hash)
		{
			if (values == null)
    			{
    				throw TableException.ErrorReader("values is null. Table: {0}  Key:{1}.", GetInstanceFile(), nKey);
    			}
			if (values.Length != VALUE_NUM_PER_ROW)
			{
				throw TableException.ErrorReader("Load {0}  error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(), VALUE_NUM_PER_ROW, values.Length);
			}
			Tab_GoodData tabData = new Tab_GoodData();
			tabData.m_EventDropId[0] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[7]));
			
			tabData.m_EventDropId[1] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[9]));
			
			tabData.m_EventDropId[2] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[11]));
			
			tabData.m_EventRatioDrop = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[5]));
			tabData.m_EventRatioRise = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[6]));
			tabData.m_EventRiseId[0] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[8]));
			
			tabData.m_EventRiseId[1] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[10]));
			
			tabData.m_EventRiseId[2] = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[12]));
			
			tabData.m_Icon = values[1];
			tabData.m_ID = nKey;
			tabData.m_Name = values[0];
			tabData.m_PriceMax = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[4]));
			tabData.m_PriceMin = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[3]));
			tabData.m_Repute = BasicUtil.EncryptInt32Value(Convert.ToInt32(values[2]));
			
			_hash.Add(nKey,tabData);
			return tabData;
		}
  	}
}