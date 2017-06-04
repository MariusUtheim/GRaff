using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff.Graphics
{
	public sealed class AnimationStrip
	{
		private readonly Texture[] _frames;
		private readonly int[] _indices;
		private readonly double[] _durations;

		public AnimationStrip(IEnumerable<Texture> frames)
		{
			Contract.Requires<ArgumentNullException>(frames != null);
			Contract.Requires<ArgumentException>(frames.Count() > 0);
			Contract.Requires<ArgumentException>(Contract.ForAll(frames, frame => frame != null));

			_frames = frames.ToArray();
			_indices = Enumerable.Range(0, _frames.Length).ToArray();
			_durations = Enumerable.Repeat(1.0, _frames.Length).ToArray();
		}

		public AnimationStrip(params Texture[] frames)
			: this(frames.AsEnumerable())
		{
			Contract.Requires<ArgumentNullException>(frames != null);
			Contract.Requires<ArgumentException>(frames.AsEnumerable().Count() > 0);
			Contract.Requires<ArgumentException>(Contract.ForAll(frames, frame => frame != null));
		}

		public AnimationStrip(IEnumerable<Texture> frames, IEnumerable<(int index, double duration)> frameDurations)
		{
			Contract.Requires<ArgumentNullException>(frames != null && frameDurations != null);
			Contract.Requires<ArgumentException>(frames.Count() > 0 && frameDurations.Count() > 0);
			Contract.Requires<ArgumentOutOfRangeException>(Contract.ForAll(frameDurations, f => f.Item1 >= 0 && f.Item1 < frames.Count() && f.Item2 > 0));
			_frames = frames.ToArray();
			_indices = frameDurations.Select(f => f.index).ToArray();
			_durations = frameDurations.Select(f => f.duration).ToArray();
		}

		public AnimationStrip(TextureBuffer strip, int imageCount, IEnumerable<(int index, double duration)> frameDurations)
		{
			double dw = 1.0 / imageCount;
			_frames = Enumerable.Range(0, imageCount)
								.Select(i => strip.Subtexture(new Rectangle(i * dw, 0, dw, 1.0)))
								.ToArray();
			_indices = frameDurations.Select(f => f.index).ToArray();
			_durations = frameDurations.Select(f => f.duration).ToArray();
		}

		public AnimationStrip(TextureBuffer strip, int imageCount)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCount >= 1);

			double dw = 1.0 / imageCount;
			_frames = Enumerable.Range(0, imageCount)
								.Select(i => strip.Subtexture(new Rectangle(i * dw, 0, dw, 1.0)))	
								.ToArray();
			_indices = Enumerable.Range(0, imageCount).ToArray();
			_durations = Enumerable.Repeat(1.0, imageCount).ToArray();
		}

		public AnimationStrip(TextureBuffer strip, IntVector imageCounts)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCounts.X >= 1 && imageCounts.Y >= 1);

			int c = imageCounts.X, r = imageCounts.Y; ;
			double dw = 1.0 / c, dh = 1.0 / r;
			_frames = Enumerable.Range(0, c * r)
								.Select(i => strip.Subtexture(new Rectangle((i % c) * dw, (i / c) * dh, dw, dh)))
								.ToArray();
			_indices = Enumerable.Range(0, c * r).ToArray();
			_durations = Enumerable.Repeat(1.0, c * r).ToArray();
		}


		public AnimationStrip(TextureBuffer strip, IntVector imageCounts, IEnumerable<(int index, double duration)> frameDurations)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCounts.X >= 1 && imageCounts.Y >= 1);

			int c = imageCounts.X, r = imageCounts.Y; ;
			double dw = 1.0 / c, dh = 1.0 / r;
			_frames = Enumerable.Range(0, c * r)
								.Select(i => strip.Subtexture(new Rectangle((i % c) * dw, (i / c) * dh, dw, dh)))
								.ToArray();
			_indices = frameDurations.Select(f => f.index).ToArray();
			_durations = frameDurations.Select(f => f.duration).ToArray();
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(_frames != null);
			Contract.Invariant(_frames.Length > 0);
			Contract.Invariant(_indices != null);
			Contract.Invariant(_indices.Length > 0);
			Contract.Invariant(Contract.ForAll(_indices, i => i >= 0 && i < _frames.Length));
			Contract.Invariant(_durations != null);
			Contract.Invariant(_durations.Length > 0);
			Contract.Invariant(Contract.ForAll(_durations, d => d > 0));
			Contract.Invariant(_indices.Length == _durations.Length);
			Contract.Invariant(ImageCount >= 1);
			Contract.Invariant(Duration > 0);
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

		public Texture Frame(int index)
		{
			Contract.Ensures(Contract.Result<Texture>() != null);
			return _frames[GMath.Remainder(index, _frames.Length)];
		}

		public Texture SubImage(double dt)
		{
			Contract.Ensures(Contract.Result<Texture>() != null);
			dt = GMath.Remainder(dt, Duration);
			for (int i = 0; i < _indices.Length; i++)
			{
				dt -= _durations[i];
				if (dt < 0)
					return _frames[_indices[i]];
			}

			throw new NotSupportedException($"{nameof(GRaff)}.{nameof(Graphics)}.{nameof(AnimationStrip)}.{nameof(SubImage)} did not return a texture. This indicates an internal error with G-Raff.");
		}
	}
}
