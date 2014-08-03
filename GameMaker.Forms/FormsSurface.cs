using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace GameMaker.Forms
{
	public class FormsSurface : Surface, IDisposable
	{
		private Bitmap _bmp;

		public FormsSurface(int width, int height)
		{
			this._bmp = new Bitmap(width, height);
			this.Graphics = Graphics.FromImage(_bmp);
		}

		public FormsSurface(int width, int height, Graphics graphics)
		{
			this._bmp = null;
			this.Graphics = graphics;
		}

		public Graphics Graphics { get; internal set; }

		public override Color GetPixel(int x, int y)
		{
			return _bmp.GetPixel(x, y).ToGMColor();
		}

		public override void SetPixel(int x, int y, Color color)
		{
			Graphics.FillRectangle(new SolidBrush(color.ToFormsColor()), x, y, 1, 1);
		}

		public override void DrawImage(double x, double y, Transform transform, Image image)
		{
			float tx = (float)x, ty = (float)y;
			
			Graphics.TranslateTransform(tx, ty);
			Graphics.RotateTransform((float)transform.Rotation.Degrees);
			Graphics.ScaleTransform((float)transform.XScale, (float)transform.YScale);

			Graphics.TranslateTransform(-(float)image.Sprite.XOrigin, -(float)image.Sprite.YOrigin);

			ColorMatrix cm = new ColorMatrix();
			cm.Matrix00 = (float)image.Blend.R / 255.0f;
			cm.Matrix11 = (float)image.Blend.G / 255.0f;
			cm.Matrix22 = (float)image.Blend.B / 255.0f;
			cm.Matrix33 = (float)image.Blend.A / 255.0f;
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(cm);

			var formsTexture = image.CurrentTexture as FormsTexture;
			Graphics.DrawImage(formsTexture.UnderlyingImage, new System.Drawing.Rectangle(0, 0, image.Sprite.Width, image.Sprite.Height), 0.0f, 0.0f, (float)image.Sprite.Width, (float)image.Sprite.Height, GraphicsUnit.Pixel, attributes);

			Graphics.ResetTransform();
		}

		public override void DrawTexture(double x, double y, Texture texture)
		{
			var formsTexture = texture as FormsTexture;
			Graphics.DrawImage(formsTexture.UnderlyingImage, new RectangleF((float)x, (float)y, (float)texture.Width, (float)texture.Height));
		}

		public override void DrawCircle(Color color, Point location, double radius)
		{
			Pen pen = new Pen(color.ToFormsColor());
			Graphics.DrawEllipse(pen, (float)(location.X - radius), (float)(location.Y - radius), (float)(2 * radius), (float)(2 * radius));
		}

		public override void FillCircle(Color color, Point location, double radius)
		{
			Brush brush = new SolidBrush(color.ToFormsColor());
			Graphics.FillEllipse(brush, (float)(location.X - radius), (float)(location.Y - radius), (float)(2 * radius + 1), (float)(2 * radius + 1));
		}

		public override void DrawRectangle(Color color, double x, double y, double width, double height)
		{
			Pen pen = new Pen(color.ToFormsColor());
			Graphics.DrawRectangle(pen, (float)x, (float)y, (float)width, (float)height);
		}

		public override void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			float left = (float)x, top = (float)y, right = (float)(x + width), bottom = (float)(y + height);
			var brush = new PathGradientBrush(new[] { new PointF(left, top), new PointF(right, top), new PointF(right, bottom), new PointF(left, bottom) });
			brush.SurroundColors = new[] { col1.ToFormsColor(), col2.ToFormsColor(), col3.ToFormsColor(), col4.ToFormsColor() };
			brush.CenterColor = Color.Merge(col1, col2, col3, col4).ToFormsColor();
			brush.SetSigmaBellShape(1);
			Graphics.DrawRectangle(new Pen(brush), left, top, (float)width, (float)height);
		}

		public override void FillRectangle(Color color, double x, double y, double width, double height)
		{
			Brush brush = new SolidBrush(color.ToFormsColor());
			Graphics.FillRectangle(brush, (float)x, (float)y, (float)width, (float)height);
		}

		public override void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			float left = (float)x, top = (float)y, right = (float)(x + width - 1), bottom = (float)(y + height - 1);
			var brush = new PathGradientBrush(new[] { new PointF(left, top), new PointF(right, top), new PointF(right, bottom), new PointF(left, bottom) });
			brush.SurroundColors = new[] { col1.ToFormsColor(), col2.ToFormsColor(), col3.ToFormsColor(), col4.ToFormsColor() };
			brush.CenterColor = Color.Merge(col1, col2, col3, col4).ToFormsColor();
			brush.SetSigmaBellShape(1, 1);
			Graphics.FillRectangle(brush, left, top, (float)width, (float)height);
		}

		public override void DrawLine(Color color, double x1, double y1, double x2, double y2)
		{
			Graphics.DrawLine(new Pen(color.ToFormsColor()), (float)x1, (float)y1, (float)x2, (float)y2);
		}

		public override void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			PointF p1 = new PointF((float)x1, (float)y1), p2 = new PointF((float)x2, (float)y2);
			var brush = new LinearGradientBrush(p1, p2, col1.ToFormsColor(), col2.ToFormsColor());
			var pen = new Pen(brush);

			Graphics.DrawLine(pen, p1, p2);
		}

		public override void DrawText(Color color, object text, double x, double y)
		{
			Brush brush = new SolidBrush(color.ToFormsColor());
			Graphics.DrawString(text.ToString(), new Font("Arial", 12), brush, (float)x, (float)y);
		}

		public override void Clear(Color color)
		{
			Graphics.Clear(System.Drawing.Color.FromArgb(color.Argb));
		}

		public override void Blit(Surface dest, IntRectangle srcRect, IntRectangle destRect)
		{
			FormsSurface surface = dest as FormsSurface;
			if (surface == null)
				throw new InvalidOperationException("The destination surface must be a FormsSurface.");

			surface.Graphics.DrawImage(_bmp, new RectangleF(destRect.Left, destRect.Top, destRect.Width, destRect.Height), new RectangleF(srcRect.Left, srcRect.Top, srcRect.Width, srcRect.Height), GraphicsUnit.Pixel);
		}

		public override int Height
		{
			get { return _bmp.Height; }
		}

		public override int Width
		{
			get { return _bmp.Width; }
		}

		public void Dispose()
		{
			_bmp.Dispose();
		}
	}
}
