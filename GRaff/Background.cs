using System;
using GRaff.Graphics;


namespace GRaff
{
	/// <summary>
 /// Represents an element that is drawn in the background. This can be a fill color, a sprite or both.
 /// </summary>
	public sealed class Background : GameElement
	{
		private TexturedRenderSystem _renderSystem;

		/// <summary>
  /// Creates a new instance of the GRaff.Background class with no sprite or color.
  /// </summary>
		public Background()
		{
			Depth = Int32.MaxValue;
			_renderSystem = new TexturedRenderSystem();
			_renderSystem.SetColors(UsageHint.StaticDraw, Color.White, Color.White, Color.White, Color.White);
		}

		internal static void Initialize()
		{
			Default = Instance.Create(new Background { ClearColor = Color.LightGray });
		}

		/// <summary>
		/// Gets the GRaff.Background that was initialized at the beginning of the game.
		/// </summary>
		/// <remarks>This is a background with no sprite and a clear color set to Color.LightGray.</remarks>
		public static Background Default { get; private set; }

		/// <summary>
		/// Gets or sets the background clear color. If set to null, the background won't draw a color.
		/// </summary>
		public Color? ClearColor { get; set; }

		/// <summary>
  /// Gets or sets the sprite for this GRaff.Background.
  /// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
  /// Gets or sets whether this GRaff.Background should draw its sprite tiled.
  /// </summary>
		public bool IsTiled { get; set; }

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
			if (ClearColor != null)
				Draw.Clear(ClearColor.Value);

			if (Sprite != null)
			{
				if (IsTiled)
				{
					float u0 = -(float)(XOffset / Sprite.Width), v0 = -(float)(YOffset / Sprite.Height);
					float u1 = u0 + Room.Width / (float)Sprite.Width, v1 = v0 + Room.Height / (float)Sprite.Height;

					_renderSystem.SetVertices(UsageHint.StreamDraw, 0.0f, 0.0f, Room.Width, 0.0f, Room.Width, Room.Height, 0.0f, Room.Height);
					_renderSystem.SetTexCoords(UsageHint.StreamDraw, u0, v0, u1, v0, u1, v1, u0, v1);

					Sprite.Texture.Bind();
					ShaderProgram.CurrentTextured.SetCurrent();

					_renderSystem.Render(PrimitiveType.Quads, 4);
				}
				else
					Draw.Sprite(Sprite, 0, XOffset, YOffset);
			}
		}
	}
}
