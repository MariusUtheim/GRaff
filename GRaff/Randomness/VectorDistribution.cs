using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public class VectorDistribution : IDistribution<Vector>
	{
		private readonly IDistribution<double> _radius;
		private readonly IDistribution<Angle> _angle;

		public VectorDistribution(double magnitude)
			: this(GRandom.Source, magnitude)
		{ }

		public VectorDistribution(double magnitude, Angle minDirection, Angle maxDirection)
			: this(GRandom.Source, magnitude, minDirection, maxDirection)
		{ }

		public VectorDistribution(Random rnd, double magnitude)
			: this(new ConstantDistribution<double>(magnitude), new AngleDistribution(rnd))
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
		}

		public VectorDistribution(Random rnd, double magnitude, Angle minDirection, Angle maxDirection)
			: this(new ConstantDistribution<double>(magnitude), new AngleDistribution(rnd, minDirection, maxDirection))
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
		}

		public VectorDistribution(double minMagnitude, double maxMagnitude)
			: this(GRandom.Source, minMagnitude, maxMagnitude)
		{
			Contract.Requires<ArgumentOutOfRangeException>(minMagnitude >= 0 && maxMagnitude >= minMagnitude);
		}

		public VectorDistribution(double minMagnitude, double maxMagnitude, Angle direction)
			: this(GRandom.Source, minMagnitude, maxMagnitude, direction)
		{
			Contract.Requires<ArgumentOutOfRangeException>(minMagnitude >= 0 && maxMagnitude >= minMagnitude);
		}

		public VectorDistribution(double minMagnitude, double maxMagnitude, Angle minDirection, Angle maxDirection)
			: this(GRandom.Source, minMagnitude, maxMagnitude, minDirection, maxDirection)
		{
			Contract.Requires<ArgumentOutOfRangeException>(minMagnitude >= 0 && maxMagnitude >= minMagnitude);
		}


		public VectorDistribution(Random rnd, double minMagnitude, double maxMagnitude)
			: this(new RadialDistribution(rnd, minMagnitude, maxMagnitude), new AngleDistribution(rnd))
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(minMagnitude >= 0 && maxMagnitude >= minMagnitude);
		}

		public VectorDistribution(Random rnd, double minMagnitude, double maxMagnitude, Angle direction)
			: this(new RadialDistribution(rnd, minMagnitude, maxMagnitude), new ConstantDistribution<Angle>(direction)) 
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(minMagnitude >= 0 && maxMagnitude >= minMagnitude);
		}

		public VectorDistribution(Random rnd, double minMagnitude, double maxMagnitude, Angle minDirection, Angle maxDirection)
			: this(new RadialDistribution(rnd, minMagnitude, maxMagnitude), new AngleDistribution(rnd, minDirection, maxDirection))
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentOutOfRangeException>(minMagnitude >= 0 && maxMagnitude >= minMagnitude);
		}

		public VectorDistribution(IDistribution<double> radiusDistribution, IDistribution<Angle> angleDistribution)
		{
			Contract.Requires<ArgumentNullException>(radiusDistribution != null && angleDistribution != null);
			this._radius = radiusDistribution;
			this._angle = angleDistribution;
		}

		public Vector Generate()
		{
			return new Vector(_radius.Generate(), _angle.Generate());
		}
	}
}
