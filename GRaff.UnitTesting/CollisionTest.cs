using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	class TestObject : GameObject
	{
		public TestObject(MaskShape maskShape) : base(0, 0)
		{
			Mask.Shape = maskShape;
		}

	}

	[TestClass]
	public class CollisionTest
	{
		private static TestObject testRegion20x20 = new TestObject(MaskShape.Rectangle(20, 20));

		[TestMethod]
		public void Collision_Rectangle()
		{
			TestObject targetRegion;

			targetRegion = new TestObject(MaskShape.Rectangle(2, 2));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Rectangle(5, 5, 10, 10));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Rectangle(new Rectangle(5, 5, 10, 10)));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));


			targetRegion = new TestObject(MaskShape.Rectangle(12, 2, 15, 15));
			Assert.IsFalse(testRegion20x20.Intersects(targetRegion));
		}

		[TestMethod]
		public void Collision_Diamond()
		{
			TestObject targetRegion;

			targetRegion = new TestObject(MaskShape.Diamond(2, 2));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Diamond(5, 0, 10, 10));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Diamond(new Rectangle(5, 5, 12, 12)));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));


			targetRegion = new TestObject(MaskShape.Diamond(5, 5, 30, 30));
			Assert.IsFalse(testRegion20x20.Intersects(targetRegion));
			
		}

		[TestMethod]
		public void Collision_Circle()
		{
			TestObject targetRegion;

			targetRegion = new TestObject(MaskShape.Circle(1));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Circle(new Point(15, 15), 10));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Circle(new Point(15, 15), 5));
			Assert.IsFalse(testRegion20x20.Intersects(targetRegion));
			
		}

		[TestMethod]
		public void Collision_Ellipse()
		{
			TestObject targetRegion;

			targetRegion = new TestObject(MaskShape.Ellipse(3, 2));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Ellipse(0, 9, 2, 20));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

			targetRegion = new TestObject(MaskShape.Ellipse(new Rectangle(1, 1, 40, 40)));
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));


			targetRegion = new TestObject(MaskShape.Ellipse(5, 5, 40, 40));
			Assert.IsFalse(testRegion20x20.Intersects(targetRegion));
		}

		[TestMethod]
		public void Collision_Transformed()
		{
			TestObject targetRegion = new TestObject(MaskShape.Rectangle(40, 40));

			targetRegion.Location = new Point(60, 5);
			Assert.IsFalse(testRegion20x20.Intersects(targetRegion));

			targetRegion.Transform.Scale *= 2;
			targetRegion.Transform.Rotation += Angle.Deg(45);
			Assert.IsTrue(testRegion20x20.Intersects(targetRegion));

		}
	}
}
