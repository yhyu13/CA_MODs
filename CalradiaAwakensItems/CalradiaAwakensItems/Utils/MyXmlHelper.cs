using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using TaleWorlds.Library;
using TaleWorlds.Core;

namespace CalradiaAwakensItems.Utils
{
	public static class MyXmlHelper
	{
		public static T SettingsFor<T>(string fileName)
		{
			string inputUri = Path.Combine(BasePath.Name, "Modules", Main.ModNamePath, "settings", $"{fileName}.xml");
			T result;
			try
			{
				using (XmlReader xmlReader = XmlReader.Create(inputUri))
				{
					XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
					xmlRootAttribute.ElementName = Main.ModName;
					xmlRootAttribute.IsNullable = true;
					bool flag = xmlReader.MoveToContent() != XmlNodeType.Element;
					if (flag)
					{
						result = default(T);
					}
					else
					{
						bool flag2 = xmlReader.Name != xmlRootAttribute.ElementName;
						if (flag2)
						{
							result = default(T);
						}
						else
						{
							XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);
							T t = (T)((object)xmlSerializer.Deserialize(xmlReader));
							result = t;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MyException.DisplayInGameConsole("Failed to load settings", ex);
				result = default(T);
			}
			return result;
		}

		public static void ReadEnglishLanguageFile(string fileName, ref Dictionary<string , string> dict)
		{
			string inputUri = Path.Combine(BasePath.Name, "Modules", Main.ModNamePath, "ModuleData", "Languages", $"{fileName}.xml");
			try
			{
				XDocument doc = XDocument.Load(inputUri);
				var elements = doc.Root.Element("strings").Elements();
				foreach(var pair in elements)
				{
					dict[pair.Attribute("id").Value] = pair.Attribute("text").Value;
				}
			}
			catch (Exception ex)
			{
				MyException.DisplayInGameConsole("Failed to load xml", ex);
			}
		}

		public static void ReadEquipmentSpecialPowerFile(string fileName, ref Dictionary<string, string> dict)
		{
			string inputUri = Path.Combine(BasePath.Name, "Modules", Main.ModNamePath, "ModuleData", "settings", $"{fileName}.xml");
			try
			{
				XDocument doc = XDocument.Load(inputUri);
				var elements = doc.Root.Element("strings").Elements();
				foreach (var pair in elements)
				{
					dict[pair.Attribute("id").Value] = pair.Attribute("text").Value;
				}
			}
			catch (Exception ex)
			{
				MyException.DisplayInGameConsole("Failed to load xml", ex);
			}
		}
	}
}
