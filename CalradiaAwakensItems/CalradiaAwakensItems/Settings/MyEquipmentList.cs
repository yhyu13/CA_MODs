using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CalradiaAwakensItems.Settings
{
	[Serializable]
	public class MyEquipmentList
	{
		[XmlArray("Data")]
		public List<EquipmentData> data { get; set; }
	}

	[Serializable]
	public class EquipmentData
	{
		[XmlElement]
		public string Name { get; set; }
		[XmlElement]
		public string ShotgunNum { get; set; }
		[XmlElement]
		public string ShotgunSpread { get; set; }
		[XmlElement]
		public string ShotgunSpeedDiff { get; set; }
	}
}
