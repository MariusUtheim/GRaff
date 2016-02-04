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
		private class PriorityQueue<T>
		{
			private readonly HashSet<T> _items;
			private readonly Dictionary<T, double> _priorities;

			public PriorityQueue()
			{
				_items = new HashSet<T>();
				_priorities = new Dictionary<T, double>();
			}

			public void SetPriority(T item, double priority)
			{
				Debug.Assert(_items.Contains(item));
				_priorities[item] = priority;
			}

			public void Remove(T item)
			{
				_items.Remove(item);
				_priorities.Remove(item);
			}

			public bool Push(T item, double priority)
			{
				_priorities[item] = priority;
				return _items.Add(item);
			}

			public T Pop()
			{
				Debug.Assert(_items.Any());
				var item = _priorities.ArgMin(kv => kv.Value).Key;
				_items.Remove(item);
				_priorities.Remove(item);
				return item;
			}

			public int Count => _items.Count;
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

		public static TEdge EdgeTo<TVertex, TEdge>(this TVertex vertex, TVertex other)
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

		public static IEnumerable<TEdge> GenerateMinimalEdges<TVertex, TEdge>(this IGraph<TVertex, TEdge> graph, TVertex v, double maxDistance)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Graph == graph);

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
		}
	}
}
