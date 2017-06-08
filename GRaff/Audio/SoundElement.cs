using System;
using OpenTK.Audio.OpenAL;


namespace GRaff.Audio
{
    /// <summary>
    /// Represents an instance of a sound that is currently playing.
    /// </summary>
#warning Review class
#warning How about positioning?;
    public class SoundElement : GameElement
	{
		private bool _shouldDropIntro;
        internal bool _isDisposed = false;

		internal SoundElement(SoundBuffer buffer, int? introBufferId, int mainBufferId, bool looping, double volume, double pitch, Point location)
		{
            this.Buffer = buffer;
            Source = new SoundSource
            {
                Looping = looping,
                Volume = volume,
                Pitch = pitch,
                Location = location,
                Buffer = buffer
            };

            //if (looping)
			//{
			//	if (introBufferId == null)
			//	{
			//		AL.SourceQueueBuffer(_sid, mainBufferId);
			//	}
			//	else
			//	{
			//		AL.SourceQueueBuffer(_sid, introBufferId.Value);
			//		AL.SourcePlay(_sid);
			//		AL.SourceQueueBuffer(_sid, mainBufferId);
			//		_shouldDropIntro = true;
			//	}
			//}
			//else
			//{
			//	AL.Source(_sid, ALSourcei.Buffer, mainBufferId);
			//	_shouldDropIntro = false;
			//	AL.SourcePlay(_sid);
			//}
		}

        public SoundBuffer Buffer { get; }

        public SoundSource Source { get; }

        public double Completion => Source.SecondsOffset / Buffer.Duration;
        

        public override void OnStep()
		{
			if (Source.State == SoundState.Stopped)
			{
				this.Destroy();
			}
			//else if (_shouldDropIntro && State == SoundState.Playing)
			//{
            //    AL.GetSource(_sid, ALGetSourcei.BuffersProcessed, out int buffersProcessed);
            //    if (buffersProcessed > 0)
			//	{
			//		Console.WriteLine("[SoundElement] Unqueueing buffer");
			//		AL.SourceUnqueueBuffers(_sid, 1);
			//		_shouldDropIntro = false;
			//		Looping = true;
			//	}
			//}
		}

		protected override void OnDestroy()
		{
            if (!_isDisposed)
            {
                _isDisposed = true;

                Source.Dispose();
                _Audio.ErrorCheck();

                Buffer.Remove(this);
                _Audio.ErrorCheck();
            }
        }
	}
}
