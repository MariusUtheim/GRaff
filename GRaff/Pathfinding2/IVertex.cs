using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding2
{
    interface IVertex
    {
        IGraph Graph { get; }
        bool IsConnectedTo(IVertex other);
        IEnumerable<IEdge> Edges { get; }
        IEnumerable<IVertex> Neighbours { get; }
        double HeuristicDistance(IVertex other);
    }
}
