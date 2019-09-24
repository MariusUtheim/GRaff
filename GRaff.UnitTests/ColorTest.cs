using System;
using Xunit;

namespace GRaff.UnitTesting
{
	public class ColorTest
	{
		[Fact]
		public void Color_Constructors()
		{
			Color expected = new Color((byte)16, (byte)32, (byte)48, (byte)128);

			Assert.Equal(16, expected.R);
			Assert.Equal(32, expected.G);
			Assert.Equal(48, expected.B);
			Assert.Equal(128, expected.A);
			Assert.Equal(0x10203080u, expected.Rgba);
			
			Assert.Equal(expected, new Color(16, 32, 48, 128));
			Assert.Equal(expected, Color.FromRgba(0x10203080));
		}

		[Fact]
		public void Color_Transformations()
		{
			Color expected, actual;

			actual = Color.FromRgba(0x102030FF).Transparent(128);
			expected = 0x10203080u;
			Assert.Equal(expected, actual);

			actual = Color.FromRgba(0x102030FF).Transparent(0.5);
			expected = 0x10203080u;
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Color_OverridesFromSystemObject()
		{
			Color color = Color.FromRgba(0x102030FF);

			Assert.Equal("Color=0x102030FF", color.ToString());
            Assert.Equal(color, Color.FromRgba(0x102030FF));
			unchecked
			{
				Assert.Equal(0x102030FF, color.GetHashCode());
			}
		}

		[Fact]
		public void Color_Operators()
		{
			Color expected = Color.FromRgba(0x102030FF);

			Assert.True(expected == Color.FromRgba(0x102030FF));
			Assert.True(expected != Color.FromRgba(0x00000000));
			Assert.True(expected == (Color)0x102030FF);
		}
	}
}
