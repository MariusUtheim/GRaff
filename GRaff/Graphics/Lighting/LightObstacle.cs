namespace GRaff.Graphics.Lighting
{
	public class LightObstacle
	{
		public LightObstacle(Line wall)
			: this(wall, Color.Black)
		{ }

		public LightObstacle(Line wall, Color color)
		{
			Wall = wall;
			Color = color;
		}

		public Line Wall { get; private set; }

		public Color Color { get; private set; }
	}
}