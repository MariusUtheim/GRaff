using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public abstract class Surface
	{
		public abstract void DrawImage(double x, double y, Image image);

		public abstract void DrawTexture(double x, double y, Texture texture);

		public abstract void DrawCircle(Color color, Point location, double radius);

		public abstract void FillCircle(Color color, Point location, double radius);

		public abstract void DrawRectangle(Color color, double x, double y, double width, double height);

		public abstract void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height);

		public abstract void FillRectangle(Color color, double x, double y, double width, double height);

		public abstract void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height);

		public abstract void DrawLine(Color color, double x1, double y1, double x2, double y2);

		public abstract void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2);

		public abstract void DrawText(Color color, Object text, double x, double y);

		public abstract void Clear(Color color);

		public abstract void Blit(Surface dest, IntRectangle srcRect, IntRectangle destRect);
	}
}
