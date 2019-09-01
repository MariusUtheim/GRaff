using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	/// <summary>
	/// Generator for vectors where the x and y coordinates are independently distributed values.
	/// </summary>
	public class CartesianVectorDistribution : IDistribution<Vector>
	{

		public CartesianVectorDistribution(IDistribution<double> xDistribution, IDistribution<double> yDistribution)
		{
			this.XDistribution = xDistribution;
			this.YDistribution = yDistribution;
		}

        public CartesianVectorDistribution(double x, double y)
            : this(new ConstantDistribution<double>(x), new ConstantDistribution<double>(y))
        { }

        public CartesianVectorDistribution(double x, (double min, double max) y)
            : this(new ConstantDistribution<double>(x), new DoubleDistribution(y.min, y.max))
        { }
        public CartesianVectorDistribution((double min, double max) x, double y)
            : this(new DoubleDistribution(x.min, x.max), new ConstantDistribution<double>(y))
        { }
        public CartesianVectorDistribution((double min, double max) x, (double min, double max) y)
            : this(new DoubleDistribution(x.min, x.max), new DoubleDistribution(y.min, y.max))
        { }

        public CartesianVectorDistribution(Random rnd, double x, (double min, double max) y)
            : this(new ConstantDistribution<double>(x), new DoubleDistribution(rnd, y.min, y.max))
        { }

        public CartesianVectorDistribution(Random rnd, (double min, double max) x, double y)
            : this(new DoubleDistribution(rnd, x.min, x.max), new ConstantDistribution<double>(y))
        { }
        public CartesianVectorDistribution(Random rnd, (double min, double max) x, (double min, double max) y)
            : this(new DoubleDistribution(rnd, x.min, x.max), new DoubleDistribution(rnd, y.min, y.max))
        { }

        public IDistribution<double> XDistribution { get; set; }

		public IDistribution<double> YDistribution { get; set; }

		public Vector Generate() => new Vector(XDistribution.Generate(), YDistribution.Generate());
	}
}
