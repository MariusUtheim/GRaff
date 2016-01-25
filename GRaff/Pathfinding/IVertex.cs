using System.Collections.Generic;

namespace GRaff.Pathfinding
{
	public interface IVertex
	{
		IEnumerable<IEdge> Edges();
	}
}