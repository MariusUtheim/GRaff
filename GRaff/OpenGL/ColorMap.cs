using OpenTK.Graphics.ES30;


namespace GRaff.OpenGL
{
	public static class ColorMap
	{
		private static BlendEquation _blendMode;

		public static BlendEquation BlendMode
		{
			get
			{
				return _blendMode;
			}

			set
			{
				_blendMode = value;
				GL.BlendEquation((BlendEquationMode)value);
			}
		}

	}
}
