using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio.OpenAL;

namespace GameMaker.OpenAL
{
	public class OpenALSoundInstance : SoundInstance
	{
		private int _bid;
		private int _sid;

		public OpenALSoundInstance(int _bid)
		{
			this._bid = _bid;
			this._sid = AL.GenSource();
			AL.Source(_sid, ALSourcei.Buffer, _bid);
		}

		public override bool Looping
		{
			get
			{
				bool value;
				AL.GetSource(_sid, ALSourceb.Looping, out value);
				return value;
			}
			set
			{
				AL.Source(_sid, ALSourceb.Looping, value);
			}
		}

		public override double Pitch
		{
			get
			{
				float value;
				AL.GetSource(_sid, ALSourcef.Pitch, out value);
				return value;
			}

			set
			{
				AL.Source(_sid, ALSourcef.Pitch, (float)value);
			}
		}

		public override double Volume
		{
			get
			{
				float value;
				AL.GetSource(_sid, ALSourcef.Gain, out value);
				return value;
			}
			set
			{
				AL.Source(_sid, ALSourcef.Gain, (float)value);
			}
		}

		public override void Play()
		{
			AL.SourcePlay(_sid);
		}

		public override void Stop()
		{
			AL.SourceStop(_sid);
		}

		public override void Pause()
		{
			AL.SourcePause(_sid);
		}

		public override SoundState State
		{
			get 
			{
				switch (AL.GetSourceState(_sid))
				{
					case ALSourceState.Initial:
					case ALSourceState.Stopped:
						return GameMaker.SoundState.Stopped;

					case ALSourceState.Paused:
						return GameMaker.SoundState.Paused;

					case ALSourceState.Playing:
						return GameMaker.SoundState.Playing;

					default:
						throw new Exception("Internal error in OpenALSoundInstance.cs");
				}
			}
		}
	}
}
