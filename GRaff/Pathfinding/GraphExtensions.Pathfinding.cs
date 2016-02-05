using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;


namespace GRaff.Pathfinding
{
	public static partial class GraphExtensions
	{		
		private struct VertexLengthMetric<TVertex, TEdge> : IComparable<VertexLengthMetric<TVertex, TEdge>>
			where TVertex : IVertex<TVertex, TEdge> where TEdge : IEdge<TVertex, TEdge>
		{
			public readonly double Length, DistanceRemaining;
			public readonly TVertex Vertex;

			public VertexLengthMetric(TVertex vertex, double length, double distanceRemaining)
			{
				Vertex = vertex;
				Length = length;
				DistanceRemaining = distanceRemaining;
			}

			public int CompareTo(VertexLengthMetric<TVertex, TEdge> other)
			{
				if (Length + DistanceRemaining == other.Length + other.DistanceRemaining)
					return GMath.Sign(DistanceRemaining - other.DistanceRemaining);
				else
					return GMath.Sign((Length + DistanceRemaining) - (other.Length + other.DistanceRemaining));
			}

			public override bool Equals(object obj)
			{
				return Vertex.Equals(((VertexLengthMetric<TVertex, TEdge>)obj).Vertex);
            }

			public override int GetHashCode() => Vertex.GetHashCode();
		}

		public static Path<TVertex, TEdge> ShortestPath<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v1, TVertex v2, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Graph == graph && v2.Graph == graph && !v1.Equals(v2));

			var edges = graph.MinimalSpanningTree(v1, v2, maxDistance).TakeWhilePrevious(e => !e.To.Equals(v2)).ToArray();

			if (!edges[edges.Length - 1].To.Equals(v2))
				return null;

			var pathEdges = new LinkedList<TEdge>();
			var edge = edges.Last();
			pathEdges.AddLast(edge);
			for (int i = edges.Length - 1; i >= 0; i--)
			{
				if (edges[i].To.Equals(edge.From))
				{
					edge = edges[i];
					pathEdges.AddFirst(edge);
				}
			}

			return new Path<TVertex, TEdge>(pathEdges);
		}


		public static IEnumerable<TEdge> MinimalSpanningTree<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, Func<TVertex, double> heuristic, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(graph != null && v != null && heuristic != null);
			Contract.Requires<ArgumentException>(v.Graph == graph);

			var distance = graph.Vertices.ToDictionary(_ => _, _ => Double.PositiveInfinity);
			var previous = graph.Vertices.ToDictionary(_ => _, _ => default(TVertex));

			distance[v] = 0;

			var candidateNodes = new HeapSet<VertexLengthMetric<TVertex, TEdge>>();
			candidateNodes.Push(new VertexLengthMetric<TVertex, TEdge>(v, 0, 0));

			while (candidateNodes.Count > 0)
			{
				var current = candidateNodes.Pop().Vertex;
				var currentDistance = distance[current];

				if (currentDistance > maxDistance)
					yield break;

				if (previous[current] != null)
					yield return previous[current].EdgeTo(current);

				foreach (var edge in current.Edges)
				{
					var neighbour = edge.To;
					var length = currentDistance + edge.Weight;
					if (length < distance[neighbour])
					{
						if (distance[neighbour] == Double.PositiveInfinity)
							candidateNodes.Push(new VertexLengthMetric<TVertex, TEdge>(neighbour, length, heuristic(neighbour)));
						distance[neighbour] = length;
						previous[neighbour] = current;
					}
				}
			}
		}


		public static IEnumerable<TEdge> MinimalSpanningTree<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Graph == graph);
			return graph.MinimalSpanningTree(v, _ => 0, maxDistance);
		}

		public static IEnumerable<TEdge> MinimalSpanningTree<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, TVertex target, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(graph != null && v != null && target != null);
			Contract.Requires<ArgumentException>(v.Graph == graph && target.Graph == graph);
			Contract.Requires<ArgumentException>(!v.Equals(target));
			return graph.MinimalSpanningTree(v, h => h.HeuristicDistance(target), maxDistance);
		}
	}
}
