using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker.Motions
{
    public static class MotionsExtensions
    {
		public static bool PlaceMeeting(this GameObject instance, Point location, GameObject other)
		{
			Point previousLocation = instance.Location;
			try
			{
				instance.Location = location;
				return instance.Intersects(other);
			}
			finally
			{
				instance.Location = previousLocation;
			}
		}

		public static bool PlaceFree(this GameObject instance, Point location)
		{
			throw new NotImplementedException();
		}

		public static bool PlaceFree<T>(this GameObject instance, Point location) where T : GameObject
		{
			throw new NotImplementedException();
		}

		public static bool PositionFree(this GameObject instance)
		{
			throw new NotImplementedException();
		}

		public static void MoveOutside(this GameObject instance, Angle direction)
		{
			throw new NotImplementedException();
		}

		public static void MoveOutside(this GameObject instance, Angle direction, double maxDistance)
		{
			throw new NotImplementedException();
		}

		public static void MoveSnap(this GameObject instance, double xgrid, double ygrid)
		{
			throw new NotImplementedException();
		}

		public static void MoveTowards(this MovingObject instance, Point point, double speed)
		{
			instance.Velocity = Vector.FromPolar(speed, (point - instance.Location).Direction);
		}

		public static T NearestInstance<T>(this GameObject instance) where T : GameObject
		{
			T min = null;
			double offset = Double.PositiveInfinity;
			foreach (var obj in Instance<T>.All)
				if ((obj.Location - instance.Location).Magnitude < offset)
				{
					offset = (obj.Location - instance.Location).Magnitude;
					min = obj;
				}

			return min;
		}

		public static void AccelerateTowards(this MovingObject instance, Point point, double speed)
		{
			instance.Velocity += Vector.FromPolar(speed, (point - instance.Location).Direction);
		}
    }
}
