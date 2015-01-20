using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	//
	// Summary:
	//     Used in GL.BufferData
	internal enum UsageHint
	{
		//
		// Summary:
		//     Original was GL_STREAM_DRAW = 0x88E0
		StreamDraw = 35040,
		//
		// Summary:
		//     Original was GL_STREAM_READ = 0x88E1
		StreamRead = 35041,
		//
		// Summary:
		//     Original was GL_STREAM_COPY = 0x88E2
		StreamCopy = 35042,
		//
		// Summary:
		//     Original was GL_STATIC_DRAW = 0x88E4
		StaticDraw = 35044,
		//
		// Summary:
		//     Original was GL_STATIC_READ = 0x88E5
		StaticRead = 35045,
		//
		// Summary:
		//     Original was GL_STATIC_COPY = 0x88E6
		StaticCopy = 35046,
		//
		// Summary:
		//     Original was GL_DYNAMIC_DRAW = 0x88E8
		DynamicDraw = 35048,
		//
		// Summary:
		//     Original was GL_DYNAMIC_READ = 0x88E9
		DynamicRead = 35049,
		//
		// Summary:
		//     Original was GL_DYNAMIC_COPY = 0x88EA
		DynamicCopy = 35050
	}
}
