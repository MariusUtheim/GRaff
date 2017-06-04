using System;


namespace GRaff
{
	public static partial class GMath
	{
		public static Angle Acos(double d)
		{
			return Angle.Rad(Math.Acos(d));
		}

		public static Angle Asin(double d)
		{
			return Angle.Rad(Math.Asin(d));
		}

		public static Angle Atan2(Vector v)
		{
			return Angle.Rad(Math.Atan2(v.Y, v.X));
		}
	}
}


