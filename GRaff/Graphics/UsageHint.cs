using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	public enum UsageHint
	{
		StreamDraw = 35040,
		StreamRead = 35041,
		StreamCopy = 35042,
		StaticDraw = 35044,
		StaticRead = 35045,
		StaticCopy = 35046,
		DynamicDraw = 35048,
		DynamicRead = 35049,
		DynamicCopy = 35050
	}
}
