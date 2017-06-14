using System;


namespace GRaff.Audio
{
    internal class SimpleSoundElement : SoundElement
    {
        public SimpleSoundElement(SoundBuffer buffer, bool looping, double volume, double pitch, Point location)
        {
            this.Buffer = buffer;
            Source.Looping = looping;

            Source.Volume = volume;
            Source.Pitch = pitch;
            Source.Location = location;
            Source.Buffer = buffer;

        }

        public SoundBuffer Buffer { get; }

        protected override void OnDestroy()
        {
            base.OnDestroy();
			Buffer.Remove(this);
        }
    }
}
