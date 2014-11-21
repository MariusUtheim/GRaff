using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	class RotationProperty : IParticleProperty
	{
		private Angle _rotation;

		public RotationProperty(Angle rotation)
		{
			this._rotation = rotation;
		}

		public void Update(Particle particle)
		{
			particle.TransformationMatrix.Rotate(_rotation);
		}
	}
}
