using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GRaff.Pathfinding
{
	[ContractClass(typeof(VertexContract<,>))]
	public interface IVertex<TVertex, TEdge>
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		IGraph<TVertex, TEdge> Graph { get; }
		bool IsConnectedTo(TVertex other);
		IEnumerable<TEdge> Edges { get; }

	}

	[ContractClassFor(typeof(IVertex<,>))]
	abstract class VertexContract<TVertex, TEdge> : IVertex<TVertex, TEdge>
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		public IEnumerable<TEdge> Edges
		{
			get
			{
				Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<TEdge>>(), e => e != null));
				return default(IEnumerable<TEdge>);
			}
		}

		public IGraph<TVertex, TEdge> Graph
		{
			get
			{
				Contract.Ensures(Contract.Result<IGraph<TVertex, TEdge>>() != null);
				return default(IGraph<TVertex, TEdge>);
			}
		}

		public bool IsConnectedTo(TVertex other) => default(bool);
	}
}