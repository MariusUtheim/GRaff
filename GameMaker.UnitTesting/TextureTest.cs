using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
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
