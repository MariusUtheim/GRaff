using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Forms
{
	public partial class DisplayObject : GameElement
	{
		private LinkedList<DisplayObject> _children;
		private LinkedListNode<DisplayObject> _node;
		
		public DisplayObject()
		{
			_children = new LinkedList<DisplayObject>();
			_node = new LinkedListNode<DisplayObject>(this);
		}

		public DisplayObject AddChildFirst(DisplayObject child)
		{
			Contract.Requires<ArgumentNullException>(child != null);
			Contract.Requires<InvalidOperationException>(child != this && !Ancestors.Contains(child), "Adding child would create cyclic dependencies.");
			child.RemoveFromParent();
			_children.AddFirst(child);
			child.Parent = this;
			return child;
		}

		public DisplayObject AddChild(DisplayObject child)
		{
			Contract.Requires<ArgumentNullException>(child != null);
			Contract.Requires<InvalidOperationException>(child != this && !Ancestors.Contains(child), "Adding child would create cyclic dependencies.");
			child.RemoveFromParent();
			_children.AddLast(child._node);
			child.Parent = this;
			return child;
		}

		public DisplayObject RemoveChild(DisplayObject child)
		{
			if (child != null && child._node.List == _children)
			{
				_children.Remove(child._node);
				child.Parent = null;
			}
			return child;
		}

		public void RemoveFromParent()
		{
			if (Parent == null)
				return;
			Parent._children.Remove(_node);
			Parent = null;
		}

		public IEnumerable<DisplayObject> Ancestors
		{
			get
			{
				if (Parent != null)
				{
					yield return Parent;
					foreach (var ancestor in Parent.Ancestors)
						yield return ancestor;
				}
			}
		}

		public IEnumerable<DisplayObject> Descendants
		{
			get
			{
				foreach (var child in _children)
				{
					yield return child;
					foreach (var descendant in child.Descendants)
						yield return descendant;
				}
			}
		}

		public DisplayObject Parent { get; private set; }

		public double X { get; set; }
		public double Y { get; set; }
		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public double Width { get; set; }
		public double Height { get; set; }
		public Vector Size
		{
			get { return new Vector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}
		public Rectangle Region
		{
			get { return new Rectangle(X, Y, Width, Height); }
			set { X = value.Left; Y = value.Top; Width = value.Width; Height = value.Height; }
		}


		public Point PointToLocal(Point p)
		{
			var origin = Parent?.DisplayLocation ?? Point.Zero;
			return new Point(p.X - origin.X - X, p.Y - origin.Y - Y);
		}

		public Point PointToGlobal(Point p)
		{
			var origin = Parent?.DisplayLocation ?? Point.Zero;
			return new Point(X + origin.X + p.X, Y + origin.Y + p.Y);
		}

		public Rectangle ToGlobal(Rectangle rectangle)
		{
			return new Rectangle(PointToGlobal(rectangle.TopLeft), rectangle.Size);
		}

		/// <summary>
		/// Gets the actual location in the room of this GRaff.Forms.DisplayObject.
		/// </summary>
		public Point DisplayLocation
		 => Parent?.PointToGlobal(Location) ?? Location;

		public Rectangle DisplayRegion
		 => new Rectangle(DisplayLocation, DisplaySize);

		public Vector DisplaySize
			=> Size * DisplayScale;

		public Vector DisplayScale
			=> (Parent?.DisplayScale ?? new Vector(1, 1)) * Scale;


		public double XScale { get; set; } = 1;
		public double YScale { get; set; } = 1;
		public Vector Scale => new Vector(XScale, YScale);

		public virtual void OnPaint()
		{ }

		public sealed override void OnDraw()
		{
			using (Scissor.UseIntersection((IntRectangle)DisplayRegion))
			{
				OnPaint();
				var previous = View.FocusRegion;
				View.InverseFocusRegion(Region, Size * Scale);
				View.Refresh();
				foreach (var child in _children)
					child.OnDraw();
				View.FocusRegion = previous;
				View.Refresh();
			}
		}
	}
}
