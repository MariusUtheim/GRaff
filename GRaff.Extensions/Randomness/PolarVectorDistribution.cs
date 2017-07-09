using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	/// <summary>
	/// Represents a vector distribution where the magnitude and direction are independent generally distributed values.
	/// </summary>
	public class PolarVectorDistribution : IDistribution<Vector>
	{
		private IDistribution<double> _magnitudeDistribution;
		private IDistribution<Angle> _directionDistribution;

		public PolarVectorDistribution(IDistribution<double> magnitudeDistribution, IDistribution<Angle> directionDistribution)
		{
			Contract.Requires<ArgumentNullException>(magnitudeDistribution != null);
			Contract.Requires<ArgumentNullException>(directionDistribution != null);
			this._magnitudeDistribution = magnitudeDistribution;
			this._directionDistribution = directionDistribution;
		}

		public IDistribution<double> MagnitudeDistribution
		{
			get { return _magnitudeDistribution; }
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				_magnitudeDistribution = value;
			}
		}

		public IDistribution<Angle> DirectionDistribution
		{
			get { return _directionDistribution; }
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				_directionDistribution = value;
			}
		}

		public Vector Generate()
			=> new Vector(MagnitudeDistribution.Generate(), DirectionDistribution.Generate());
	}
}
