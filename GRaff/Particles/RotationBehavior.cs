using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class RotationBehavior : IParticleBehavior
	{
		Angle _maxAngle, _minAngle;
		Angle _maxRotation, _minRotation;

		public RotationBehavior(Angle minAngle, Angle maxAngle, Angle minRotation, Angle maxRotation)
		{
			this._minAngle = minAngle;
			this._maxAngle = maxAngle;
			this._minRotation = minRotation;
			this._maxRotation = maxRotation;
		}

		public RotationBehavior()
			: this(Angle.Zero, Angle.Zero, Angle.Zero, Angle.Zero)
		{ }

		public RotationBehavior(Angle minRotation, Angle maxRotation)
			: this(Angle.Zero, Angle.Zero, minRotation, maxRotation)
		{ }


		public void AttachTo(Particle particle)
		{
			particle.TransformationMatrix.Rotate(GRandom.Angle(_minAngle, _maxAngle));
			if (_minRotation != Angle.Zero || _maxRotation != Angle.Zero)
				particle.AttachProperty(new RotationProperty(GRandom.Angle(_minRotation, _maxRotation)));
		}
	}
}
