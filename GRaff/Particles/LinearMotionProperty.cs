using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class LinearMotionProperty : IParticleProperty
	{
		private double _dx, _dy;

		public LinearMotionProperty(Vector v)
		{
			_dx = v.X;
			_dy = v.Y;
		}

		public LinearMotionProperty(double dx, double dy)
		{
			_dx = dx;
			_dy = dy;
		}

		public void Update(Particle particle)
		{
			particle.TransformationMatrix.M02 += _dx;
			particle.TransformationMatrix.M12 += _dy;
		}
	}

}
