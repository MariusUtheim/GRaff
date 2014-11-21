using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class LinearMotionBehavior : IParticleBehavior
	{

		public LinearMotionBehavior(double speed)
		{
			SpeedLowerBound = SpeedUpperBound = speed;
			DirectionLowerBound = Angle.Zero;
			DirectionUpperBound = Angle.MaxAngle;
		}

		public LinearMotionBehavior(double speedLowerBound, double speedUpperBound)
		{
			SpeedLowerBound = speedLowerBound;
			SpeedUpperBound = speedUpperBound;
			DirectionLowerBound = Angle.Zero;
			DirectionUpperBound = Angle.MaxAngle;
		}

		public LinearMotionBehavior(double speed, Angle directionLowerBound, Angle directionUpperBound)
		{
			SpeedLowerBound = SpeedUpperBound = speed;
			DirectionLowerBound = directionLowerBound;
			DirectionUpperBound = directionUpperBound;
		}

		public LinearMotionBehavior(double speedLowerBound, double speedUpperBound, Angle directionLowerBound, Angle directionUpperBound)
		{
			SpeedLowerBound = speedLowerBound;
			SpeedUpperBound = speedUpperBound;
			DirectionLowerBound = directionLowerBound;
			DirectionUpperBound = directionUpperBound;
		}

		public double SpeedLowerBound { get; set; }
		public double SpeedUpperBound { get; set; }
		public Angle DirectionLowerBound { get; set; }

		public Angle DirectionUpperBound { get; set; }

		public void AttachTo(Particle particle)
		{
			particle.Velocity = new Vector(GRandom.Double(SpeedLowerBound, SpeedUpperBound), GRandom.Angle(DirectionLowerBound, DirectionUpperBound));
		}
	}
}
