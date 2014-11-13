using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public sealed class Texture
	{
		public Texture(TextureBuffer buffer, double left, double bottom, double right, double top)
		{
			Buffer = buffer;
			TopLeft = new Point(left, top);
			TopRight = new Point(right, top);
			BottomLeft = new Point(left, bottom);
			BottomRight = new Point(right, bottom);
		}

		public int Id { get { return Buffer.Id; } }

		public TextureBuffer Buffer { get; private set; }

		public Point TopLeft { get; private set; }
		
		public Point TopRight { get; private set; }

		public Point BottomLeft { get; private set; }

		public Point BottomRight { get; private set; }
	}
}
