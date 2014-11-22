using System;

namespace GRaff
{
	[Flags]
	public enum FontAlignment
	{
		Left = 0x00,
		HorizontalCenter = 0x01,
		Right = 0x02,

		Top = 0x00,
		VerticalCenter = 0x10,
		Bottom = 0x20,

		TopLeft = 0x00,
		TopRight = 0x02,
		BottomLeft = 0x20,
		BottomRight = 0x22,

		Center = 0x11
	}
}