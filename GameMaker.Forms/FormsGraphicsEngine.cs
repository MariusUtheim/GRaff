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
using FormsImage = System.Drawing.Image;

namespace GameMaker.Forms
{
	public class FormsGraphicsEngine : GraphicsEngine, IDisposable
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
			this._form.Paint += this.OnPaint;
			this._form.MouseMove += this.OnMouseMove;
			this._form.MouseDown += this.OnMouseDown;
			this._form.MouseUp += this.OnMouseUp;
			this._form.KeyDown += this.OnKeyDown;
			this._form.KeyUp += this.OnKeyUp;
			this._form.Width = Room.Current.Width;
			this._form.Height = Room.Current.Height;

			if (gameStart != null)
				gameStart();

			Application.Run(_form);
		}

		public override void Quit()
		{
			_form.Close();
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			(Draw.DefaultSurface as FormsSurface).Graphics = e.Graphics;
			
			try
			{
				Game.Loop();
				//var s = new FormsSurface(Width, Height);
				//s.Graphics = _graphics;
				Game.Redraw(Draw.DefaultSurface); //new FormsSurface(_form.Width, _form.Height, _graphics));
				//RectangleF sourceRect;
				//Rectangle visibleRegion = Draw.GetVisibleRegion();
				//sourceRect = new RectangleF((float)visibleRegion.Left, (float)visibleRegion.Top, (float)visibleRegion.Width, (float)visibleRegion.Height);
				//e.Graphics.DrawImage(_buffer, 0, 0, sourceRect, GraphicsUnit.Pixel);
				
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

		public override Texture[] LoadTexture(string file, int subimages)
		{
			if (subimages <= 0)
				throw new ArgumentException("Must be positive", "subimages");
			if (subimages == 1)
				return new Texture[] { new FormsTexture(file) };
			else
			{
				Bitmap rawImage = new Bitmap(file);
				
				//if (rawImage.Width % subimages != 0)
				//	throw new ArgumentException("The width of the image does not divide the number of subimages");
				int w = rawImage.Width / subimages;

				var result = new Texture[subimages];

				for (int i = 0; i < subimages; i++)
					result[i] = new FormsTexture(rawImage.Clone(new RectangleF(w * i, 0, w, rawImage.Height), PixelFormat.Format32bppArgb)); 

				return result;
			}
		}

		public override Surface CreateSurface(int width, int height)
		{
			return new FormsSurface(width, height);
		}

		public override string Title
		{
			get { return _form.Text; }
			set { _form.Text = value; }
		}

		public override void Refresh()
		{
			//_form.Update();
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

		public void Dispose()
		{
			_form.Dispose();
		}
	}
}
