using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Pathfinding;

namespace GRaff.GraphicTest
{
	[Test]
	class GridTest : GameElement, IGlobalMousePressListener, IKeyPressListener
	{
		private const int width = 32, height = 24;
		private const double x0 = 32, y0 = 24, dx = 30, dy = 30;
		private static readonly Vector size = new Vector(dx, dy);
		private int mode = 1;
		int n = 0;

		private readonly Grid _grid;
		private GridVertex _from = null, _to = null;

		public GridTest()
		{
			Room.Current.Background.Color = Colors.Black;
			var blocked = new bool[width, height];
			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
					blocked[x, y] = GRandom.Probability(0.1);
			_grid = new Grid(blocked);
		}

		public override void OnStep()
		{
			Window.Title = $"GridTest - FPS: {Time.FPS}";
			if (Time.LoopCount % 10 == 0)
				n++;
        }

		static Point mapToScreen(IntVector p) => new Point(x0 + dx * p.X, y0 + dy * p.Y);
		static IntVector mapToGrid(Point p) => new IntVector((int)((p.X - x0) / dx + 0.5), (int)((p.Y - y0) / dy + 0.5));
		static Point snapToGrid(Point p) => mapToScreen(mapToGrid(p));

		private void drawShortestPath()
		{
			if (_from != null)
				Draw.FillCircle(Colors.Red.Transparent(0.5), mapToScreen(_from.Location), 7);
			if (_to != null)
				Draw.FillCircle(Colors.Blue.Transparent(0.5), mapToScreen(_to.Location), 7);
			if (_from != null && _to != null)
			{
				var path = _grid.ShortestPath(_from, _to);
				if (path != null)
					Draw.Primitive.Lines(Colors.Red, path.Edges.Select(e => new Line(mapToScreen(e.From.Location), mapToScreen(e.To.Location))).ToArray());
			}
		}

		private void drawSpanningTree()
		{
			if (_from != null)
			{
				Draw.FillCircle(Colors.Red.Transparent(0.5), mapToScreen(_from.Location), 7);
				var edges = _grid.GenerateMinimalEdges(_from);
				Draw.Primitive.Lines(Colors.Red, edges.Take(n).Select(e => new Line(mapToScreen(e.From.Location), mapToScreen(e.To.Location))).ToArray());
			}
		}

		private void drawHeuristicSpanningTree()
		{
			if (_from != null)
				Draw.FillCircle(Colors.Red.Transparent(0.9), mapToScreen(_from.Location), 7);
			if (_to != null)
				Draw.FillCircle(Colors.Blue.Transparent(0.9), mapToScreen(_to.Location), 7);
			if (_from != null && _to != null)
			{
				Draw.FillCircle(Colors.Red.Transparent(0.5), mapToScreen(_from.Location), 7);
				var path = _grid.ShortestPath(_from, _to, n);
				Draw.Primitive.Lines(Colors.Red, edges.Take(n).Select(e => new Line(mapToScreen(e.From.Location), mapToScreen(e.To.Location))).ToArray());
			}
		}

		public override void OnDraw()
		{
			Draw.Primitive.Lines(Colors.White, _grid.Edges.Select(e => new Line(mapToScreen(e.From.Location), mapToScreen(e.To.Location))).ToArray());
			Draw.FillRectangle(Colors.White.Transparent(0.8), snapToGrid(Mouse.Location) - size/2, size);
			switch (mode)
			{
				case 1: drawShortestPath(); break;
				case 2: drawSpanningTree(); break;
				case 3: drawHeuristicSpanningTree(); break;
			}
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			if (button == MouseButton.Left)
			{
				_from = _grid[mapToGrid(Mouse.Location)];
			}
			else if (button == MouseButton.Right)
			{
				_to = _grid[mapToGrid(Mouse.Location)];
			}
		}

		public void OnKeyPress(Key key)
		{
			switch (key)
			{
				case Key.Number1: mode = 1; break;
				case Key.Number2: mode = 2; n = 0; break;
				case Key.Number3: mode = 3; n = 0; break;
			}
		}
	}
}
