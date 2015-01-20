using System;
 

namespace GRaff.Randomness
{
	public class VectorDistribution : IDistribution<Vector>
	{
		private IDistribution<double> _radius;
		private IDistribution<Angle> _angle;

		public VectorDistribution(double magnitude)
			: this(GRandom.Source, magnitude)
		{ }

		public VectorDistribution(double magnitude, Angle minDirection, Angle maxDirection)
			: this(GRandom.Source, magnitude, minDirection, maxDirection)
		{ }

		public VectorDistribution(double minMagnitude, double maxMagnitude)
			: this(GRandom.Source, minMagnitude, maxMagnitude)
		{ }

		public VectorDistribution(double minMagnitude, double maxMagnitude, Angle direction)
			: this(GRandom.Source, minMagnitude, maxMagnitude, direction)
		{ }

		public VectorDistribution(double minMagnitude, double maxMagnitude, Angle minDirection, Angle maxDirection)
			: this(GRandom.Source, minMagnitude, maxMagnitude, minDirection, maxDirection)
		{ }

		public VectorDistribution(Random rnd, double magnitude)
		: this(new ConstantDistribution<double>(magnitude), new AngleDistribution(rnd))
		{ }

		public VectorDistribution(Random rnd, double magnitude, Angle minDirection, Angle maxDirection)
			: this (new ConstantDistribution<double>(magnitude), new AngleDistribution(rnd, minDirection, maxDirection))
		{ }

		public VectorDistribution(Random rnd, double minMagnitude, double maxMagnitude)
			: this(new RadialDistribution(rnd, minMagnitude, maxMagnitude), new AngleDistribution(rnd))
		{ }

		public VectorDistribution(Random rnd, double minMagnitude, double maxMagnitude, Angle direction)
#warning This throws an unexpected exception - make sure to validate arguments
			: this(new RadialDistribution(rnd, minMagnitude, maxMagnitude), new ConstantDistribution<Angle>(direction)) 
		{ }

		public VectorDistribution(Random rnd, double minMagnitude, double maxMagnitude, Angle minDirection, Angle maxDirection)
			: this(new RadialDistribution(rnd, minMagnitude, maxMagnitude), new AngleDistribution(rnd, minDirection, maxDirection)) { }

		private VectorDistribution(IDistribution<double> radiusDistribution, IDistribution<Angle> angleDistribution)
		{
			this._radius = radiusDistribution;
			this._angle = angleDistribution;
		}

		public Vector Generate()
		{
			return new Vector(_radius.Generate(), _angle.Generate());
		}
	}
}
