using System;
using System.Diagnostics.Contracts;

namespace GRaff.Pathfinding
{
	[ContractClass(typeof(EdgeContract))]
	public interface IEdge
	{
		IGraph<IVertex, IEdge> Owner { get; }
		bool IsDirected { get; }
		IVertex Vertex1 { get; }
		IVertex Vertex2 { get; }
	}

	[ContractClassFor(typeof(IEdge))]
	abstract class EdgeContract : IEdge
	{
		public IGraph<IVertex, IEdge> Owner
		{
			get
			{
				Contract.Ensures(Contract.Result<IGraph<IVertex, IEdge>>() != null);
				return default(IGraph<IVertex, IEdge>);
			}
		}

		public bool IsDirected => default(bool);

		public IVertex Vertex1
		{
			get
			{
				Contract.Ensures(Contract.Result<IVertex>() != null);
				return default(IVertex);
			}
		}

		public IVertex Vertex2
		{
			get
			{
				Contract.Ensures(Contract.Result<IVertex>() != null);
				return default(IVertex);
			}
		}
	}
}