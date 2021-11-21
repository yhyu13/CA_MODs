using System;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace CA_EagleRisingSpecialPowers.Items
{
	public class MyItems
	{
		public static ItemObject GetItemObjectById(string stringID)
		{
			return Extensions.GetRandomElementWithPredicate<ItemObject>(MBObjectManager.Instance.GetObjectTypeList<ItemObject>(), delegate (ItemObject a)
			{
				bool flag = a.StringId == stringID;
				return flag;
			});
		}
	}
}