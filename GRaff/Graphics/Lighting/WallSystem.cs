using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics.Lighting
{
#if !PUBLISH
	public class WallSystem
	{
		public WallSystem(Rectangle region, IEnumerable<LightSource> sources, IEnumerable<LightObstacle> obstacles)
		{
			_region = region;
			_sources = sources.ToList();
			_obstacles = obstacles.ToList();
		}

		private List<LightSource> _sources;
		private List<LightObstacle> _obstacles;
		private Rectangle _region;

		public void Render()
		{
			foreach (var source in _sources)
				source.Render(_obstacles);
		}
	}
#endif
}
