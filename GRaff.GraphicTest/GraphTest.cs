using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Pathfinding;

namespace GRaff.GraphicTest
{
	[Test]
	class GraphTest : GameElement, IGlobalMousePressListener
	{
		private const double Radius = 5;
		private readonly SpatialGraph _graph = new SpatialGraph();
		private SpatialVertex _selectedVertex, _fromVertex, _toVertex;
		private bool _drag;

		public override void OnStep()
		{
			if (_selectedVertex == null)
				return;

			if (_drag)
			{
				if (!Mouse.IsDown(MouseButton.Left))
					_selectedVertex = null;
				else if (_selectedVertex != null)
				{
					if (_drag)
						_selectedVertex.Location = Mouse.ViewLocation;
				}
			}
			else
			{
				if (!Mouse.IsDown(MouseButton.Right))
				{
					var drop = _graph.Vertices
						.FirstOrDefault(v => v != _selectedVertex && (v.Location - Mouse.ViewLocation).Magnitude < Radius);
					if (drop != null)
						_graph.AddEdge(_selectedVertex, drop);
					_selectedVertex = null;
				}
			}
		}

		public override void OnDraw()
		{
            Draw.Clear(Colors.Black);
			if (_fromVertex != null)
				Draw.FillCircle(_fromVertex.Location, Radius + 2, Colors.ForestGreen);
			if (_toVertex != null)
				Draw.FillCircle(_toVertex.Location, Radius + 2, Colors.Black);
			foreach (var vertex in _graph.Vertices)
				Draw.FillCircle(vertex.Location, Radius, Colors.Red);
			if (_selectedVertex != null && !_drag)
				Draw.Line(_selectedVertex.Location, Mouse.ViewLocation, Colors.White);
			foreach (var edge in _graph.Edges)
				Draw.Line(edge.Line, Colors.Red);

			if (_fromVertex != null)
			{
#warning This doesn't actually work correctly
                foreach (var e in _graph.MinimalSpanningTree(_fromVertex, Double.PositiveInfinity))
				{
					Draw.Line(e.Line, Colors.Green);
				}
			}
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			if (Keyboard.IsDown(Key.ControlLeft) || Keyboard.IsDown(Key.ControlRight))
			{
				foreach (var vertex in _graph.Vertices)
					if ((vertex.Location - Mouse.ViewLocation).Magnitude < Radius)
					{
						if (button == MouseButton.Left)
							_fromVertex = vertex;
						else
							_toVertex = vertex;
						return;
					}
				if (button == MouseButton.Left)
					_fromVertex = null;
				else
					_toVertex = null;
			}
			else
			{
				if (button == MouseButton.Left)
				{
					_drag = true;
					foreach (var vertex in _graph.Vertices)
						if ((vertex.Location - Mouse.ViewLocation).Magnitude < Radius)
						{
							_selectedVertex = vertex;
							return;
						}

					_selectedVertex = _graph.AddVertex(Mouse.ViewLocation);
				}
				else if (button == MouseButton.Right)
				{
					foreach (var vertex in _graph.Vertices)
						if ((vertex.Location - Mouse.ViewLocation).Magnitude < Radius)
						{
							_selectedVertex = vertex;
							_drag = false;
							return;
						}
				}
			}
		}
	}
}
