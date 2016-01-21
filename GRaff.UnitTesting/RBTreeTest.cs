using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRaff;
using System.Linq;
using System.Collections.Generic;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class RBTreeTest
	{
		class TestElement : GameElement
		{
			public int Tag;

			public TestElement(int depth)
				: this(depth, depth)
			{ }

			public TestElement(int depth, int tag)
			{
				Depth = depth;
				Tag = tag;
			}

			public override string ToString()
			{
				return $"Depth={Depth}; Tag={Tag}";
			}
		}

		[TestMethod]
		public void RBTree_Add()
		{
			var collection = new RedBlackTree();
			var indices = GRandom.Shuffle(Enumerable.Range(0, 100).ToArray());
			var elements = indices.Select(i => new TestElement(i)).ToList();

			for (int i = 0; i < elements.Count; i++)
				collection.Add(elements[i]);

			var results = collection.ToArray();
			indices = indices.OrderBy(i => i).ToArray();
			for (int i = 0; i < indices.Length; i++)
				Assert.AreEqual(indices[i], results[i].Depth);
		}

		[TestMethod]
		public void RBTree_Remove()
		{
			var collection = new RedBlackTree();
			var indices = new List<int>(new[] { 1, 6, 8, 11, 15, 13, 22, 17, 27, 25 });
			var elements = indices.Select(i => new TestElement(i)).ToList();

			for (int i = 0; i < elements.Count; i++)
				collection.Add(elements[i]);

			
			collection.Remove(elements[0]);
			collection.Remove(elements[3]);
			collection.Remove(elements[5]);
			indices.RemoveAt(5);
			indices.RemoveAt(3);
			indices.RemoveAt(0);
			elements.RemoveAt(5);
			elements.RemoveAt(3);
			elements.RemoveAt(0);

			var results = collection.ToArray();

			indices = indices.OrderBy(i => i).ToList();
			for (int i = 0; i < indices.Count; i++)
				Assert.AreEqual(indices[i], results[i].Depth);
		}


		[TestMethod]
		public void RBTree_Enumerate()
		{
			var collection = new RedBlackTree();
			var indices = GRandom.Shuffle(Enumerable.Range(0, 100).ToArray());
			var elements = indices.Select(i => new TestElement(i)).ToList();

			foreach (var element in elements)
				collection.Add(element);

			indices = indices.OrderBy(i => i).ToArray();
			var index = 0;
			foreach (var element in collection)
				Assert.AreEqual(indices[index++], element.Depth);
		}
	}
}
