using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding2
{
    interface IGraph
    {
        IEnumerable<IVertex> Vertices { get; }
    }
}
