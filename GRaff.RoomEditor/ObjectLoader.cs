using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.RoomEditor
{
	public class ObjectLoader
	{
		public static IEnumerable<Type> LoadAssembly(Assembly assembly)
		{
			return assembly.DefinedTypes
					.Where(t => t.GetCustomAttribute(typeof(EditorPlaceableAttribute)) != null).ToArray();
		}
	}
}
