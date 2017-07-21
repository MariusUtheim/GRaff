using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Panels
{
    public class Node
    {

        public Node() { }

        public virtual Rectangle Region { get; set; }


        public Point Location
        {
            get => Region.TopLeft;
            set => Region = new Rectangle(value, Region.Size);
        }

        public double X
        {
            get => Region.Left;
            set => Region = new Rectangle(value, Region.Top, Region.Width, Region.Height);
        }

        public double Y
        {
            get => Region.Top;
            set => Region = new Rectangle(Region.Top, value, Region.Width, Region.Height);
        }

        private Node _parent;
        public Node Parent
        {
            get => _parent;
            set
            {
                if (value == _parent)
                    return;
                OnParentChanging(value);
                _parent?._children.Remove(this);
                value?._children.AddLast(this);
            }
        }

#warning Make children keep track of their LinkedListNode
        private LinkedList<Node> _children = new LinkedList<Node>();
        public IEnumerable<Node> Children => _children.ToList();

        public bool IsMouseHovering { get; internal set; }

        public virtual void OnParentChanging(Node parent) { }

		public TNode AddChildLast<TNode>(TNode element) where TNode : Node
		{
            Contract.Requires<ArgumentNullException>(element != null);
			if (element._parent != this)
			{
                element.OnParentChanging(this);
				element._parent?._children.Remove(element);
				element._parent = this;
				_children.AddLast(element);
			}
            return element;
		}

        public TNode AddChildFirst<TNode>(TNode element) where TNode : Node
        {
			Contract.Requires<ArgumentNullException>(element != null);
			if (element._parent != this)
			{
                element.OnParentChanging(this);
				element._parent?._children.Remove(element);
				element._parent = this;
                _children.AddFirst(element);
			}
			return element;
        }

		public void RemoveChild(Node element)
		{
			if (element.Parent == this)
			{
				element._parent = null;
				_children.Remove(element);
			}
		}


        public Point ToClient(Point point)
        {
			if (Parent == null)
                return (Point)(point - Region.TopLeft);
			else
                return (Point)(Parent.ToClient(point) - Region.TopLeft);
        }
        public Vector ToClient(Vector vector)
        {
            return vector;
        }
        public Rectangle ToClient(Rectangle rect) => new Rectangle(ToClient(rect.TopLeft), ToClient(rect.Size));
        public Line ToClient(Line line) => new Line(ToClient(line.Origin), ToClient(line.Direction));

        public Point ToParent(Point point)
        {
            if (Parent == null)
                return point;
            else
                return Parent.ToClient(point);
        }
        public Vector ToParent(Vector vector)
        {
            return vector;
        }
        public Rectangle ToParent(Rectangle rect) => new Rectangle(ToParent(rect.TopLeft), ToParent(rect.Size));
        public Line ToParent(Line line) => new Line(ToParent(line.Origin), ToParent(line.Direction));


        public virtual void OnStep() { }

        public virtual void OnDraw() { }

        public virtual void OnMouseHover(MouseEventArgs e) { }

        // Return whetner the event should propagate
        internal bool _MouseEvent<TInterface>(Action<TInterface, MouseEventArgs> action, MouseEventArgs e)
        {
            if (!Region.Contains(e.Location))
                return true;

            foreach (var child in _children.Reverse())
            {
				var nextE = new MouseEventArgs(e.Button, (Point)(e.Location - Region.TopLeft), e.WheelDelta);
				if (!child._MouseEvent(action, nextE))
                    return false;
            }

			if (this is TInterface)
			{
				action((TInterface)(object)this, e);
                if (e.IsHandled)
                    return false;
			}

            return true;
		}

        internal void _Step()
        {
            OnStep();
            foreach (var child in _children)
                child._Step();
        }

        internal void _Draw()
        {
            OnDraw();
            //using (View.UMap(X, Y, View.Width, View.Height, View.Rotation))
            using (View.DrawTo(Region.TopLeft).Use())
                foreach (var child in _children)
                    child._Draw();
        }
    }
}
