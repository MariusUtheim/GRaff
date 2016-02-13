using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;


namespace GRaff.Pathfinding
{
	public static partial class GraphExtensions
	{		
		private class VertexLengthMetric<TVertex, TEdge> : IComparable<VertexLengthMetric<TVertex, TEdge>>
			where TVertex : IVertex<TVertex, TEdge> where TEdge : IEdge<TVertex, TEdge>
		{
			public readonly double CurrentDistance, DistanceRemaining, BeautifulDistance;
			public readonly TVertex Vertex;
			public readonly VertexLengthMetric<TVertex, TEdge> Previous;

			public double BestCaseDistance => CurrentDistance + DistanceRemaining;

			public VertexLengthMetric(TVertex vertex, VertexLengthMetric<TVertex, TEdge> previous, double currentDistance, double distanceRemaining, double beautifulDistance)
			{
				Vertex = vertex;
				Previous = previous;
				CurrentDistance = currentDistance;
				DistanceRemaining = distanceRemaining;
				BeautifulDistance = beautifulDistance;
			}

			public int CompareTo(VertexLengthMetric<TVertex, TEdge> other)
			{
				if (Vertex.Equals(other.Vertex))
					return 0;
				if (BestCaseDistance == other.BestCaseDistance)
				{
					var closestDistance = DistanceRemaining.CompareTo(other.DistanceRemaining);
					if (closestDistance == 0)
						return BeautifulDistance.CompareTo(other.BeautifulDistance);
					else
						return closestDistance;
				}
				else
					return BestCaseDistance.CompareTo(other.BestCaseDistance);
			}

			public override bool Equals(object obj) => Vertex.Equals(((VertexLengthMetric<TVertex, TEdge>)obj).Vertex);

			public override int GetHashCode() => Vertex.GetHashCode();
		}


		public static Path<TVertex, TEdge> ShortestPath<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex origin, TVertex goal, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			return graph.ShortestPath(origin, goal, _ => 0, maxDistance);
		}


		public static Path<TVertex, TEdge> ShortestPath<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex origin, TVertex goal, Func<TVertex, double> beautyMetric, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(origin != null && goal != null);
			Contract.Requires<ArgumentException>(origin.Graph == graph && goal.Graph == graph && !origin.Equals(goal));

			var edges = graph.MinimalSpanningTree(origin, h => h.HeuristicDistance(goal), beautyMetric, maxDistance).TakeWhilePrevious(e => !e.To.Equals(goal)).ToArray();

			if (!edges[edges.Length - 1].To.Equals(goal))
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
			return MinimalSpanningTree(graph, v, heuristic, _ => 0, maxDistance);
        }

		public static IEnumerable<TEdge> MinimalSpanningTree<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, Func<TVertex, double> heuristic, Func<TVertex, double> beautyHeuristic, double maxDistance)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(graph != null && v != null && heuristic != null);
			Contract.Requires<ArgumentException>(v.Graph == graph);

			var distance = graph.Vertices.ToDictionary(_ => _, _ => Double.PositiveInfinity);
			var isTaken = new HashSet<VertexLengthMetric<TVertex, TEdge>>();

			distance[v] = 0;

			var candidateNodes = new Heap<VertexLengthMetric<TVertex, TEdge>>();
			candidateNodes.Add(new VertexLengthMetric<TVertex, TEdge>(v, null, 0, 0, 0));

			while (candidateNodes.Any())
			{
				var currentNode = candidateNodes.Pop();
				if (isTaken.Contains(currentNode))
					continue;

				var current = currentNode.Vertex;
				
				if (currentNode.BestCaseDistance > maxDistance)
					yield break;

				isTaken.Add(currentNode);
				if (currentNode.Previous != null)
					yield return currentNode.Previous.Vertex.EdgeTo(current);

				foreach (var edge in current.Edges)
				{
					var neighbour = edge.To;
					var length = currentNode.CurrentDistance + edge.Weight;
					if (length < distance[neighbour])
					{
						candidateNodes.Add(new VertexLengthMetric<TVertex, TEdge>(neighbour, currentNode, length, heuristic(neighbour), beautyHeuristic(neighbour)));
						distance[neighbour] = length;
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
	}
}
