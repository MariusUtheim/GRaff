using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class RotationBehavior : IParticleBehavior
	{
		private Angle _maxAngle;
		private Angle _minAngle;
		private Angle _maxRotation;
		private Angle _minRotation;

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


		public IParticleProperty Generate()
		{
			if (_minRotation == Angle.Zero && _maxRotation == Angle.Zero)
				return null;
			else
				return new RotationProperty(GRandom.Angle(_minRotation, _maxRotation));
		}

		public void Initialize(Particle particle)
		{
			particle.TransformationMatrix.Rotate(GRandom.Angle(_minAngle, _maxAngle));
		}
	}
}
