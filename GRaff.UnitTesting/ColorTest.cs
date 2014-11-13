using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class ColorTest
	{
		[TestMethod]
		public void Color_Constructors()
		{
			Color expected = new Color((byte)128, (byte)16, (byte)32, (byte)48);

			Assert.AreEqual(expected.A, 128);
			Assert.AreEqual(expected.R, 16);
			Assert.AreEqual(expected.G, 32);
			Assert.AreEqual(expected.B, 48);
			Assert.AreEqual(expected.Argb, 0x80102030);
			
			Assert.AreEqual(expected, new Color(128, 16, 32, 48));
			Assert.AreEqual(expected, new Color(0x80102030));
		}

		[TestMethod]
		public void Color_Transformations()
		{
			Color expected, actual;

			actual = Color.Merge(new Color[] { 0xFF300000, 0xFF003000, 0xFF000030 });
			expected = 0xFF101010;
			Assert.AreEqual<Color>(expected, actual);

			actual = new Color(0xFF102030).Transparent(128);
			expected = 0x80102030;
			Assert.AreEqual<Color>(expected, actual);

			actual = new Color(0xFF102030).Transparent(0.5);
			expected = 0x80102030;
			Assert.AreEqual<Color>(expected, actual);
		}

		[TestMethod]
		public void Color_OverridesFromSystemObject()
		{
			Color color = new Color(0xFF102030);

			Assert.AreEqual("Color=0xFF102030", color.ToString());
			Assert.AreEqual(true, color.Equals(new Color(0xFF102030)));
			unchecked
			{
				Assert.AreEqual((int)0xFF102030, color.GetHashCode());
			}
		}

		[TestMethod]
		public void Color_Operators()
		{
			Color expected = new Color(0xFF102030);

			Assert.IsTrue(expected == new Color(0xFF102030));
			Assert.IsTrue(expected != new Color(0x00000000));
			Assert.IsTrue(expected == (Color)0xFF102030);
		}
	}
}
