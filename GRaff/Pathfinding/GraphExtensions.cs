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

		public static bool EdgeExists<TVertex, TEdge>(IGraph<TVertex, TEdge> graph, TVertex v1, TVertex v2)
			where TVertex : IVertex<TVertex, TEdge>
			where TEdge : IEdge<TVertex, TEdge>
		{
			Contract.Requires<ArgumentNullException>(graph != null && v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Graph == graph && v2.Graph == graph);
			return v1.IsConnectedTo(v2);
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
			Contract.Requires<ArgumentException>(v1.Graph == graph && v2.Graph == graph);

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
	}
}
