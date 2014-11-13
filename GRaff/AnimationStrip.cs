using System;
using System.Diagnostics;
using System.Linq;

namespace GRaff
{
	public sealed class AnimationStrip
	{
		Tuple<int, double>[] _images;

		public AnimationStrip(int uniformImageCount)
		{
			_images = new Tuple<int, double>[uniformImageCount];
			for (int i = 0; i < uniformImageCount; i++)
				_images[i] = new Tuple<int, double>(i, 1.0);
		}

		public AnimationStrip(params Tuple<int, double>[] images)
		{
			_images = (Tuple < int, double>[])images.Clone();
		}


		public double Duration { get; private set; }

		public int FrameCount { get; private set; }

		public int GetFrameIndex(double dt)
		{
			dt = ((dt % Duration) + Duration) % Duration;
			double sum = 0;
			for (int i = 0; i < _images.Length; i++)
			{
				sum += _images[i].Item2;
				if (sum > dt)
					return _images[i].Item1;
			}

			Debug.Fail("Unreachable code was reached.");
			throw new NotSupportedException("Supposedly unreachable code was reached. This indicates a bug in G-Raff.");
		}
	}
}
