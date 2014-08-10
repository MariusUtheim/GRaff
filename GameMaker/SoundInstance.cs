using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio.OpenAL;

namespace GameMaker
{
	public class SoundInstance
	{
		private int _bid;
		private int _sid;

		public SoundInstance(int _bid, bool looping, double volume, double pitch)
		{
			this._bid = _bid;
			this._sid = AL.GenSource();
			AL.Source(_sid, ALSourcei.Buffer, _bid);
		}

#warning TODO: Argument asserts
		public bool Looping
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

		public double Pitch
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

		public double Volume
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

		public void Play()
		{
			AL.SourcePlay(_sid);
		}

		public void Stop()
		{
			AL.SourceStop(_sid);
		}

		public void Pause()
		{
			AL.SourcePause(_sid);
		}

		public SoundState State
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
						throw new Exception("Internal error in SoundInstance.cs");
				}
			}
		}
	}
}
