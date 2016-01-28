using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GRaff.Pathfinding
{
	[ContractClass(typeof(VertexContract))]
	public interface IVertex
	{
		IGraph<IVertex, IEdge> Owner { get; }
		bool IsConnectedTo(IVertex other);
		IEnumerable<IEdge> Edges { get; }
	}

	[ContractClassFor(typeof(IVertex))]
	abstract class VertexContract : IVertex
	{
		public IEnumerable<IEdge> Edges
		{
			get
			{
				Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<IEdge>>(), e => e != null));
				return default(IEnumerable<IEdge>);
			}
		}

		public IGraph<IVertex, IEdge> Owner
		{
			get
			{
				Contract.Ensures(Contract.Result<IGraph<IVertex, IEdge>>() != null);
				return default(IGraph<IVertex, IEdge>);
			}
		}

		public bool IsConnectedTo(IVertex other)
		{
			Contract.Requires<ArgumentNullException>(other != null);
			Contract.Requires<ArgumentException>(Owner == other.Owner);
			return default(bool);
		}
	}
}