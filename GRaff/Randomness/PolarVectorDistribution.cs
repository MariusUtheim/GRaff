using System;

 
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
			if (magnitudeDistribution == null) throw new ArgumentNullException("magnitudeDistribution");  /*C#6.0*/
			if (directionDistribution == null) throw new ArgumentNullException("directionDistribution"); 
			this._magnitudeDistribution = magnitudeDistribution;
			this._directionDistribution = directionDistribution;
		}

		public IDistribution<double> MagnitudeDistribution
		{
			get { return _magnitudeDistribution; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				_magnitudeDistribution = value;
			}
		}

		public IDistribution<Angle> DirectionDistribution
		{
			get { return _directionDistribution; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				_directionDistribution = value;
			}
		}

		public Vector Generate()
		{
			return new Vector(MagnitudeDistribution.Generate(), DirectionDistribution.Generate());
		}
	}
}
