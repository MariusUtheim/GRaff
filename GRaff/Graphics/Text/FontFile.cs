using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GRaff.Graphics.Text
{
	[Serializable]
	[XmlRoot("font")]
	public class FontFile
	{
		[XmlElement("info")]
		public FontInfo? Info { get; set; }

		[XmlElement("common")]
		public FontCommon? Common { get; set; }

		[XmlArray("pages")]
		[XmlArrayItem("page")]
		public List<FontPage>? Pages { get; set; }

		[XmlArray("chars")]
		[XmlArrayItem("char")]
		public List<FontCharacter>? Chars { get; set; }

		[XmlArray("kernings")]
		[XmlArrayItem("kerning")]
		public List<FontKerning>? Kernings { get; set; }
	}
      
   
}
