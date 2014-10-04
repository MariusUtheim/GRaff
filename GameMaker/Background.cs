using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;


namespace GRaff
{
	public static class Background
	{
		private static int _vertexArray;
		private static int _vertexBuffer, _colorBuffer, _textureBuffer;
		private static readonly IntPtr _4 = new IntPtr(4 * Marshal.SizeOf(typeof(Point)));

		static Background()
		{
			_vertexArray = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArray);

			_vertexBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			_colorBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(16), new[] { Color.White, Color.White, Color.White, Color.White }, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, false, 0, 0);

			_textureBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
		}

		public static Color Color { get; set; } = Color.LightGray;

		public static Sprite Sprite { get; set; } = null;

		public static bool IsTiled { get; set; } = false;

		public static bool DrawColor { get; set; } = true;

		public static double XOffset { get; set; }
		public static double YOffset { get; set; }
		public static Vector Offset
		{
			get { return new Vector(XOffset, YOffset); }
			set { XOffset = value.X; YOffset = value.Y; }
		}

		public static double HSpeed { get; set; }
		public static double VSpeed { get; set; }
		public static Vector Velocity
		{
			get { return new Vector(HSpeed, VSpeed); }
			set { HSpeed = value.X; VSpeed = value.Y; }
		}

		public static void Redraw()
		{
			if (DrawColor)
				GL.ClearColor(new OpenTK.Graphics.Color4(Color.R, Color.G, Color.B, Color.A));

			if (Sprite != null)
			{
				if (IsTiled)
				{
					float u0 = -(float)(XOffset / Sprite.Width), v0 = -(float)(YOffset / Sprite.Height);
					float u1 = u0 + Room.Width / (float)Sprite.Width, v1 = v0 + Room.Height / (float)Sprite.Height;

					GL.BindVertexArray(_vertexArray);
					GL.BindTexture(TextureTarget.Texture2D, Sprite.Texture.Id);
					View.EnableTexture();

					GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
					GL.BufferData(BufferTarget.ArrayBuffer, _4, new double[] { 0, 0, Room.Width, 0, Room.Width, Room.Height, 0, Room.Height }, BufferUsageHint.StreamDraw);

					GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
					GL.BufferData(BufferTarget.ArrayBuffer, _4, new double[] { u0, v0, u1, v0, u1, v1, u0, v1 }, BufferUsageHint.StreamDraw);

					GL.DrawArrays(PrimitiveType.Quads, 0, 4);

					View.DisableTexture();
				}
				else
					Draw.Sprite(0, 0, Sprite, 0);
			}
		}
	}
}
