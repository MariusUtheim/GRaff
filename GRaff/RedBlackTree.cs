using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	class RedBlackTree : ICollection<GameElement>
	{
		
		internal RBTreeNode _root;

		public int Count
		{
			get; private set;
		}

		public bool IsReadOnly => false;

		public void Add(GameElement item)
		{
			var node = new RBTreeNode(this, item);
			Count += 1;
			if (_root == null)
				_root = node;
			else
				_root.Add(node);

			node.Inserted();

			var structure = _root.Trace();
			_root.AssertStructure();
		}

		public void Clear()
		{
			_root = null;
		}

		public bool Contains(GameElement item)
		{
			return _root?.Find(item) != null;
		}

		public void CopyTo(GameElement[] array, int arrayIndex)
		{
			_root.CopyTo(array, ref arrayIndex);
		}

		public bool Remove(GameElement item)
		{
			var node = _root?.Find(item);

			if (node == null)
				return false;


			if (_root.IsLeaf)
			{
				_root = null;
				return true;
			}

			node.Remove();
			Count -= 1;
			return true;
		}

		public IEnumerator<GameElement> GetEnumerator() => new RBTreeEnumerator(this);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	}

	class RBTreeNode
	{
		private const bool Red = false;
		private const bool Black = true;

		private bool _color = Red;
		private RedBlackTree _tree;

		public RBTreeNode(RedBlackTree tree, GameElement element)
		{
			this._tree = tree;
			this.Element = element;
		}

		private static bool GetColor(RBTreeNode node) => node?._color ?? Black;

		public RBTreeNode this[RBTreeNode childReference]
		{
			set
			{
				if (childReference == _leftChild)
					_leftChild = value;
				else
				{
					Debug.Assert(_rightChild == childReference);
					_rightChild = value;
				}
			}
		}

		public GameElement Element { get; private set; }

		private RBTreeNode _parent;
		public RBTreeNode Parent => _parent;

		private RBTreeNode _leftChild;
		public RBTreeNode LeftChild
		{
			get { return _leftChild; }
			private set
			{
				_leftChild = value;
				if (value != null)
					value._parent = this;
			}
		}

		internal RBTreeNode LeftmostChild => LeftChild?.LeftmostChild ?? this;
		internal RBTreeNode RightmostChild => RightChild?.RightmostChild ?? this;


		private RBTreeNode _rightChild;
		public RBTreeNode RightChild
		{
			get { return _rightChild; }
			private set
			{
				_rightChild = value;
				if (value != null)
					value._parent = this;
			}
		}

		public RBTreeNode Grandparent => Parent?.Parent;

		public RBTreeNode Uncle
		{
			get
			{
				var grandparent = Grandparent;
				if (grandparent == null)
					return null;
				if (grandparent.LeftChild == Parent)
					return grandparent.RightChild;
				else
					return grandparent.LeftChild;
			}
		}
		public RBTreeNode Sibling => (Parent?.LeftChild == this) ? Parent?.RightChild : Parent?.LeftChild;
		private RBTreeNode Child => LeftChild ?? RightChild;
		internal bool IsLeaf => LeftChild == null && RightChild == null;


		public void Add(RBTreeNode node)
		{
			if (node.Element.Depth < this.Element.Depth)
			{
				if (LeftChild == null)
					LeftChild = node;
				else
					LeftChild.Add(node);
			}
			else
			{
				if (RightChild == null)
					RightChild = node;
				else
					RightChild.Add(node);
			}
		}

		internal void Inserted()
		{
			if (Parent == null)
				_color = Black;

			else if (Parent._color == Black)
				return;

			// NOTE: Since the parent is red, there must be a grandparent
			else if (Uncle?._color.Equals(Red) ?? false)
			{
				Grandparent.Flip();
			}

			else
			{
				if (Parent.LeftChild == this && Grandparent.RightChild == Parent)
				{
					Parent.RotateRight();
					_color = Black;
					Parent._color = Red;
					Parent.RotateLeft();
				}
				else if (Parent.RightChild == this && Grandparent.LeftChild == Parent)
				{
					Parent.RotateLeft();
					_color = Black;
					Parent._color = Red;
					Parent.RotateRight();
				}
				else
				{
					Parent._color = Black;
					Grandparent._color = Red;
					if (Parent.LeftChild == this)
						Grandparent.RotateRight();
					else
						Grandparent.RotateLeft();
				}

			}
		}

		public void Remove()
		{
			RBTreeNode target;

			if (LeftChild == null && RightChild == null)
				target = this;
			else if (LeftChild == null)
				target = RightChild.LeftmostChild;
			else
				target = LeftChild.RightmostChild;

			this.Element = target.Element;

			target.Delete();

		}

		private void Delete()
		{
			Debug.Assert(IsLeaf);

			if (_color == Black)
				Rebalance();

			_parent[this] = null;
		}

		private void Rebalance()
		{
			var parent = Parent;

			if (parent == null)
				return;

			var sibling = Sibling;
			var siblingIsLeftChild = Parent.LeftChild == sibling;

			if (sibling._color == Red)
			{
				parent._color = Red;
				sibling._color = Black;
				if (siblingIsLeftChild)
					parent.RotateRight();
				else
					parent.RotateLeft();
			}
			// Sibling is black

			else if (GetColor(sibling._leftChild) == Black
				  && GetColor(sibling._rightChild) == Black)
			{
				// Sibling is both of Sibling's children are black
				sibling._color = Red;
				if (parent._color == Red)
					parent._color = Black;
				else
					parent.Rebalance();
			}
			else
			{
				if (siblingIsLeftChild && GetColor(sibling.LeftChild) == Black && GetColor(sibling.RightChild) == Red)
				{
					var nephew = sibling._rightChild;
					sibling._color = Red;
					nephew._color = Black;
					sibling.RotateLeft();
					sibling = nephew;
				}
				else if (!siblingIsLeftChild && GetColor(sibling.LeftChild) == Red && GetColor(sibling.RightChild) == Black)
				{
					var nephew = sibling._leftChild;
					sibling._color = Red;
					nephew._color = Black;
					sibling.RotateRight();
					sibling = nephew;
				}

				// if sibling is left child, sibling's left child is red; if sibling is right child, sibling's right child is red
				sibling._color = parent._color;
				parent._color = Black;

				if (siblingIsLeftChild)
				{
					sibling._leftChild._color = Black;
					parent.RotateRight();
				}
				else
				{
					sibling._rightChild._color = Black;
					parent.RotateLeft();
				}
			}
		}

		internal RBTreeNode Find(GameElement element)
		{
			if (element == this.Element)
				return this;
			else if (element.Depth >= this.Element.Depth)
				return RightChild?.Find(element);
			else
				return LeftChild?.Find(element);
		}

		private void Flip()
		{
			_color = Red;
			LeftChild._color = Black;
			RightChild._color = Black;
			Inserted();
		}

		private void RotateLeft()
		{
			var originalParent = _parent;
			var originalChild = _rightChild;

			this.RightChild = originalChild.LeftChild;
			originalChild.LeftChild = this;

			originalChild._parent = originalParent;
			if (originalParent == null)
				_tree._root = originalChild;
			else
				originalParent[this] = originalChild;
		}

		private void RotateRight()
		{
			var originalParent = Parent;
			var child = LeftChild;

			this.LeftChild = child.RightChild;
			child.RightChild = this;

			child._parent = originalParent;
			if (originalParent == null)
				_tree._root = child;
			else
				originalParent[this] = child;
		}

		internal void CopyTo(GameElement[] array, ref int index)
		{
			_leftChild?.CopyTo(array, ref index);
			array[index++] = Element;
			_rightChild?.CopyTo(array, ref index);
		}

		internal IEnumerable<GameElement> Enumerate()
		{
			if (LeftChild != null)
				foreach (var element in LeftChild.Enumerate())
					yield return element;

			yield return this.Element;

			if (RightChild != null)
				foreach (var element in RightChild.Enumerate())
					yield return element;
		}

		public override string ToString()
		{
			return String.Format($"RBTreeNode Depth={Element.Depth} Color={{0}}", _color == Black ? Black : Red);
		}

		[Conditional("DEBUG")]
		internal void AssertStructure()
		{
			int i = 0;
			AssertStructure(true, ref i);
		}

		[Conditional("DEBUG")]
		internal void AssertStructure(bool requireBlack, ref int i)
		{
			if (requireBlack)
				Debug.Assert(_color == Black);

			int leftCount = 0, rightCount = 0;
			Debug.Assert(_leftChild == null || _leftChild.Parent == this);
			Debug.Assert(_rightChild == null || _rightChild.Parent == this);
			_leftChild?.AssertStructure(_color == Red, ref leftCount);
			_rightChild?.AssertStructure(_color == Red, ref rightCount);

			if (leftCount != rightCount)
			{
				var str = Trace();
			}
			Debug.Assert(leftCount == rightCount);

			i = leftCount + (_color == Black ? 1 : 0);
		}

		internal string Trace(int depth = 0)
		{
			string str = new String(Enumerable.Repeat(_color == Black ? 'X' : '-', depth).ToArray()) + " " + Element.Depth + Environment.NewLine;
			str += LeftChild?.Trace(depth + 1);
			str += RightChild?.Trace(depth + 1);
			return str;
		}
	}

	class RBTreeEnumerator : IEnumerator<GameElement>
	{
		private RedBlackTree _tree;
		private RBTreeNode _current = null;

		public RBTreeEnumerator(RedBlackTree tree)
		{
			_tree = tree;
		}

		public GameElement Current => _current.Element;

		object IEnumerator.Current => Current;

		public void Dispose() => _current = null;

		public bool MoveNext()
		{
			if (_current == null)
			{
				_current = _tree?._root.LeftmostChild;
				return true;
			}
			else if (_current.RightChild != null)
			{
				_current = _current.RightChild.LeftmostChild;
				return true;
			}
			else
			{
				for (; ;)
				{
					var previous = _current;
					_current = _current.Parent;
					if (_current == null)
						return false;
					else if (_current.LeftChild == previous)
						return true;
				}
			}
		}

		public void Reset() => _current = null;
	}
}
