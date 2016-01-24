﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public static class Statistics
	{
		
		private static double _directMedian(double[] items)
		{
			items = items.OrderBy(d => d).ToArray();
			return items[items.Length / 2];
		}
		private static double _median(double[] items)
		{
			if (items.Length == 1)
				return items[0];

			var n = items.Length / 5;
			var r = items.Length % 5;
			var subRange = new double[5];
			var aggregate = new double[n + (r > 0 ? 1 : 0)];
			
			for (var i = 0; i < n; i++)
			{
				Array.Copy(items, 5 * i, subRange, 0, 5);
				aggregate[i] = _directMedian(subRange);
			}

			if (r > 0)
			{
				subRange = new double[r];
				Array.Copy(items, 5 * n, subRange, 0, r);
				aggregate[n] = _directMedian(subRange);
			}

			return _median(aggregate);
		}
		public static double Median(params double[] items)
		{
			return _median(items);
		}
	}
}