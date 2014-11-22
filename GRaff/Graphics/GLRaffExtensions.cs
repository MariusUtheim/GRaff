﻿using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	public static class GLRaffExtensions
	{
		public static void Bind(this TextureBuffer texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);
		}

		public static void Bind(this Texture texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);
		}

		public static PointF[] GetTexCoords(this Texture texture)
		{
			return new[] { texture.BottomLeft, texture.BottomRight, texture.TopLeft, texture.TopRight };
		}
	}
}
