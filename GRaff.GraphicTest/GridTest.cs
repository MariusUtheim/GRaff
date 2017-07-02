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
			var blocked = new bool[width, height];
			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
					blocked[x, y] = GRandom.Probability(0.15);

			_grid = new Grid(blocked);
		}

		public override void OnStep()
		{
			Window.Title = $"GridTest - FPS: {Time.Fps}";
			if (Time.LoopCount % 5 == 0)
				n++;
        }

		static Point _mapToScreen(IntVector p) => new Point(x0 + dx * p.X, y0 + dy * p.Y);
		static IntVector _mapToGrid(Point p) => new IntVector((int)((p.X - x0) / dx + 0.5), (int)((p.Y - y0) / dy + 0.5));
		static Point _snapToGrid(Point p) => _mapToScreen(_mapToGrid(p));

		private void _drawShortestPath()
		{
			if (_from != null)
				Draw.FillCircle(_mapToScreen(_from.Location), 7, Colors.Red.Transparent(0.5));
			if (_to != null)
				Draw.FillCircle(_mapToScreen(_to.Location), 7, Colors.Blue.Transparent(0.5));
			if (_from != null && _to != null)
			{
				var path = _grid.ShortestPath(_from, _to);
				if (path != null)
					Draw.Lines(path.Edges.Select(e => new Line(_mapToScreen(e.From.Location), _mapToScreen(e.To.Location))).ToArray(), Colors.Red);
			}
		}

		private void _drawSpanningTree()
		{
			if (_to != null)
				Draw.FillCircle(_mapToScreen(_to.Location), 7, Colors.Blue.Transparent(0.5));
			if (_from != null)
			{
				Draw.FillCircle(_mapToScreen(_from.Location), 7, Colors.Red.Transparent(0.5));
				var edges = _grid.MinimalSpanningTree(_from);
				Draw.Lines(edges.Take(n).Select(e => new Line(_mapToScreen(e.From.Location), _mapToScreen(e.To.Location))).ToArray(), Colors.Red);
			}
		}

		private void _drawHeuristicSpanningTree()
		{
			if (_from != null)
				Draw.FillCircle(_mapToScreen(_from.Location), 7, Colors.Red.Transparent(0.9));
			if (_to != null)
				Draw.FillCircle(_mapToScreen(_to.Location), 7, Colors.Blue.Transparent(0.9));
			if (_from != null && _to != null)
			{
				Draw.FillCircle(_mapToScreen(_from.Location), 7, Colors.Red.Transparent(0.5));
				var edges = _grid.MinimalSpanningTree(_from, h => h.HeuristicDistance(_to));
				Draw.Lines(edges.Take(n).TakeWhilePrevious(e => e.To != _to)
										.Select(e => new Line(_mapToScreen(e.From.Location), _mapToScreen(e.To.Location))), Colors.Red);
			}
		}

		public void _drawBeautifulTree()
		{
			if (_from != null)
				Draw.FillCircle(_mapToScreen(_from.Location), 7, Colors.Red.Transparent(0.9));
			if (_to != null)
				Draw.FillCircle( _mapToScreen(_to.Location), 7, Colors.Blue.Transparent(0.9));
			if (_from != null && _to != null)
			{
				Draw.FillCircle(_mapToScreen(_from.Location), 7, Colors.Red.Transparent(0.5));
				var edges = _grid.MinimalSpanningTree(_from, h => h.HeuristicDistance(_to), b => b.EuclideanDistance(_to), Double.PositiveInfinity);
				Draw.Lines(edges.Take(n).TakeWhilePrevious(e => e.To != _to)
										.Select(e => new Line(_mapToScreen(e.From.Location), _mapToScreen(e.To.Location))), Colors.Red);
			}
		}

		public override void OnDraw()
		{
            Draw.Clear(Colors.LightGray);
			Draw.Lines(_grid.Edges.Select(e => new Line(_mapToScreen(e.From.Location), _mapToScreen(e.To.Location))), Colors.White);
            Draw.FillRectangle(_snapToGrid(Mouse.Location) - size/2, size, Colors.White.Transparent(0.8));
			switch (mode)
			{
				case 1: _drawShortestPath(); break;
				case 2: _drawSpanningTree(); break;
				case 3: _drawHeuristicSpanningTree(); break;
				case 4: _drawBeautifulTree(); break;
			}
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			if (button == MouseButton.Left)
			{
				_from = _grid[_mapToGrid(Mouse.Location)];
			}
			else if (button == MouseButton.Right)
			{
				_to = _grid[_mapToGrid(Mouse.Location)];
			}
		}

		public void OnKeyPress(Key key)
		{
			switch (key)
			{
				case Key.Number1: mode = 1; break;
				case Key.Number2: mode = 2; n = 0; break;
				case Key.Number3: mode = 3; n = 0; break;
				case Key.Number4: mode = 4; n = 0; break;
			}
		}
	}
}
