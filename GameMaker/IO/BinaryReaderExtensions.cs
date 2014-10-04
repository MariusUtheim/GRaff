using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.IO
{
	internal static class BinaryReaderExtensions
	{
		public static string ReadString(this BinaryReader reader, int count)
		{
			return new String(reader.ReadChars(count));
		}
	}
}
