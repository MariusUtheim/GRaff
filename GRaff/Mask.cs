

using System;
using System.Diagnostics.Contracts;

namespace GRaff
{
#warning Review class
	public sealed class Mask
	{
		private readonly GameObject _owner;
		private MaskShape _maskShape;

		internal Mask(GameObject owner)
		{
			Contract.Requires<ArgumentNullException>(owner != null);
			this._owner = owner;
			this.Shape = MaskShape.Automatic;
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(_owner != null);
		}

		public Polygon GetPolygon() => Transform.Polygon(Shape?.Polygon);

		private Transform Transform => _owner.Transform;

		/// <summary>
		/// Gets or sets the shape of this GRaff.Mask.
		/// If the shape is set to GRaff.MaskShape.Automatic, it instead returns the MaskShape of the underlying sprite, or GRaff.MaskShape.None if that sprite is null.
		/// This value cannot be set to null.
		/// </summary>
		/// <exception cref="ArgumentNullException">the value is null.</exception>
		public MaskShape Shape
		{
			get
			{
				if (_maskShape == MaskShape.Automatic)
					return _owner.Sprite?.MaskShape;
				else
					return _maskShape;	
			}

			set { _maskShape = value; }
		}

		public Rectangle BoundingBox
		{
			get
			{
				Polygon _polygon = GetPolygon();
				if (_polygon == null)
					return new Rectangle(Transform.X, Transform.Y, 0, 0);

				Point vertex;
				double left, right, top, bottom;
				vertex = _polygon.Vertex(0);
				left = right = vertex.X;
				top = bottom = vertex.Y;

				for (int i = 1; i < _polygon.Length; i++)
				{
					vertex = _polygon.Vertex(i);
					if (vertex.X < left) left = vertex.X;
					if (vertex.X > right) right = vertex.X;
					if (vertex.Y < top) top = vertex.Y;
					if (vertex.Y > bottom) bottom = vertex.Y;
				}

				return new Rectangle(left, top, right - left, bottom - top);
			}
		}

		public bool ContainsPoint(Point pt) => GetPolygon()?.ContainsPoint(pt) ?? false;

		public bool ContainsPoint(double x, double y) => ContainsPoint(new Point(x, y));

		public bool Intersects(Mask other)
		{
			if (other == null)
				return false;
			return GetPolygon()?.Intersects(other.GetPolygon()) ?? false;
		}
	}
}
