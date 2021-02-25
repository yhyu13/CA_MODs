using System;
using TaleWorlds.CampaignSystem;

namespace CalradiaAwakensSpecialPowers.Utils
{
	public class MyDataStore
    {
		public static void SyncValue<T>(IDataStore dataStore, string key, ref T data)
		{
			try
			{
				dataStore.SyncData<T>(key, ref data);
			}
			catch (Exception)
			{
			}
		}
	}
}
