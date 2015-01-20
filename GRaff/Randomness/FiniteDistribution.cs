using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace GRaff.Randomness
{
	public sealed class FiniteDistribution<T> : IDistribution<T>
	{
		Random _rnd;
		T[] _elements;

		public FiniteDistribution(IEnumerable<T> elements)
			: this(GRandom.Source, elements) { }

		public FiniteDistribution(params T[] elements)
			: this(GRandom.Source, elements) { }

		public FiniteDistribution(Random rnd, params T[] elements)
			: this(rnd, elements.AsEnumerable()) { }

		public FiniteDistribution(Random rnd, IEnumerable<T> elements)
		{
			_rnd = rnd;
			_elements = elements.ToArray();
		}

		public T Generate()
		{
			return _rnd.Choose(_elements);
		}
	}
}
