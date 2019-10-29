using System;
using System.Linq;
using GRaff.Synchronization;

namespace GRaff.Graphics
{
    public sealed class Sprite
	{
		private readonly Vector? _origin;
		private readonly Mask _maskShape;

		public Sprite(AnimationStrip animationStrip, Vector? size = null, Vector? origin = null, Mask? maskShape = null)
		{
			this.AnimationStrip = animationStrip;
			this.Size = size ?? animationStrip.SubImage(0).Size;
			this._origin = origin;
			this._maskShape = maskShape ?? Mask.Automatic;
		}

        public Sprite(Texture texture, Vector? size = null, Vector? origin = null, Mask? maskShape = null)
            : this(new SubTexture(texture), size, origin, maskShape)
        { }

		public Sprite(SubTexture texture, Vector? size = null, Vector? origin = null, Mask? maskShape = null)
		{
			this.AnimationStrip = new AnimationStrip(texture);
			this.Size = size ?? texture.Size;
			this._origin = origin;
			this._maskShape = maskShape ?? Mask.Automatic;
		}

		public static Sprite Load(string path, int imageCount = 1, Vector? origin = null, Mask? maskShape = null)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCount >= 1);
			return new Sprite(new AnimationStrip(Texture.Load(path), imageCount), null, origin, maskShape);
		}

		public static Sprite Load(string path, IntVector imageCounts, Vector? origin = null, Mask? maskShape = null)
		{
			Contract.Requires<ArgumentOutOfRangeException>(imageCounts.X >= 1 && imageCounts.Y >= 1);
			return new Sprite(new AnimationStrip(Texture.Load(path), imageCounts), null, origin, maskShape);
		}

		public static IAsyncOperation<Sprite> LoadAsync(string path, int imageCount = 1, Vector? origin = null, Mask? maskShape = null)
			=> Texture.LoadAsync(path).ThenQueue(buffer => new Sprite(new AnimationStrip(buffer, imageCount), null, origin, maskShape));

		public static IAsyncOperation<Sprite> LoadAsync(string path, IntVector imageCounts, Vector? origin = null, Mask? maskShape = null)
			=> Texture.LoadAsync(path).ThenQueue(buffer => new Sprite(new AnimationStrip(buffer, imageCounts), null, origin, maskShape));



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

#warning
        public Polygon? GetMaskPolygon(double subImage = 0)
        {
            if (Mask.ReferenceEquals(_maskShape, Mask.None))
                return null;
            else
                return MaskShape.Polygon;
        }

		public Mask MaskShape
			=> _maskShape == Mask.Automatic ? Mask.Rectangle(Width, Height) : _maskShape;

		public SubTexture SubImage(double dt)
			=> AnimationStrip.SubImage(dt);
	}
}
