using System;
using OpenTK;
using OpenTK.Audio.OpenAL;
using System.Diagnostics;

namespace GRaff.Audio
{
	internal static class _Audio
	{
		public static void Initialize()
		{
			IntPtr device = Alc.OpenDevice("");
			ContextHandle context = Alc.CreateContext(device, new int[0]);
			Alc.MakeContextCurrent(context);

#if DEBUG
            GlobalEvent.EndStep += () =>
            {
                _Audio.ErrorCheck();
            };
#endif
        }

        [Conditional("DEBUG")]
        public static void ErrorCheck()
        {
            var err = AL.GetError();
            if (err != ALError.NoError)
                throw new Exception($"An OpenAL operation threw an exception with error code {Enum.GetName(typeof(ALError), err)}");
        }

#warning
        //[Conditional("DEBUG")]
        public static bool ClearError()
        {
            var err = AL.GetError();
            if (err != ALError.NoError)
            {
                Console.WriteLine($"[{nameof(_Audio)}] Cleared audio error {err}");
                return true;
            }
            else
                return false;
        }
    }
}
