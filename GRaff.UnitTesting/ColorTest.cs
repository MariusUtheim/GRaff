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
			Color expected = new Color((byte)16, (byte)32, (byte)48, (byte)128);

			Assert.AreEqual(expected.R, 16);
			Assert.AreEqual(expected.G, 32);
			Assert.AreEqual(expected.B, 48);
			Assert.AreEqual(expected.A, 128);
			Assert.AreEqual(expected.Rgba, 0x10203080u);
			
			Assert.AreEqual(expected, new Color(16, 32, 48, 128));
			Assert.AreEqual(expected, Color.FromRgba(0x10203080));
		}

		[TestMethod]
		public void Color_Transformations()
		{
			Color expected, actual;

			actual = Color.FromRgba(0x102030FF).Transparent(128);
			expected = 0x10203080u;
			Assert.AreEqual<Color>(expected, actual);

			actual = Color.FromRgba(0x102030FF).Transparent(0.5);
			expected = 0x10203080u;
			Assert.AreEqual<Color>(expected, actual);
		}

		[TestMethod]
		public void Color_OverridesFromSystemObject()
		{
			Color color = Color.FromRgba(0x102030FF);

			Assert.AreEqual("Color=0x102030FF", color.ToString());
			Assert.AreEqual(true, color.Equals(Color.FromRgba(0x102030FF)));
			unchecked
			{
				Assert.AreEqual((int)0x102030FF, color.GetHashCode());
			}
		}

		[TestMethod]
		public void Color_Operators()
		{
			Color expected = Color.FromRgba(0x102030FF);

			Assert.IsTrue(expected == Color.FromRgba(0x102030FF));
			Assert.IsTrue(expected != Color.FromRgba(0x00000000));
			Assert.IsTrue(expected == (Color)0x102030FF);
		}
	}
}
