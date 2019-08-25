using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Pathfinding2
{
    static class PathfindingExtensions
    {
        private class PathfindingInfo
        {
            public IVertex Vertex { get; set; }

            public IVertex Previous { get; set; }

            public double Distance { get; set; }

            public bool IsFixed { get; set; }

        }

        public static List<IVertex> ShortestDistanceDijkstra(this IGraph graph, IVertex from, IVertex to)
        {
            var pathfindingInfos = new Dictionary<IVertex, PathfindingInfo>();
            var priorityQueue = new Heap<PathfindingInfo>();

            var fromInfo = new PathfindingInfo { Vertex = from, Previous = null, Distance = 0, IsFixed = true };
            pathfindingInfos.Add(from, fromInfo);
            priorityQueue.Push(fromInfo);

            while (priorityQueue.Any())
            {
                var currentInfo = priorityQueue.Pop();
                if (currentInfo.IsFixed)
                    continue;
                
                if (currentInfo.Vertex == to)
                {
                    // Return backtrack
                    throw new NotImplementedException();
                }

                foreach (var edge in currentInfo.Vertex.Edges)
                {
                    if (pathfindingInfos.TryGetValue(edge.To, out var otherInfo))
                    {

                    }
                    else
                    {
                        var newInfo = new PathfindingInfo { Vertex = edge.To, Previous = currentInfo.Vertex, Distance = currentInfo.Distance + edge.Weight, IsFixed = false };
                        pathfindingInfos.Add(edge.To, newInfo);
                        priorityQueue.Add(newInfo);
                    }
                }

            }


            throw new NotImplementedException();
        }

    }
}
