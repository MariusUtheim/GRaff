using System;
using GRaff.Randomness;


namespace GRaff.Particles.Behaviors
{
	public class DynamicRotationFeature : IParticleFeature
	{
		private IDistribution<Angle> _spin;

		public DynamicRotationFeature(Angle minSpin, Angle maxSpin)
		{
			_spin = new AngleDistribution(minSpin, maxSpin);
		}

		public DynamicRotationFeature(IDistribution<Angle> spinDistribution)
		{
			if (spinDistribution == null) throw new ArgumentNullException("spinDistribution");	 /*C#6. 0*/
			_spin = spinDistribution;
		}

		public IDistribution<Angle> SpinDistribution
		{
			get { return _spin; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				_spin = value;
			}
		}

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new RotatingBehavior(_spin.Generate()));
		}
	}
}
