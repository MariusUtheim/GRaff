using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace GRaff.Audio
{
	public static class Listener
	{
		public static Point Location
		{
			get
			{
				float v1, v2, v3;
				AL.GetListener(ALListener3f.Position, out v1, out v2, out v3);
				return new Point(v1, v2);
			}

			set
			{
				AL.Listener(ALListener3f.Position, (float)value.X, (float)value.Y, 0);
			}
		}

		private static Angle _cachedDirection = Angle.Zero;
		public static Vector Velocity
		{
			get
			{
				float v1, v2, v3;
				AL.GetListener(ALListener3f.Velocity, out v1, out v2, out v3);
				if (v1 == 0 && v2 == 0)
					return new Vector(0, _cachedDirection);
				else
				{
					Vector result = new Vector(v1, v2);
					_cachedDirection = result.Direction;
					return result;
				}
			}
			
			set
			{
				_cachedDirection = value.Direction;
				AL.Listener(ALListener3f.Velocity, (float)value.X, (float)value.Y, 0);
			}
		}

		public static double MasterVolume
		{
			get
			{
				float v;
				AL.GetListener(ALListenerf.Gain, out v);
				return v;
			}

			set
			{
				AL.Listener(ALListenerf.Gain, (float)value);
			}
		}

		public static DistanceModel DistanceModel
		{
			get
			{
				return (DistanceModel)AL.GetDistanceModel();
			}
			set
			{
				AL.DistanceModel((ALDistanceModel)value);
			}
		}

		public static double DopplerFactor
		{
			get
			{
				return AL.Get(ALGetFloat.DopplerFactor);
			}
			set
			{
				AL.DopplerFactor((float)value);
			}
		}

		public static double SpeedOfSound
		{
			get
			{
				return AL.Get(ALGetFloat.SpeedOfSound);
			}
			set
			{
				AL.SpeedOfSound((float)value);
			}
		}
	}
}
