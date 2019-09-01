using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for a vector distribution where the magnitude and direction are independently distributed values.
    /// </summary>
    public class PolarVectorDistribution : IDistribution<Vector>
	{

        public PolarVectorDistribution(double magnitude)
            : this(new ConstantDistribution<double>(magnitude), new AngleDistribution())
        { }

        public PolarVectorDistribution(double magnitude, IDistribution<Angle> directionDistribution)
            : this(new ConstantDistribution<double>(magnitude), directionDistribution)
        { }

        public PolarVectorDistribution(double minMagnitude, double maxMagnitude)
            : this(new DoubleDistribution(minMagnitude, maxMagnitude), new AngleDistribution())
        { }

        public PolarVectorDistribution(double minMagnitude, double maxMagnitude, IDistribution<Angle> directionDistribution)
            : this(new DoubleDistribution(minMagnitude, maxMagnitude), directionDistribution)
        { }

        public PolarVectorDistribution(Random rnd, double magnitude)
            : this(new ConstantDistribution<double>(magnitude), new AngleDistribution(rnd))
        { }

        public PolarVectorDistribution(Random rnd, double minMagnitude, double maxMagnitude)
            : this(new DoubleDistribution(rnd, minMagnitude, maxMagnitude), new AngleDistribution())
        { }

        public PolarVectorDistribution(Random rnd, double minMagnitude, double maxMagnitude, IDistribution<Angle> directionDistribution)
            : this(new DoubleDistribution(rnd, minMagnitude, maxMagnitude), directionDistribution)
        { }


        public PolarVectorDistribution(IDistribution<double> magnitudeDistribution)
            : this(magnitudeDistribution, new AngleDistribution())
        { }

		public PolarVectorDistribution(IDistribution<double> magnitudeDistribution, IDistribution<Angle> directionDistribution)
		{
			Contract.Requires<ArgumentNullException>(magnitudeDistribution != null);
			Contract.Requires<ArgumentNullException>(directionDistribution != null);
			this.MagnitudeDistribution = magnitudeDistribution;
			this.DirectionDistribution = directionDistribution;
		}

		public IDistribution<double> MagnitudeDistribution { get; set; }

		public IDistribution<Angle> DirectionDistribution { get; set; }

		public Vector Generate()
			=> new Vector(MagnitudeDistribution.Generate(), DirectionDistribution.Generate());
	}
}
