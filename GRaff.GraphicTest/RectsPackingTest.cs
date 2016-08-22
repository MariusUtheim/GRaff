using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Graphics.Text;

namespace GRaff.GraphicTest
{
	[Test]
    class RectsPackingTest : GameElement
	{
		private List<IntVector> sz;
		private IntRectangle[] rects;
		private Color[] colors;

		public RectsPackingTest()
		{
			GRandom.Seed(299792458);

			sz = Enumerable.Range(0, 20)
				.Select(_ => new IntVector(GRandom.Integer(45, 90), GRandom.Integer(45, 90)))
				.ToList();

			var colorRNG = new GRaff.Randomness.RgbDistribution();

			var canvas = new Canvas();
			Func<int, int, double> diagonalComparison = (w, h) => w * w + h * h;
			rects = RectanglePacker.Pack(sz.Select(s => new IntVector(s.X, s.Y)).ToArray())
						  .Select(im => new IntRectangle(im.Left, im.Top, im.Size.X, im.Size.Y))
						  .ToArray();


			colors = Enumerable.Range(0, rects.Length).Select(_ => colorRNG.Generate()).ToArray();
		}

		public override void OnDraw()
		{
			for (var i = 0; i < rects.Length; i++)
				Draw.FillRectangle(colors[i], rects[i]);
		}
	}
}
