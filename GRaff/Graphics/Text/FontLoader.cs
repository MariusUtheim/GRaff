// ---- AngelCode BmFont XML serializer ----------------------
// ---- By DeadlyDan @ deadlydan@gmail.com -------------------
// ---- There's no license restrictions, use as you will. ----
// ---- Credits to http://www.angelcode.com/ -----------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SysFont = System.Drawing.Font;


namespace GRaff.Graphics.Text
{

	internal static class FontLoader
	{
		private static XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));

		public static FontFile Load(string filename)
		{
			using (var textReader = new FileStream(filename, FileMode.Open, FileAccess.Read))
				return (FontFile)deserializer.Deserialize(textReader);
		}
       
	}
}