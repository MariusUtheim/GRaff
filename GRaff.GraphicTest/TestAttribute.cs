using System;

namespace GRaff.GraphicTest
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class TestAttribute : Attribute
	{
		public int Order { get; set; } = 0;
	}
}