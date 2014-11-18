using System;
using System.Collections.Generic;
using OpenTK;

namespace GRaff.RoomEditor
{
	public class GiraffeEditor : GameElement
	{
		private static readonly IntVector RectSize = new IntVector(48, 48);
		private List<IEditorPlaceable> _objects;

		public override void OnDraw()
		{
			foreach (var obj in _objects)
			{
				//obj.EditorDraw(obj.X, obj.Y);
			}
		}
	}
}
