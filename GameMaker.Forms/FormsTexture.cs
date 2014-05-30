using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker.Forms
{
	public class FormsTexture : Texture
	{

		public FormsTexture(System.Drawing.Image image)
		{
			UnderlyingImage = image;
		}


		public FormsTexture(string file)
		{
			UnderlyingImage = System.Drawing.Image.FromFile(file);
		}

		internal System.Drawing.Image UnderlyingImage { get; private set; }

		public override int Width
		{
			get { return UnderlyingImage.Width; }
		}

		public override int Height
		{
			get { return UnderlyingImage.Height; }
		}
	}
}
