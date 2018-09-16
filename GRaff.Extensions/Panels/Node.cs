using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Panels
{
    public class Node
    {
        private LinkedListNode<Node> _listNode;

        public Node()
        {
            _listNode = new LinkedListNode<Node>(this);
        }

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

        public virtual bool ContainsPoint(Point location) => Region.Contains(location);

        public Node GetChildAt(Point location)
        {
            var child = Children.Reverse().FirstOrDefault(n => n.ContainsPoint(location));
            if (child == null)
                return null;
            else
                return child.GetChildAt(child._fromParent(location)) ?? child;
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
                _parent?._children.Remove(_listNode);
                value?._children.AddLast(_listNode);
            }
        }

        private PanelElement _container;
        public PanelElement Container
        {
            get => _container ?? _parent?.Container;
            internal set => _container = value; // Do this ONLY in PanelElement constructor!
        }

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
				element._parent?._children.Remove(element._listNode);
				element._parent = this;
				_children.AddLast(element._listNode);
			}
            return element;
		}

        public TNode AddChildFirst<TNode>(TNode element) where TNode : Node
        {
			Contract.Requires<ArgumentNullException>(element != null);
			if (element._parent != this)
			{
                element.OnParentChanging(this);
				element._parent?._children.Remove(element._listNode);
				element._parent = this;
                _children.AddFirst(element._listNode);
			}
			return element;
        }

		public void RemoveChild(Node element)
		{
			if (element != null && element.Parent == this)
			{
				element._parent = null;
				_children.Remove(element._listNode);
			}
		}

        public void RemoveFirstChild() => RemoveChild(_children.First());

        public void RemoveLastChild() => RemoveChild(_children.Last());

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

        private Point _fromParent(Point point) => (Point)(point - Region.TopLeft);

        public virtual void OnStep() { }

        public virtual void OnDraw() { }

        public virtual void OnMouseHover(MouseEventArgs e) { }

        // Return whether the event should propagate
        internal bool _MouseEvent<TInterface>(Action<TInterface, MouseEventArgs> action, MouseEventArgs e)
        {
            if (!ContainsPoint(e.Location))
                return true;

            foreach (var child in _children.Reverse())
            {
                var nextE = new MouseEventArgs(e.Button, _fromParent(e.Location), e.WheelDelta);
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
            using (View.TranslateTo(Region.TopLeft).Use())
                foreach (var child in _children)
                    child._Draw();
        }
    }
}
