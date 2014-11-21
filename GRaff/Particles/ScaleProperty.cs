using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	class ScaleProperty : IParticleProperty
	{
		private double _xScaleFactor;
		private double _yScaleFactor;

		public ScaleProperty(double xScaleFactor, double yScaleFactor)
		{
			this._xScaleFactor = xScaleFactor;
			this._yScaleFactor = yScaleFactor;
		}

		public void Update(Particle particle)
		{
			particle.TransformationMatrix.Scale(_xScaleFactor, _yScaleFactor);
		}
	}
}
