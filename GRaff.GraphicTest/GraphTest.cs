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

		public GraphTest()
		{

		}

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
						_selectedVertex.Location = Mouse.Location;
				}
			}
			else
			{
				if (!Mouse.IsDown(MouseButton.Right))
				{
					var drop = _graph.Vertices
						.FirstOrDefault(v => v != _selectedVertex && (v.Location - Mouse.Location).Magnitude < Radius);
					if (drop != null)
						_graph.AddEdge(_selectedVertex, drop);
					_selectedVertex = null;
				}
			}
		}

		public override void OnDraw()
		{
			if (_fromVertex != null)
				Draw.FillCircle(Colors.ForestGreen, _fromVertex.Location, Radius + 2);
			if (_toVertex != null)
				Draw.FillCircle(Colors.Black, _toVertex.Location, Radius + 2);
			foreach (var vertex in _graph.Vertices)
				Draw.FillCircle(Colors.Red, vertex.Location, Radius);
			if (_selectedVertex != null && !_drag)
				Draw.Line(Colors.Black, _selectedVertex.Location, Mouse.Location);
			foreach (var edge in _graph.Edges)
				Draw.Line(Colors.DarkRed, edge.Line);

			if (_fromVertex != null)
			{
				foreach (var e in _graph.MinimalSpanningTree(_fromVertex, Double.PositiveInfinity))
				{
					Draw.Line(Colors.Green, e.Line);
				}
			}
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			if (Keyboard.IsDown(Key.ControlLeft) || Keyboard.IsDown(Key.ControlRight))
			{
				foreach (var vertex in _graph.Vertices)
					if ((vertex.Location - Mouse.Location).Magnitude < Radius)
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
						if ((vertex.Location - Mouse.Location).Magnitude < Radius)
						{
							_selectedVertex = vertex;
							return;
						}

					_selectedVertex = _graph.AddVertex(Mouse.Location);
				}
				else if (button == MouseButton.Right)
				{
					foreach (var vertex in _graph.Vertices)
						if ((vertex.Location - Mouse.Location).Magnitude < Radius)
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
