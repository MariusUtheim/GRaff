using System;
using OpenTK.Audio.OpenAL;


namespace GRaff.Audio
{
    /// <summary>
    /// Represents an instance of a sound that is currently playing.
    /// </summary>
    public abstract class SoundElement : GameElement
	{
		protected SoundElement()
		{
            Source = new SoundSource();
		}

        public SoundSource Source { get; }

        protected override void OnDestroy()
        {
            if (!Source.IsDisposed)
            {
                Source.Dispose();
                _Audio.ErrorCheck();
            }

        }
	}
}
