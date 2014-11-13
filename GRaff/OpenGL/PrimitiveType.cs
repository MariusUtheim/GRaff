using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.OpenGL
{
	//
	// Summary:
	//     Used in GL.Angle.DrawArraysInstanced, GL.Angle.DrawElementsInstanced and 11 other
	//     functions
	public enum PrimitiveType
	{
		//
		// Summary:
		//     Original was GL_POINTS = 0x0000
		Points,
		//
		// Summary:
		//     Original was GL_LINES = 0x0001
		Lines,
		//
		// Summary:
		//     Original was GL_LINE_LOOP = 0x0002
		LineLoop,
		//
		// Summary:
		//     Original was GL_LINE_STRIP = 0x0003
		LineStrip,
		//
		// Summary:
		//     Original was GL_TRIANGLES = 0x0004
		Triangles,
		//
		// Summary:
		//     Original was GL_TRIANGLE_STRIP = 0x0005
		TriangleStrip,
		//
		// Summary:
		//     Original was GL_TRIANGLE_FAN = 0x0006
		TriangleFan,
		//
		// Summary:
		//     Original was GL_QUADS = 0x0007
		Quads,
		//
		// Summary:
		//     Original was GL_QUAD_STRIP = 0x0008
		QuadStrip,
		//
		// Summary:
		//     Original was GL_POLYGON = 0x0009
		Polygon,
		//
		// Summary:
		//     Original was GL_LINES_ADJACENCY = 0x000A
		LinesAdjacency,
		//
		// Summary:
		//     Original was GL_LINES_ADJACENCY_ARB = 0x000A
		LinesAdjacencyArb = 10,
		//
		// Summary:
		//     Original was GL_LINES_ADJACENCY_EXT = 0x000A
		LinesAdjacencyExt = 10,
		//
		// Summary:
		//     Original was GL_LINE_STRIP_ADJACENCY = 0x000B
		LineStripAdjacency = 11,
		//
		// Summary:
		//     Original was GL_LINE_STRIP_ADJACENCY_ARB = 0x000B
		LineStripAdjacencyArb = 11,
		//
		// Summary:
		//     Original was GL_LINE_STRIP_ADJACENCY_EXT = 0x000B
		LineStripAdjacencyExt = 11,
		//
		// Summary:
		//     Original was GL_TRIANGLES_ADJACENCY = 0x000C
		TrianglesAdjacency = 12,
		//
		// Summary:
		//     Original was GL_TRIANGLES_ADJACENCY_ARB = 0x000C
		TrianglesAdjacencyArb = 12,
		//
		// Summary:
		//     Original was GL_TRIANGLES_ADJACENCY_EXT = 0x000C
		TrianglesAdjacencyExt = 12,
		//
		// Summary:
		//     Original was GL_TRIANGLE_STRIP_ADJACENCY = 0x000D
		TriangleStripAdjacency = 13,
		//
		// Summary:
		//     Original was GL_TRIANGLE_STRIP_ADJACENCY_ARB = 0x000D
		TriangleStripAdjacencyArb = 13,
		//
		// Summary:
		//     Original was GL_TRIANGLE_STRIP_ADJACENCY_EXT = 0x000D
		TriangleStripAdjacencyExt = 13,
		//
		// Summary:
		//     Original was GL_PATCHES = 0x000E
		Patches = 14
	}
}
