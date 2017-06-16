using System;


namespace GRaff.Audio
{
    internal class SimpleSoundElement : SoundElement
    {
        public SimpleSoundElement(SoundBuffer buffer, bool looping, double volume, double pitch, Point location)
        {
            this.Buffer = buffer;
            Source.IsLooping = looping;

            Source.Volume = volume;
            Source.Pitch = pitch;
            Source.Location = location;
            Source.Buffer = buffer;

        }

        public SoundBuffer Buffer { get; }


		public override void OnStep()
		{
			if (Source.State == SoundState.Stopped)
			    this.Destroy();
		}

        protected override void OnDestroy()
        {
            base.OnDestroy();
			Buffer.Remove(this);
        }
    }
}
