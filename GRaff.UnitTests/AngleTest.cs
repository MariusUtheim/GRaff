using System;
using Xunit;

namespace GRaff.UnitTesting
{
	public class AngleTest
	{
        [Fact]
        public void Angle_RadiansAndDegrees()
		{
			for (int i = -360; i < 360; i++)
				Assert.Equal(Angle.Deg(i), Angle.Rad(i * GMath.DegToRad));
		}

        [Fact]
        public void Angle_Direction()
		{
			double delta = Angle.Epsilon.Degrees;

			Angle expected = Angle.Deg(45);
			Assert.Equal(expected, Angle.Direction(2, 2));
			Assert.Equal(expected, Angle.Direction(new Point(5, 5)));
			Assert.Equal(expected, Angle.Direction(10, 0, 12, 2));
			Assert.Equal(expected, Angle.Direction(new Point(104, 204), new Point(108, 208)));
			Assert.Equal(Angle.Zero, Angle.Direction(0, 0));
		}

        [Fact]
        public void Angle_Acute()
		{
			Angle a0 = Angle.Deg(83);
			Assert.Equal(Angle.Deg(45), Angle.Acute(a0, a0 + Angle.Deg(45)));
			Assert.Equal(Angle.Deg(90), Angle.Acute(a0, a0 + Angle.Deg(270)));

			Angle _0 = Angle.Deg(0), _180 = Angle.Deg(180);
			Assert.Equal(Angle.Deg(180), Angle.Acute(_0, _180));
			Assert.Equal(Angle.Deg(180), Angle.Acute(_180, _0));
		}

        [Fact]
        public void Angle_Operators()
		{
			Assert.Equal("45°", Angle.Deg(45).ToString());

			Assert.True(Angle.Deg(180) == Angle.Rad(GMath.Tau / 2));
			Assert.True(Angle.Deg(180) != Angle.Rad(180.00001));
			Assert.Equal(Angle.Deg(100), Angle.Deg(40) + Angle.Deg(60));
			Assert.Equal(Angle.Deg(40), Angle.Deg(100) - Angle.Deg(60));
            Assert.Equal(Angle.Deg(270), Angle.Deg(-90));
			Assert.Equal(Angle.Deg(90) * 4.0, Angle.Deg(360));
			Assert.Equal(-Angle.Deg(90), Angle.Deg(270));
        }
	}
}
