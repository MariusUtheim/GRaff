using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{

	public enum PrimitiveType
	{
        //     Original was GL_POINTS = 0x0000
        Points = 0x0000,
		//     Original was GL_LINES = 0x0001
		Lines = 0x0001,
		//     Original was GL_LINE_LOOP = 0x0002
		LineLoop = 0x0002,
		//     Original was GL_LINE_STRIP = 0x0003
		LineStrip = 0x0003,
		//     Original was GL_TRIANGLES = 0x0004
		Triangles = 0x0004,
		//     Original was GL_TRIANGLE_STRIP = 0x0005
		TriangleStrip = 0x0005,
		//     Original was GL_TRIANGLE_FAN = 0x0006
		TriangleFan = 0x0006,
		//     Original was GL_POLYGON = 0x0009
		Polygon = 0x0009,
		//     Original was GL_LINES_ADJACENCY = 0x000A
		LinesAdjacency = 0x000A,
		//     Original was GL_LINE_STRIP_ADJACENCY = 0x000B
		LineStripAdjacency = 0x000B,
		//     Original was GL_TRIANGLES_ADJACENCY = 0x000C
		TrianglesAdjacency = 0x000C,
		//     Original was GL_TRIANGLE_STRIP_ADJACENCY = 0x000D
		TriangleStripAdjacency = 0x000D,
		//
		// Summary:
		//     Original was GL_PATCHES = 0x000E
		Patches = 0x000E
	}
}
