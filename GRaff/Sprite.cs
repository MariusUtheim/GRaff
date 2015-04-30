using System.Diagnostics.Contracts;
using GRaff.Synchronization;

namespace GRaff
{
	public sealed class Sprite
	{
		private Vector? _origin;
		private MaskShape _maskShape;

		public Sprite(AnimationStrip animationStrip, Vector? size = null, Vector? origin = null, MaskShape maskShape = null)
		{
			Contract.Requires(animationStrip != null);

			this.AnimationStrip = animationStrip;
			this.Size = size ?? animationStrip.SubImage(0).Size;
			this._origin = origin;
			this._maskShape = maskShape;
		}
		
		public static Sprite Load(string path, int imageCount = 1, Vector? origin = null, MaskShape maskShape = null) 
			=> new Sprite(new AnimationStrip(TextureBuffer.Load(path), imageCount), null, origin, maskShape);

		public static IAsyncOperation<Sprite> LoadAsync(string path, int imageCount = 1, Vector? origin = null, MaskShape maskShape = null)
			=> TextureBuffer.LoadAsync(path).Then(buffer => new Sprite(new AnimationStrip(buffer, imageCount), null, origin, maskShape));

		public AnimationStrip AnimationStrip { get; private set; }

		public Vector Size { get; private set; }

		public double Width => Size.X;

		public double Height => Size.Y;


		public Vector Origin 
			=> _origin ?? new Vector(Width / 2, Height / 2);

		public double XOrigin 
			=> Origin.X;

		public double YOrigin
			=> Origin.Y;

		public MaskShape MaskShape
		{
			get
			{
				if (_maskShape == MaskShape.Automatic)
					return MaskShape.Rectangle(Width, Height);
				else
					return _maskShape;
			}

			set
			{
				_maskShape = value;
			}
		}

		public Texture SubImage(double dt)
			=> AnimationStrip.SubImage(dt);
	}
}
