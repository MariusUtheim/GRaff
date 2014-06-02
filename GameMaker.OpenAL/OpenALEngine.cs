using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace GameMaker.OpenAL
{
    public class OpenALEngine : SoundEngine
    {
		private IntPtr _device;
		private ContextHandle _context;

		public OpenALEngine()
		{
			_device = Alc.OpenDevice("");
			_context = Alc.CreateContext(_device, new int[0]);
			Alc.MakeContextCurrent(_context);
		}

		public override SoundSample LoadSample(string path)
		{
			return new OpenALSoundSample(path);
		}

		public override SoundInstance Play(SoundSample sound, bool loop, double volume, double pitch)
		{
			var alSample = sound as OpenALSoundSample;
			OpenALSoundInstance instance = new OpenALSoundInstance(alSample.Id);
			instance.Looping = loop;
			instance.Volume = volume;
			instance.Pitch = pitch;
			instance.Play();
			return instance;
		}
	}
}
