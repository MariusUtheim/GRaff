﻿using System;
using System.Linq;
using GRaff.Synchronization;

namespace GRaff.Graphics
{
    public sealed class Sprite
	{
		private readonly Vector? _origin;
		private readonly MaskShape _maskShape;

		public Sprite(AnimationStrip animationStrip, Vector? size = null, Vector? origin = null, MaskShape maskShape = null)
		{
			Contract.Requires<ArgumentNullException>(animationStrip != null);

			this.AnimationStrip = animationStrip;
			this.Size = size ?? animationStrip.SubImage(0).Size;
			this._origin = origin;
			this._maskShape = maskShape ?? MaskShape.Automatic;
		}

		public Sprite(Texture texture, Vector? size = null, Vector? origin = null, MaskShape maskShape = null)
		{
			if (texture == null)
				this.AnimationStrip = new AnimationStrip(Enumerable.Empty<Texture>());
			else
				this.AnimationStrip = new AnimationStrip(texture);
			this.Size = size ?? texture.Size;
			this._origin = origin;
			this._maskShape = maskShape ?? MaskShape.Automatic;
		}

		public static Sprite Load(string path, int imageCount = 1, Vector? origin = null, MaskShape maskShape = null)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCount >= 1);
			return new Sprite(new AnimationStrip(TextureBuffer.Load(path), imageCount), null, origin, maskShape);
		}

		public static Sprite Load(string path, IntVector imageCounts, Vector? origin = null, MaskShape maskShape = null)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCounts.X >= 1 && imageCounts.Y >= 1);
			return new Sprite(new AnimationStrip(TextureBuffer.Load(path), imageCounts), null, origin, maskShape);
		}

		public static IAsyncOperation<Sprite> LoadAsync(string path, int imageCount = 1, Vector? origin = null, MaskShape maskShape = null)
			=> TextureBuffer.LoadAsync(path).ThenQueue(buffer => new Sprite(new AnimationStrip(buffer, imageCount), null, origin, maskShape));

		public static IAsyncOperation<Sprite> LoadAsync(string path, IntVector imageCounts, Vector? origin = null, MaskShape maskShape = null)
			=> TextureBuffer.LoadAsync(path).ThenQueue(buffer => new Sprite(new AnimationStrip(buffer, imageCounts), null, origin, maskShape));



		public AnimationStrip AnimationStrip { get; }

		public Vector Size { get; }

		public double Width => Size.X;

		public double Height => Size.Y;


		public Vector Origin 
			=> _origin ?? new Vector(Width / 2, Height / 2);

		public double XOrigin 
			=> Origin.X;

		public double YOrigin
			=> Origin.Y;

		public MaskShape MaskShape
			=> _maskShape == MaskShape.Automatic ? MaskShape.Rectangle(Width, Height) : _maskShape;

		public Texture SubImage(double dt)
			=> AnimationStrip.SubImage(dt);
	}
}