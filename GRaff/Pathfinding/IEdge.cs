using System;
using System.Diagnostics.Contracts;

namespace GRaff.Pathfinding
{
	[ContractClass(typeof(EdgeContract<,>))]
	public interface IEdge<out TVertex, out TEdge>
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		IGraph<TVertex, TEdge> Graph { get; }
		TVertex From { get; }
		TVertex To { get; }
		double Weight { get; }
	}

	[ContractClassFor(typeof(IEdge<,>))]
	abstract class EdgeContract<TVertex, TEdge> : IEdge<TVertex, TEdge>
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		public IGraph<TVertex, TEdge> Graph
		{
			get
			{
				Contract.Ensures(Contract.Result<IGraph<TVertex, TEdge>>() != null);
				return default(IGraph<TVertex, TEdge>);
			}
		}

		public TVertex From
		{
			get
			{
				Contract.Ensures(Contract.Result<TVertex>() != null);
				return default(TVertex);
			}
		}

		public TVertex To
		{
			get
			{
				Contract.Ensures(Contract.Result<TVertex>() != null);
				return default(TVertex);
			}
		}

		public double Weight
		{
			get
			{
				Contract.Ensures(Contract.Result<double>() >= 0);
				return default(double);
			}
		}
	}
}