using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public static partial class GraphExtensions
	{
		public static bool EdgeExists<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v1, TVertex v2)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(graph != null && v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Graph == graph && v2.Graph == graph);
			return v1.IsConnectedTo(v2);
		}

		public static TEdge EdgeTo<TVertex, TEdge>(this IVertex<TVertex, TEdge> vertex, TVertex other)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(vertex != null && other != null);
			Contract.Requires<ArgumentNullException>(vertex.Edges.Any(e => e.To.Equals(other)));
			return vertex.Edges.First(e => e.To.Equals(other));
		}


		private static bool _tagTree<TVertex, TEdge>(TVertex vertex, TVertex except, Dictionary<TVertex, bool> tagged)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			if (tagged[vertex])
				return false;
			tagged[vertex] = true;
			return vertex.Edges.Select(e => e.To)
							   .Except(new[] { except })
							   .All(v => _tagTree<TVertex, TEdge>(v, vertex, tagged));

		}
		public static bool IsTree<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			if (graph.IsDirected)
				return false;
			var vertices = graph.Vertices.ToArray();
			if (vertices.Length == 0)
				return true;

			var tagged = vertices.ToDictionary(v => v, _ => false);
			return _tagTree<TVertex, TEdge>(vertices.First(), default(TVertex), tagged);
		}
	}
}
