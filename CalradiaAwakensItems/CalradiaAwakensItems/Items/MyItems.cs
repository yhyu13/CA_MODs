using System;
using TaleWorlds.Core;

namespace CalradiaAwakensItems.Items
{
	public class MyItems
    {
		public static ItemObject GetItemObjectById(string stringID)
		{
			return Extensions.GetRandomElementWithPredicate<ItemObject>(ItemObject.All, delegate (ItemObject a) // Hang Yu Feb 9th. After BannerLoad version 1.5.8 changes
			{
				bool flag = a.StringId == stringID;
				return flag;
			});
			//return Extensions.GetRandomElement<ItemObject>(ItemObject.All, delegate (ItemObject a) // Hang Yu Feb 9th. Prior to BannerLoad version 1.5.8 changes
			//{
			//	bool flag = a.StringId == stringID;
			//	float result;
			//	if (flag)
			//	{
			//		result = 1f;
			//	}
			//	else
			//	{
			//		result = 0f;
			//	}
			//	return result;
			//});
		}
	}
}