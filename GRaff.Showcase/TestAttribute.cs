using System;

namespace GRaff.Showcase
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class TestAttribute : Attribute
	{
		public int Order { get; set; } = 0;
	}
}