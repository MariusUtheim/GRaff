using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for angles according to a uniform distribution.
    /// </summary>
	public sealed class AngleDistribution : IDistribution<Angle>
	{
		private readonly Random _rnd;
		private readonly bool _hasRange;
		private readonly Angle _minAngle, _maxAngle;

		public AngleDistribution()
			: this(GRandom.Source) { }

		public AngleDistribution(Random rnd)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			this._rnd = rnd;
			_hasRange = false;
		}

		public AngleDistribution(Angle minimum, Angle maximum)
			: this(GRandom.Source, minimum, maximum) { }

		public AngleDistribution(Random rnd, Angle minimum, Angle maximum)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			_rnd = rnd;
			_hasRange = true;
			_minAngle = minimum;
			_maxAngle = maximum;
		}

		public Angle Generate()
			=> _hasRange ? _rnd.Angle(_minAngle, _maxAngle) : _rnd.Angle();
	}
}
