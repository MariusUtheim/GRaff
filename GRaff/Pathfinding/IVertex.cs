using System.Collections.Generic;

namespace GRaff.Pathfinding
{
	public interface IVertex
	{
#warning DESIGN: Which type?
		IEnumerable<IEdge> Edges();
	}
}