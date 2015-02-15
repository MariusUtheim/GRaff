namespace GRaff.Graphics.Lighting
{
	public class LightObstacle
	{
		public LightObstacle(Line wall)
			: this(wall, Color.Black)
		{ }

		public LightObstacle(Line wall, Color filter)
		{
			Wall = wall;
			Filter = filter;
		}

		public Line Wall { get; private set; }

		public Color Filter { get; private set; }
	}
}