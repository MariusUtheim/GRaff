using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff
{
	public sealed class AnimationStrip
	{
		private Texture[] _frames;
		private int[] _indices;
		private double[] _durations;

		public AnimationStrip(IEnumerable<Texture> frames)
		{
			Contract.Requires(frames != null && frames.Count() > 0);

			_frames = frames.ToArray();
			_indices = Enumerable.Range(0, _frames.Length).ToArray();
			_durations = Enumerable.Repeat(1.0, _frames.Length).ToArray();
		}

		public AnimationStrip(params Texture[] frames)
			: this(frames.AsEnumerable())
		{ }

		public AnimationStrip(IEnumerable<Texture> frames, params Tuple<int, double>[] frameDurations)
		{
			Contract.Requires(frames != null && frames.Count() > 0);
			Contract.Requires(frameDurations != null && frameDurations.Length > 0);
			Contract.ForAll(frameDurations, f => f.Item1 < frames.Count() && f.Item2 > 0);

			_frames = frames.ToArray();
			_indices = frameDurations.Select(f => f.Item1).ToArray();
			_durations = frameDurations.Select(f => f.Item2).ToArray();
		}

		public AnimationStrip(TextureBuffer strip, int imageCount)
		{
			Contract.Requires(strip != null);
			Contract.Requires(imageCount >= 1);

			double dw = 1.0 / imageCount;
			_frames = Enumerable.Range(0, imageCount)
								.Select(i => strip.Subtexture(new Rectangle(i * dw, 0, dw, 1.0)))	
								.ToArray();
			_indices = Enumerable.Range(0, imageCount).ToArray();
			_durations = Enumerable.Repeat(1.0, imageCount).ToArray();
		}

		public AnimationStrip InOut()
		{
			var frames = new Texture[2 * _frames.Length];
			
			for (int i = 0; i < _frames.Length; i++)
			{
				frames[i] = _frames[i];
				frames[i + _frames.Length] = _frames[_frames.Length - 1 - i];
			}

			return new AnimationStrip(frames);
		}

		public double Duration => _durations.Sum();

		public int ImageCount => _indices.Length;

		public Texture Frame(int index) => _frames[GMath.Remainder(index, _frames.Length)];

		public Texture SubImage(double dt)
		{
			dt = GMath.Remainder(dt, Duration);
			for (int i = 0; i < _indices.Length; i++)
			{
				dt -= _durations[i];
				if (dt <= 0)
					return _frames[_indices[i]];
			}

			throw new NotSupportedException("\{nameof(GRaff)}.\{nameof(AnimationStrip)}.\{nameof(SubImage)} did not return a texture. This indicates an internal error with G-Raff.");
		}
	}
}
