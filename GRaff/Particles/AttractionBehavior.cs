using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class AttractionBehavior : IParticleBehavior
	{
		private Point _origin;
		private double _strength;

		public AttractionBehavior(Point origin, double strength)
		{
			_origin = origin;
			_strength = strength;
		}

		public IParticleProperty Generate()
		{
			return new AttractionProperty(_origin, _strength);
		}

		public void Initialize(Particle particle)
		{
			return;
		}
	}
}
