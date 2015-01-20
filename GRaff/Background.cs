using System;
using System.Runtime.InteropServices;
using GRaff.Graphics;

namespace GRaff
{
	public sealed class Background : GameElement
	{
		private TexturedRenderSystem _renderSystem;

		public Background()
		{
			Depth = Int32.MaxValue;
			_renderSystem = new TexturedRenderSystem();
			_renderSystem.SetColors(UsageHint.StaticDraw, Color.White, Color.White, Color.White, Color.White);
			Color = Color.LightGray;
			DrawColor = true;
		}

		internal static void Initialize()
		{
			Default = Instance.Create(new Background { Color = Color.LightGray });
		}

		public static Background Default { get; private set; }

		public Color Color { get; set; }

		public Sprite Sprite { get; set; }

		public bool IsTiled { get; set; }

		public bool DrawColor { get; set; }

		public double XOffset { get; set; }
		public double YOffset { get; set; }
		public Vector Offset
		{
			get { return new Vector(XOffset, YOffset); }
			set { XOffset = value.X; YOffset = value.Y; }
		}

		public double HSpeed { get; set; }
		public double VSpeed { get; set; }
		public Vector Velocity
		{
			get { return new Vector(HSpeed, VSpeed); }
			set { HSpeed = value.X; VSpeed = value.Y; }
		}

		public override void OnDraw()
		{
			if (DrawColor)
				Draw.Clear(Color);

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
