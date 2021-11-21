﻿using System;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace CalradiaAwakensSpecialPowers.Items
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

