namespace GRaff.GraphicTest
{
	internal class Marker : GameObject
	{
		double t = 15;

		public override void OnDraw()
		{
			Draw.FillCircle(Location, t, Colors.White);
			if (--t == 0)
				Destroy();
		}
	}
}