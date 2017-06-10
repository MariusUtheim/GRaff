using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public sealed class FiniteDistribution<T> : IDistribution<T>
	{
		private readonly Random _rnd;
		private readonly T[] _elements;

		public FiniteDistribution(IEnumerable<T> elements)
			: this(GRandom.Source, elements)
		{
			Contract.Requires<ArgumentNullException>(elements != null);
			Contract.Requires<ArgumentException>(elements.Any());
		}

		public FiniteDistribution(params T[] elements)
			: this(GRandom.Source, (IEnumerable<T>)elements)
		{
			Contract.Requires<ArgumentNullException>(elements != null);
			Contract.Requires<ArgumentException>(elements.Any());
		}

		public FiniteDistribution(Random rnd, params T[] elements)
			: this(rnd, (IEnumerable<T>)elements)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentNullException>(elements != null);
			Contract.Requires<ArgumentException>(elements.Any());
		}

		public FiniteDistribution(Random rnd, IEnumerable<T> elements)
		{
			Contract.Requires<ArgumentNullException>(rnd != null);
			Contract.Requires<ArgumentNullException>(elements != null);
			Contract.Requires<ArgumentException>(elements.Any());
			_rnd = rnd;
			_elements = elements.ToArray();
		}

		public T Generate()
		{
			return _rnd.Choose(_elements);
		}
	}
}
