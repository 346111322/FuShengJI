// This code created by tools automatically, you would not modify it.
//#define TEST_ASSETBUNDLE
//#define TEST_BINARY
using System;
using System.Collections;
using UnityEngine;
using BH.Core.Debug;
using System.IO;
using BH.Core.Utils;
  
namespace BH.Core.Configs
{
	public interface ITableOperate
	{
		//bool LoadTable(Hashtable _tab);
		//string GetInstanceFile();
	}
	public  delegate void SerializableTable(string[] values, int nKey, Hashtable _hash);
	[Serializable]
	public  class DataTable
	{
		private DataTable() 
		{
			BasicUtil.GenerateCryptPlusValue();
		}
		private static DataTable instance = null;
		public static DataTable Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new DataTable();
				}
				return instance;
			}
		}

		public static bool ReaderPList(String _tabName, int valueNum, SerializableTable _fun, Hashtable _hash, DataTableStructure _tableStructture, bool _isDeseriaAll)

		{

    			TextAsset txtAsset = Resources.Load(_tabName,typeof(TextAsset)) as TextAsset;
    
			string[] alldataRow = txtAsset.text.Split('\n');
			string[] values = new string[valueNum];
			int cnt = alldataRow.Length;
			string line = "";
			for(int idx = 0; idx < cnt; ++idx)
			{
				line = alldataRow[idx];
				if(String.IsNullOrEmpty(line))continue;
				string[] strCol = line.Split('\t');
				if (strCol.Length == 0) continue;
				string skey = strCol[0];
				Array.Copy(strCol, 1, values, 0, valueNum);
				if (string.IsNullOrEmpty(skey)) return false;
				_fun(values, int.Parse(skey), _hash);
			}

			return true;
		}
  
		private Hashtable m_Adventure = new Hashtable();
		public Hashtable GetAdventure()
		{
			if (!Tab_Adventure.IsTableLoaded)
			{

				Tab_Adventure.LoadTable(m_Adventure, m_AdventureStructure, false);

			}
			return m_Adventure;
		}
		private DataTableStructure m_AdventureStructure = new DataTableStructure();
  
		private Hashtable m_Event = new Hashtable();
		public Hashtable GetEvent()
		{
			if (!Tab_Event.IsTableLoaded)
			{

				Tab_Event.LoadTable(m_Event, m_EventStructure, false);

			}
			return m_Event;
		}
		private DataTableStructure m_EventStructure = new DataTableStructure();
  
		private Hashtable m_GoodData = new Hashtable();
		public Hashtable GetGoodData()
		{
			if (!Tab_GoodData.IsTableLoaded)
			{

				Tab_GoodData.LoadTable(m_GoodData, m_GoodDataStructure, false);

			}
			return m_GoodData;
		}
		private DataTableStructure m_GoodDataStructure = new DataTableStructure();
  
		private Hashtable m_Hospial = new Hashtable();
		public Hashtable GetHospial()
		{
			if (!Tab_Hospial.IsTableLoaded)
			{

				Tab_Hospial.LoadTable(m_Hospial, m_HospialStructure, false);

			}
			return m_Hospial;
		}
		private DataTableStructure m_HospialStructure = new DataTableStructure();
  
		private Hashtable m_Location = new Hashtable();
		public Hashtable GetLocation()
		{
			if (!Tab_Location.IsTableLoaded)
			{

				Tab_Location.LoadTable(m_Location, m_LocationStructure, false);

			}
			return m_Location;
		}
		private DataTableStructure m_LocationStructure = new DataTableStructure();
  
		private Hashtable m_PlayerData = new Hashtable();
		public Hashtable GetPlayerData()
		{
			if (!Tab_PlayerData.IsTableLoaded)
			{

				Tab_PlayerData.LoadTable(m_PlayerData, m_PlayerDataStructure, false);

			}
			return m_PlayerData;
		}
		private DataTableStructure m_PlayerDataStructure = new DataTableStructure();
  
		private Hashtable m_Postoffice = new Hashtable();
		public Hashtable GetPostoffice()
		{
			if (!Tab_Postoffice.IsTableLoaded)
			{

				Tab_Postoffice.LoadTable(m_Postoffice, m_PostofficeStructure, false);

			}
			return m_Postoffice;
		}
		private DataTableStructure m_PostofficeStructure = new DataTableStructure();
  
		private Hashtable m_Rental = new Hashtable();
		public Hashtable GetRental()
		{
			if (!Tab_Rental.IsTableLoaded)
			{

				Tab_Rental.LoadTable(m_Rental, m_RentalStructure, false);

			}
			return m_Rental;
		}
		private DataTableStructure m_RentalStructure = new DataTableStructure();
  
    		public  IEnumerator InitTable()
    		{
    			
			if(Tab_Adventure.LoadTable(m_Adventure, m_AdventureStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_Adventure.GetInstanceFile(),m_Adventure.Count));
			}
			yield return null;
  
			if(Tab_Event.LoadTable(m_Event, m_EventStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_Event.GetInstanceFile(),m_Event.Count));
			}
			yield return null;
  
			if(Tab_GoodData.LoadTable(m_GoodData, m_GoodDataStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_GoodData.GetInstanceFile(),m_GoodData.Count));
			}
			yield return null;
  
			if(Tab_Hospial.LoadTable(m_Hospial, m_HospialStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_Hospial.GetInstanceFile(),m_Hospial.Count));
			}
			yield return null;
  
			if(Tab_Location.LoadTable(m_Location, m_LocationStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_Location.GetInstanceFile(),m_Location.Count));
			}
			yield return null;
  
			if(Tab_PlayerData.LoadTable(m_PlayerData, m_PlayerDataStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_PlayerData.GetInstanceFile(),m_PlayerData.Count));
			}
			yield return null;
  
			if(Tab_Postoffice.LoadTable(m_Postoffice, m_PostofficeStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_Postoffice.GetInstanceFile(),m_Postoffice.Count));
			}
			yield return null;
  
			if(Tab_Rental.LoadTable(m_Rental, m_RentalStructure, false))

			{
				Log.Message(string.Format("Load Table:{0} OK! Record({1})",Tab_Rental.GetInstanceFile(),m_Rental.Count));
			}
			yield return null;
  
    			yield return null;
    		}
  
		public Tab_Adventure GetAdventureById(int nIdex)
		{
			if( GetAdventure().ContainsKey(nIdex))
			{
				return m_Adventure[nIdex] as Tab_Adventure;
			}
			else
			{
				return Tab_Adventure.LoadTableItem(nIdex, m_Adventure, m_AdventureStructure);
			}
		}

  
		public Tab_Event GetEventById(int nIdex)
		{
			if( GetEvent().ContainsKey(nIdex))
			{
				return m_Event[nIdex] as Tab_Event;
			}
			else
			{
				return Tab_Event.LoadTableItem(nIdex, m_Event, m_EventStructure);
			}
		}

  
		public Tab_GoodData GetGoodDataById(int nIdex)
		{
			if( GetGoodData().ContainsKey(nIdex))
			{
				return m_GoodData[nIdex] as Tab_GoodData;
			}
			else
			{
				return Tab_GoodData.LoadTableItem(nIdex, m_GoodData, m_GoodDataStructure);
			}
		}

  
		public Tab_Hospial GetHospialById(int nIdex)
		{
			if( GetHospial().ContainsKey(nIdex))
			{
				return m_Hospial[nIdex] as Tab_Hospial;
			}
			else
			{
				return Tab_Hospial.LoadTableItem(nIdex, m_Hospial, m_HospialStructure);
			}
		}

  
		public Tab_Location GetLocationById(int nIdex)
		{
			if( GetLocation().ContainsKey(nIdex))
			{
				return m_Location[nIdex] as Tab_Location;
			}
			else
			{
				return Tab_Location.LoadTableItem(nIdex, m_Location, m_LocationStructure);
			}
		}

  
		public Tab_PlayerData GetPlayerDataById(int nIdex)
		{
			if( GetPlayerData().ContainsKey(nIdex))
			{
				return m_PlayerData[nIdex] as Tab_PlayerData;
			}
			else
			{
				return Tab_PlayerData.LoadTableItem(nIdex, m_PlayerData, m_PlayerDataStructure);
			}
		}

  
		public Tab_Postoffice GetPostofficeById(int nIdex)
		{
			if( GetPostoffice().ContainsKey(nIdex))
			{
				return m_Postoffice[nIdex] as Tab_Postoffice;
			}
			else
			{
				return Tab_Postoffice.LoadTableItem(nIdex, m_Postoffice, m_PostofficeStructure);
			}
		}

  
		public Tab_Rental GetRentalById(int nIdex)
		{
			if( GetRental().ContainsKey(nIdex))
			{
				return m_Rental[nIdex] as Tab_Rental;
			}
			else
			{
				return Tab_Rental.LoadTableItem(nIdex, m_Rental, m_RentalStructure);
			}
		}

  	}
}