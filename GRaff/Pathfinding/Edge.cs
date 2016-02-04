using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;


namespace GRaff.Pathfinding
{
	public class Edge : IEdge<Vertex, Edge>
	{
		internal Edge(IGraph<Vertex, Edge> graph, Vertex from, Vertex to)
			: this(graph, from, to, 1)
		{ }

		internal Edge(IGraph<Vertex, Edge> graph, Vertex from, Vertex to, double weight)
		{
			Contract.Requires<ArgumentNullException>(graph != null);
			Contract.Requires<ArgumentNullException>(from != null);
			Contract.Requires<ArgumentNullException>(to != null);
			Contract.Requires<ArgumentException>(from.Graph == graph && to.Graph == graph);
			Contract.Requires<ArgumentOutOfRangeException>(weight >= 0);
			this.Graph = graph;
			this.From = from;
			this.To = to;
			this.Weight = weight;
			from.edges.Add(this);
		}

		public IGraph<Vertex, Edge> Graph { get; }

		public Vertex From { get; }

		public Vertex To { get; }

		public double Weight { get; }
	}

	public class Edge<TVertex> : IEdge<TVertex, Edge<TVertex>>
		where TVertex : IVertex<TVertex, Edge<TVertex>>
	{
		public TVertex From
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IGraph<TVertex, Edge<TVertex>> Graph
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public TVertex To
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public double Weight
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
