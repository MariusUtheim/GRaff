using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker.MathX
{
    public static class NumberTheory
    {

		public static int GCD(int a, int b)
		{
			if (a == b)
				return a;
			if (a == 0)
				return b;
			if (b == 0)
				return a;

			if (a % 2 == 0)
			{
				if (b % 2 == 1)
					return GCD(a >> 1, b);
				else
					return GCD(a >> 1, b >> 1) << 1;
			}
			else if (b % 2 == 0)
				return GCD(a, b >> 1);

			if (a > b)
				return GCD((a - b) >> 1, b);
			else
				return GCD(a, (b - a) >> 1);
		}

		public static int EulerPhi(int n)
		{
			throw new NotImplementedException();
		}

		public static IEnumerable<int> Primes(int maxn)
		{
			bool[] taken = new bool[maxn];
			int ubound = (int)Math.Sqrt(maxn);
			for (int i = 2; i <= ubound; i++)
			{
				while (taken[i])
					i++;
				yield return i;
				for (int j = 2 * i; j < maxn; j += i)
					taken[j] = true;
			}

			for (int i = ubound + 1; i < maxn; i++)
				if (!taken[i])
					yield return i;
		}
    }
}
