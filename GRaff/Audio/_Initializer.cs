using System;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace GRaff.Audio
{
	internal static class _Initializer
	{
		public static void Initialize()
		{
			IntPtr device = Alc.OpenDevice("");
			ContextHandle context = Alc.CreateContext(device, new int[0]);
			Alc.MakeContextCurrent(context);
		}
	}
}
