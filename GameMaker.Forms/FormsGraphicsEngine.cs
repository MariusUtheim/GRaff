using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMaker.Forms
{
	public class FormsGraphicsEngine : GraphicsEngine
    {
		private GMForm _form;

		public FormsGraphicsEngine()
		{
			
		}

		[STAThread]
		public override void Run(Action gameStart)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			this._form = new GMForm();
			this.Graphics = null;

			this._form.Width = Room.Width;
			this._form.Height = Room.Height;
			this._form.Paint += this.OnPaint;
			this._form.MouseMove += this.OnMouseMove;
			this._form.MouseDown += this.OnMouseDown;
			this._form.MouseUp += this.OnMouseUp;
			this._form.KeyDown += this.OnKeyDown;
			this._form.KeyUp += this.OnKeyUp;

			gameStart();

			Application.Run(_form);
		}

		public override void Quit()
		{
			_form.Close();
		}

		private Graphics Graphics { get; set; }

		public Form GetWindow()
		{
			return _form;
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			this.Graphics = e.Graphics;
			try
			{
				Game.Loop();
				Game.Redraw();
				_form.Invalidate();
			}
			catch (Exception err)
			{
				MessageBox.Show(err.ToString());
			}
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			this.SetMouseLocation(e.X, e.Y);
		}

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			this.MouseDown(e.Button.ToGMMouseButton());
		}

		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			this.MouseUp(e.Button.ToGMMouseButton());
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			this.KeyDown(e.KeyCode.ToGMKey());
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			this.KeyUp(e.KeyCode.ToGMKey());
		}

		public override Texture LoadTexture(string file)
		{
			return new FormsTexture(file);
		}

		public override void DrawImage(double x, double y, Image image)
		{
			float tx = (float)x, ty = (float)y;
			Graphics.TranslateTransform(tx, ty);

			Graphics.RotateTransform((float)image.Rotation.Degrees);
			Graphics.ScaleTransform((float)image.XScale, (float)image.YScale);

			Graphics.TranslateTransform(-image.XOrigin, -image.YOrigin);

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

		public override void Clear(Color color)
		{
			Graphics.Clear(System.Drawing.Color.FromArgb(color.Argb));
		}

		public override string Title
		{
			get { return _form.Text; }
			set { _form.Text = value; }
		}

		public override void Refresh()
		{
			_form.Update();
		}

		public override bool IsVisible
		{
			get
			{
				return _form.Visible;
			}
			set
			{
				_form.Visible = value;
			}
		}

		public override bool IsFullscren
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool IsBorderVisible
		{
			get
			{
				return _form.FormBorderStyle != FormBorderStyle.None;
			}
			set
			{
				_form.FormBorderStyle = value ? FormBorderStyle.FixedDialog : FormBorderStyle.None;
			}
		}

		public override bool IsOnTop
		{
			get
			{
				return _form.TopMost;
			}
			set
			{
				_form.TopMost = value;
			}
		}

		public override bool IsResizable
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int Width
		{
			get
			{
				return _form.Width;
			}
			set
			{
				_form.Width = value;
			}
		}

		public override int Height
		{
			get
			{
				return _form.Height;
			}
			set
			{
				_form.Height = value;
			}
		}


	}
}
