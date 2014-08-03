using System;
using GameMaker;
using GameMaker.Forms;
using GameMaker.OpenGL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class SurfaceTest
	{
		private static Surface _CreateSurface(int width, int height)
		{
			return new GameMaker.Forms.FormsGraphicsEngine().CreateSurface(width, height);
		}

		private static void Compare(int[,] expectedMask, Surface target)
		{
			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[y, x] == 1 ? Color.White : Color.Black, target.GetPixel(x, y), String.Format("Discrepancy occurred at [{0}, {1}].", x, y));
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
		public void SetPoint()
		{
			var target = _CreateSurface(3, 3);
			target.SetPixel(1, 1, Color.White);
			int[,] expectedMask = new[,] {
				{ 0, 0, 0 },
				{ 0, 1, 0 },
				{ 0, 0, 0 }
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.IsTrue((expectedMask[x, y] == 1 ? Color.White : Color.Black) == target.GetPixel(x, y));
		}

		[TestMethod]
		public void DrawRectangle()
		{
			var target = _CreateSurface(5, 5);
			target.DrawRectangle(Color.White, 1, 1, 2, 2);

			int[,] expectedMask = new[,] {
				{ 0, 0, 0, 0, 0 },
				{ 0, 1, 1, 1, 0 },
				{ 0, 1, 0, 1, 0 },
				{ 0, 1, 1, 1, 0 },
				{ 0, 0, 0, 0, 0 }
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[x, y] == 1 ? Color.White : Color.Black, target.GetPixel(x, y));
		}

		[TestMethod]
		public void FillRectangle()
		{
			var target = _CreateSurface(6, 6);
			target.FillRectangle(Color.White, 1, 1, 3, 3);

			int[,] expectedMask = new[,] {
				{ 0, 0, 0, 0, 0, 0},
				{ 0, 1, 1, 1, 0, 0},
				{ 0, 1, 1, 1, 0, 0},
				{ 0, 1, 1, 1, 0, 0},
				{ 0, 0, 0, 0, 0, 0},
				{ 0, 0, 0, 0, 0, 0}
			};

			for (int x = 0; x < target.Width; x++)
				for (int y = 0; y < target.Height; y++)
					Assert.AreEqual<Color>(expectedMask[x, y] == 1 ? Color.White : Color.Black, target.GetPixel(x, y));
		}

		[TestMethod]
		public void DrawCircle()
		{

			var target = _CreateSurface(8, 8);
			target.DrawCircle(Color.White, new Point(3, 3), 3);
			int[,] expectedMask = new[,] {
				{ 0, 0, 1, 1, 1, 0, 0, 0},
				{ 0, 1, 0, 0, 0, 1, 0, 0},
				{ 1, 0, 0, 0, 0, 0, 1, 0},
				{ 1, 0, 0, 0, 0, 0, 1, 0},
				{ 1, 0, 0, 0, 0, 0, 1, 0},
				{ 0, 1, 0, 0, 0, 1, 0, 0},
				{ 0, 0, 1, 1, 1, 0, 0, 0},
				{ 0, 0, 0, 0, 0, 0, 0, 0}
			};

			Compare(expectedMask, target);
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

			Compare(expectedMask, target);
		}

		[TestMethod]
		public void DrawLine()
		{
			var target = _CreateSurface(5, 5);
			target.DrawLine(Color.White, 0, 0, 3, 0);
			target.DrawLine(Color.White, 0, 3, 3, 0);
			target.DrawLine(Color.White, 2, 4, 4, 1);

			int[,] expectedMask = new[,] {
				{ 1, 1, 1, 1, 0 },
				{ 0, 0, 1, 0, 1 },
				{ 0, 1, 0, 1, 0 },
				{ 1, 0, 0, 1, 0 },
				{ 0, 0, 1, 0, 0 }
			};

			Compare(expectedMask, target);
		}
	}
}
