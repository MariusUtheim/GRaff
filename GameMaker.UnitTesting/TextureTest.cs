using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class TextureTest
	{
		[TestMethod]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void FileNotFound() 
		{
			Texture.Load(@"C:\DoesNot.bmp");
		}
	}
}
