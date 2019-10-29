using System;
using GRaff.Graphics;
using GRaff.Synchronization;
#if OpenGL4
using coords = System.Double;
#else
using coords = System.Single;
#endif

namespace GRaff
{
	/// <summary>
	/// Represents an element that is drawn in the background. This can be a fill color, a sprite or both.
	/// </summary>
	public sealed class Background : GameElement
	{

        /// <summary>
        /// Creates a new instance of the GRaff.Background class with no sprite or color.
        /// </summary>
        public Background()
		{
			Depth = Int32.MaxValue;
		}

        /// <summary>
        /// Creates a new instance of the GRaff.Background class with the specified color.
        /// </summary>
        /// <param name="color">The color of the background.</param>
        public Background(Color color)
            : this()
        {
            this.Color = color;
        }

        /// <summary>
        /// Creates a new instance of the GRaff.Background class with the specified texture.
        /// </summary>
        /// <param name="texture">The texture of the background.</param>
        /// <param name="isRepeated">Whether the background should draw the texture repeated.</param>
        public Background(Texture texture, bool isRepeated = true)
        {
            this.Texture = texture;
            this.IsRepeated = isRepeated;
        }

		/// <summary>
		/// Gets or sets the background clear color. If set to null, the background won't draw a color.
		/// </summary>
		public Color? Color { get; set; }

		/// <summary>
		/// Gets or sets the sprite for this GRaff.Background.
		/// </summary>
		public Texture? Texture { get; set; }

		/// <summary>
		/// Gets or sets whether this GRaff.Background should draw its sprite repeated.
		/// </summary>
		public bool IsRepeated { get; set; }

		/// <summary>
		/// Gets or sets the horizontal offset of the sprite drawn by this GRaff.Background.
		/// </summary>
		public double XOffset { get; set; }

		/// <summary>
		/// Gets or sets the vertical offset of the sprite drawn by this GRaff.Background.
		/// </summary>
		public double YOffset { get; set; }

		/// <summary>
		/// Gets or sets the offset of the sprite drawn by this GRaff.Background.
		/// </summary>
		public Vector Offset
		{
			get { return new Vector(XOffset, YOffset); }
			set { XOffset = value.X; YOffset = value.Y; }
		}

		/// <summary>
		/// Gets or sets the horizontal speed of this GRaff.Background. Each step, this value will be added to the XOffset value of this GRaff.Background.
		/// </summary>
		public double HSpeed { get; set; }

		/// <summary>
		/// Gets or sets the vertical speed of this GRaff.Background. Each step, this value will be added to the YOffset value of this GRaff.Background.
		/// </summary>
		public double VSpeed { get; set; }

		/// <summary>
		/// Gets or sets the velocity of this GRaff.Background. Each step, this value will be added to the Offset value of this GRaff.Background.
		/// </summary>
		public Vector Velocity
		{
			get { return new Vector(HSpeed, VSpeed); }
			set { HSpeed = value.X; VSpeed = value.Y; }
		}

		/// <summary>
		/// Overrides GameElement.OnStep to enable background motion.
		/// </summary>
		public override void OnStep()
		{
			XOffset += HSpeed;
			YOffset += VSpeed;
		}

		/// <summary>
		/// Overrides GameElement.OnDraw to perform optimized drawing of this GRaff.Background 
		/// </summary>
		public override void OnDraw()
		{
			if (Color != null)
				Draw.Clear(Color.Value);

			if (Texture != null)
			{
				if (IsRepeated)
				{
					var viewBox = View.Current.BoundingBox;
					coords u0 = -(coords)(XOffset + viewBox.Left) / Texture.Width, v0 = -(coords)((YOffset + viewBox.Top) / Texture.Height);
					coords u1 = (coords)(u0 + viewBox.Width / Texture.Width), v1 = (coords)(v0 + viewBox.Height / Texture.Height);

					coords left = (coords)viewBox.Left, right = (coords)viewBox.Right, top = (coords)viewBox.Top, bottom = (coords)viewBox.Bottom;

                    Draw.Primitive(PrimitiveType.TriangleStrip, Texture, new[]
                    {
                        (new Point(left, top), new Point(u0, v0)),
                        (new Point(right, top), new Point(u1, v0)),
						(new Point(left, bottom), new Point(u0, v1)),
                        (new Point(right, bottom), new Point(u1, v1)),
                    }, Colors.White);
                    
				}
				else
                    Draw.Texture(Texture, (XOffset, YOffset));
			}
		}
	}
}
