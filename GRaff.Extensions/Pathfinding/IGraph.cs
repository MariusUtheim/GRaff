using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff.Pathfinding
{
	public interface IGraph<TVertex, TEdge> 
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		IEnumerable<TVertex> Vertices { get; }
		IEnumerable<TEdge> Edges { get; }
		bool IsDirected { get; }
	}

	abstract class GraphContract<TVertex, TEdge> : IGraph<TVertex, TEdge> 
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		public IEnumerable<TEdge> Edges
		{
			get
			{
				Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
				Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TEdge>>(), e => e != null));
				Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TEdge>>(), e => e.Graph == this));
                yield break;
			}
		}

		public bool IsDirected => default(bool);

		public IEnumerable<TVertex> Vertices
		{
			get
			{
				Contract.Ensures(Contract.Result<IEnumerable<TVertex>>() != null);
				Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TVertex>>(), v => v != null));
				Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TVertex>>(), v => v.Graph == this));
				yield break;
			}
		}

		public double HeuristicDistance(TVertex from, TVertex to)
		{
			Contract.Requires<ArgumentNullException>(from != null && to != null);
			Contract.Requires<ArgumentException>(from.Graph == this && to.Graph == this);
			return default(double);
		}
	}

	}