using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;
using GRaff.Audio;

namespace GRaff.GraphicTest
{
	[Test]
	class SoundTest : GameElement, IKeyPressListener
	{
		SoundBuffer buffer = SoundBuffer.Load(@"C:\test\sound.ogg");
        SoundBuffer bufferWithOffset = null;// SoundBuffer.LoadWithOffset(@"C:\test\sound.wav", 1);
		SoundElement instance;

		public SoundTest()
		{
			//bufferWithOffset = new SoundBuffer(buffer, 0.5);
		}

		public void OnKeyPress(Key key)
		{
            if (key == Key.P)
            {
                if (instance?.Source.State == SoundState.Paused)
                    instance.Source.Play();
                else
                    instance = buffer.Play(false, volume: 3);
            }
            else if (key == Key.L)
                instance = buffer.Play(true);
            else if (key == Key.O)
                instance = bufferWithOffset.Play(true);
            else if (key == Key.U)
                instance?.Source.Pause();
            else if (key == Key.S)
            {
                instance?.Destroy();
                instance = null;
            }
		}

        public override void OnStep()
        {
            Window.Title = instance?.Source.State.ToString() ?? "No sound is created";
        }

        public override void OnDraw()
        {
            if (instance != null)
            {
                var loc = instance.Completion * Room.Current.Width;
                Draw.Line(Colors.DarkGreen, loc, 0, loc, Room.Current.Height);
            }
        }

        protected override void OnDestroy()
		{
            instance?.Destroy();
			buffer.Dispose();
        //    bufferWithOffset?.Dispose();
		}
	}
}
