using System;
using GameMaker;
using GameMaker.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class SurfaceTest
	{
		private Surface _CreateSurface(int width, int height)
		{
			return new FormsSurface(width, height);
		}

		[TestMethod]
		public void Constructor()
		{
			var target = _CreateSurface(2, 2);
			Assert.AreEqual(2, target.Width);
			Assert.AreEqual(2, target.Height);

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(Color.Black, target.GetPixel(x, y));
		}

		[TestMethod]
		public void DrawRectangle()
		{
			var target = _CreateSurface(5, 5);
			target.DrawRectangle(Color.White, 1, 1, 2, 2);

			int[,] expectedMask = new[,] {
				{ 0, 0, 0, 0, 0},
				{ 0, 1, 1, 1, 0},
				{ 0, 1, 0, 1, 0},
				{ 0, 1, 1, 1, 0},
				{ 0, 0, 0, 0, 0}
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[x, y] == 1 ? Color.White : Color.Black, target.GetPixel(x, y));
		}

		[TestMethod]
		public void FillRectangle()
		{
			var target = _CreateSurface(5, 5);
			target.FillRectangle(Color.White, 1, 1, 3, 3);

			int[,] expectedMask = new[,] {
				{ 0, 0, 0, 0, 0},
				{ 0, 1, 1, 1, 0},
				{ 0, 1, 1, 1, 0},
				{ 0, 1, 1, 1, 0},
				{ 0, 0, 0, 0, 0}
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[x, y] == 1 ? Color.White : Color.Black, target.GetPixel(x, y));
		}

		[TestMethod]
		public void DrawRectangleColor()
		{
			var target = _CreateSurface(3, 3);
			target.DrawRectangle(Color.Black, Color.Red, Color.Blue, Color.Purple, 0, 0, 3, 3);

			Assert.AreEqual<Color>(Color.Black, target.GetPixel(0, 0), "Error occurred at target[0, 0]");
			Assert.AreEqual<Color>(Color.Red, target.GetPixel(target.Width - 1, 0), "error occurred at target[1, 0]");
			Assert.AreEqual<Color>(Color.Blue, target.GetPixel(0, target.Height - 1));
			Assert.AreEqual<Color>(Color.Purple, target.GetPixel(target.Width - 1, target.Height - 1));
		}

		[TestMethod]
		public void DrawCircle()
		{

			var target = _CreateSurface(7, 7);
			target.DrawCircle(Color.White, new Point(3, 3), 3);

			int[,] expectedMask = new[,] {
				{ 0, 0, 1, 1, 1, 0, 0},
				{ 0, 1, 0, 0, 0, 1, 0},
				{ 1, 0, 0, 0, 0, 0, 1},
				{ 1, 0, 0, 0, 0, 0, 1},
				{ 1, 0, 0, 0, 0, 0, 1},
				{ 0, 1, 0, 0, 0, 1, 0},
				{ 0, 0, 1, 1, 1, 0, 0},
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[x, y] == 1 ? Color.White : Color.Black, target.GetPixel(x, y));
		}

		[TestMethod]
		public void FillCircle()
		{
			var target = _CreateSurface(7, 7);
			target.FillCircle(Color.White, new Point(3, 3), 3);

			int[,] expectedMask = new[,] {
				{ 0, 0, 0, 1, 0, 0, 0},
				{ 0, 1, 1, 1, 1, 1, 0},
				{ 0, 1, 1, 1, 1, 1, 0},
				{ 1, 1, 1, 1, 1, 1, 0},
				{ 0, 1, 1, 1, 1, 1, 0},
				{ 0, 1, 1, 1, 1, 1, 0},
				{ 0, 0, 0, 0, 0, 0, 0},
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[y, x] == 1 ? Color.White : Color.Black, target.GetPixel(x, y), String.Format("Error occurred at (x,y) = ({0}, {1})", x, y));
		}
	}
}
