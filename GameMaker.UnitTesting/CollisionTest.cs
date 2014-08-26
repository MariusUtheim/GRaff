using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
{
	class TestObject : GameObject
	{
		public TestObject() : base() { }
		public TestObject(double x, double y) : base(x, y) { }
	}

	[TestClass]
	public class CollisionTest
	{
		[TestMethod]
		public void Positions_Overlap_and_Rects_Intersect()
		{
			Vector sz = new Vector(20, 20);
			Rectangle rect = new Rectangle(new Point(0, 0), sz);

			TestObject obj1 = new TestObject();
			obj1.Mask.Rectangle(10, 10, 20, 20);
			
			TestObject obj2 = new TestObject();
			obj2.Mask.Rectangle(20, 20, 20, 20);

			Assert.IsTrue(obj1.Intersects(obj2));
		}

		[TestMethod]
		public void Rects_Overlap()
		{
			TestObject obj1 = new TestObject(), obj2 = new TestObject();

			obj1.Mask.Rectangle(0, 0, 10, 10);
			obj2.Mask.Rectangle(0, 0, 32, 16);

			obj1.Location = new Point(0, 0);
			obj2.Location = new Point(8, 8);

			Assert.IsTrue(obj1.Intersects(obj2));
		}

		[TestMethod]
		public void NonIntersection()
		{
			TestObject obj1 = new TestObject(), obj2 = new TestObject();

			obj1.Mask.Rectangle(30, 30);
			obj2.Mask.Rectangle(30, 20);

			obj1.Location = new Point(1000, 1000);
			obj2.Location = new Point(10, 10);

			Assert.IsFalse(obj1.Intersects(obj2));
		}
	}
}
