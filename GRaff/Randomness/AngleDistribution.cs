using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
	public sealed class AngleDistribution : IDistribution<Angle>
	{
		private Random _rnd;
		private bool _hasRange;
		Angle _minAngle, _maxAngle;

		public AngleDistribution()
			: this(GRandom.Source) { }

		public AngleDistribution(Random rnd)
		{
			if (rnd == null)
				throw new ArgumentNullException("rnd");
			this._rnd = rnd;
			_hasRange = false;
		}

		public AngleDistribution(Angle minimum, Angle maximum)
			: this(GRandom.Source, minimum, maximum) { }

		public AngleDistribution(Random rnd, Angle minimum, Angle maximum)
		{
			Contract.Requires(rnd != null);
			_rnd = rnd;
			_hasRange = true;
			_minAngle = minimum;
			_maxAngle = maximum;
		}

		public Angle Generate()
			=> _hasRange ? _rnd.Angle(_minAngle, _maxAngle) : _rnd.Angle();
	}
}
