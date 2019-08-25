using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding2
{
    interface IEdge
    {
        double Weight { get; }

        IVertex From { get; }

        IVertex To { get; }
    }
}
