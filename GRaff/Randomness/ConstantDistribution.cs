using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public sealed class ConstantDistribution<T> : IDistribution<T>
		where T : struct
	{
		T _constantValue;

		public ConstantDistribution(T constantValue)
		{
			_constantValue = constantValue;
		}

		public T Generate()
		{
			return _constantValue;
		}
	}
}
