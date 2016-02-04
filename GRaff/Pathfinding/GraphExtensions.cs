using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public static class GraphExtensions
	{
		private class PriorityQueue<T> : PriorityQueue<T, double> { }

		private class PriorityQueue<T, TKey> where TKey : IComparable<TKey>
		{
			private readonly HashSet<T> _items;
			private readonly Dictionary<T, TKey> _priorities;

			public PriorityQueue()
			{
				_items = new HashSet<T>();
				_priorities = new Dictionary<T, TKey>();
			}

			public void SetPriority(T item, TKey priority)
			{
				Debug.Assert(_items.Contains(item));
				_priorities[item] = priority;
			}

			public void Remove(T item)
			{
				_items.Remove(item);
				_priorities.Remove(item);
			}

			public bool Push(T item, TKey priority)
			{
				_priorities[item] = priority;
				return _items.Add(item);
			}

			public T Pop()
			{
				Debug.Assert(_items.Any());
				var item = _priorities.ArgMin(kv => kv.Value).Key;

				Debug.Assert(_items.All(i => _priorities[i].CompareTo(_priorities[item]) >= 0));

				_items.Remove(item);
				_priorities.Remove(item);
				return item;
			}

			public int Count => _items.Count;
		}

		private struct VertexLengthMetric : IComparable<VertexLengthMetric>
		{
			public readonly double Length, DistanceRemaining;

			public VertexLengthMetric(double length, double distanceRemaining)
			{
				Length = length;
				DistanceRemaining = distanceRemaining;
			}

			public int CompareTo(VertexLengthMetric other)
			{
				if (Length + DistanceRemaining == other.Length + other.DistanceRemaining)
					return GMath.Sign(DistanceRemaining - other.DistanceRemaining);
				else
					return GMath.Sign((Length + DistanceRemaining) - (other.Length + other.DistanceRemaining));
			}
		}

		private static Path<TVertex,TEdge> makePath<TVertex, TEdge>(TVertex source, Dictionary<TVertex, TVertex> map)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires(map != null);

			var nodes = new LinkedList<TVertex>();
			for (var currentNode = source; currentNode != null; currentNode = map[currentNode])
				nodes.AddFirst(currentNode);
			return new Path<TVertex, TEdge>(nodes);
		}

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

		public static IEnumerable<TVertex> Neighbours<TVertex, TEdge>(this TVertex vertex)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(vertex != null);
			Contract.Ensures(Contract.Result<IEnumerable<TVertex>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TVertex>>(), v => v != null));

			throw new NotImplementedException();// return vertex.Edges.Select(e => e.To).ToList();
		}

		public static Path<TVertex, TEdge> ShortestPath<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v1, TVertex v2)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Graph == graph && v2.Graph == graph && !v1.Equals(v2));

			var edges = graph.GenerateMinimalEdges(v1, v2).TakeWhilePrevious(e => !e.To.Equals(v2)).ToArray();

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
			/*
			var distance = graph.Vertices.ToDictionary(v => v, _ => Double.PositiveInfinity);
			var previous = graph.Vertices.ToDictionary(v => v, _ => default(TVertex));

			distance[v1] = 0;

			var candidateNodes = new PriorityQueue<TVertex>();
			candidateNodes.Push(v1, 0);

			while (candidateNodes.Count > 0)
			{
				var current = candidateNodes.Pop();
				var currentDistance = distance[current];

				if (current.Equals(v2))
					return makePath<TVertex, TEdge>(current, previous);

				foreach (var edge in current.Edges)
				{
					var neighbour = edge.To;
					var length = currentDistance + edge.Weight;
					if (length < distance[neighbour])
					{
						if (distance[neighbour] == Double.PositiveInfinity)
							candidateNodes.Push(neighbour, length);
						else
							candidateNodes.SetPriority(neighbour, length);
						distance[neighbour] = length;
						previous[neighbour] = current;
					}
				}
			}

			return null;
			*/
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

		public static IEnumerable<TEdge> GenerateMinimalEdges<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Graph == graph);
			return graph.GenerateMinimalEdges(v, _ => 0, maxDistance);
			/*
			var distance = graph.Vertices.ToDictionary(_ => _, _ => Double.PositiveInfinity);
			var previous = graph.Vertices.ToDictionary(_ => _, _ => default(TVertex));

			distance[v] = 0;

			var candidateNodes = new PriorityQueue<TVertex>();
			candidateNodes.Push(v, 0);

			while (candidateNodes.Count > 0)
			{
				var current = candidateNodes.Pop();
				var currentDistance = distance[current];

				if (currentDistance > maxDistance)
					yield break;

				if (previous[current] != null)
					yield return previous[current].EdgeTo<TVertex, TEdge>(current);

				foreach (var edge in current.Edges)
				{
					var neighbour = edge.To;
					var length = currentDistance + edge.Weight;
					if (length < distance[neighbour])
					{
						if (distance[neighbour] == Double.PositiveInfinity)
							candidateNodes.Push(neighbour, length);
						else
							candidateNodes.SetPriority(neighbour, length);
						distance[neighbour] = length;
						previous[neighbour] = current;
					}
				}
			}
			*/
		}

		public static IEnumerable<TEdge> GenerateMinimalEdges<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, Func<TVertex, double> heuristic, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			var distance = graph.Vertices.ToDictionary(_ => _, _ => Double.PositiveInfinity);
			var previous = graph.Vertices.ToDictionary(_ => _, _ => default(TVertex));

			distance[v] = 0;

			var candidateNodes = new PriorityQueue<TVertex, VertexLengthMetric>();
			candidateNodes.Push(v, default(VertexLengthMetric));

			while (candidateNodes.Count > 0)
			{
				var current = candidateNodes.Pop();
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
							candidateNodes.Push(neighbour, new VertexLengthMetric(length, heuristic(neighbour)));
						else
							candidateNodes.SetPriority(neighbour, new VertexLengthMetric(length, heuristic(neighbour)));
						distance[neighbour] = length;
						previous[neighbour] = current;
					}
				}
			}
		}

		public static IEnumerable<TEdge> GenerateMinimalEdges<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, TVertex target, double maxDistance = Double.PositiveInfinity)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			return graph.GenerateMinimalEdges(v, h => h.HeuristicDistance(target), maxDistance);
		}
	}
}
