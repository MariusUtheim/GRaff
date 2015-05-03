namespace GRaff.GraphicTest
{
	internal class Marker : GameObject
	{
		double t = 15;

		public override void OnDraw()
		{
			Draw.FillCircle(Colors.White, Location, t);
			if (--t == 0)
				Destroy();
		}
	}
}