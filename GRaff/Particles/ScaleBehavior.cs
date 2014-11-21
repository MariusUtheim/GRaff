using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class ScaleBehavior : IParticleBehavior
	{
		private double _xScale, _yScale;
		private double _xScaleFactor, _yScaleFactor;

		public ScaleBehavior(double xScale = 1, double yScale = 1, double xScaleFactor = 1, double yScaleFactor = 1)
		{
			_xScale = xScale;
			_yScale = yScale;
			this._xScaleFactor = xScaleFactor;
			this._yScaleFactor = yScaleFactor;
		}

		public void AttachTo(Particle particle)
		{
			particle.TransformationMatrix.Scale(_xScale, _yScale);

			if (_xScaleFactor != 1 || _yScaleFactor == 1)
				particle.AttachProperty(new ScaleProperty(_xScaleFactor, _yScaleFactor));
		}
	}
}