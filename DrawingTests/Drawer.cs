using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;
using OpenTK.Graphics.OpenGL;

namespace DrawingTests
{
	public class Drawer : GameObject, IKeyListener
	{
		int idx = 0;

		public Drawer()
		{
			Mask.Diamond(-25, -25, 50, 50);
			X = 300;
			Y = 400;
			Depth = -1;
//			for (int x = -24; x <= 24; x += 2)
//				for (int y = -24; y <= 24; y += 2)
//					Instance<TestPoint>.Create(X + x, Y + y);
		}

		public override void OnStep()
		{
			
			Transform.Rotation += Angle.Deg(1);
		}

		public override void OnDraw()
		{
			Point[] pts = new Point[] {
				new Point(90 + 15.0 * GMath.Cos(Time.LoopCount / 23.0), 90 + 15.0 * GMath.Sin(Time.LoopCount / 15.0)),
				new Point(70 + 15.0 * GMath.Cos(Time.LoopCount / 14.0), 250+ 15.0 * GMath.Sin(Time.LoopCount / 23.0)),
				new Point(140 + 15.0 * GMath.Cos(Time.LoopCount / 13.0), 400+ 15.0 * GMath.Sin(Time.LoopCount / 14.0)),
				new Point(500 + 15.0 * GMath.Cos(Time.LoopCount / 17.0), 385+ 15.0 * GMath.Sin(Time.LoopCount / 13.0)),
				new Point(400 + 15.0 * GMath.Cos(Time.LoopCount / 15.0), 200+ 15.0 * GMath.Sin(Time.LoopCount / 17.0))
			};

			GL.Begin(PrimitiveType.TriangleFan);
			GL.Color3(Color.PeachPuff.R, Color.PeachPuff.G, Color.PeachPuff.B);
			GL.Vertex2(Mouse.X, Mouse.Y);
			GL.Color3(Color.Purple.R, Color.Purple.G, Color.Purple.B);
			foreach (Point p in pts)
				GL.Vertex2(p.X, p.Y);
			GL.Vertex2(pts[0].X, pts[0].Y);
			GL.End();

		}

		public void OnKey(Key key)
		{
			Transform.Rotation += Angle.Deg(3);
		}
	}
}
