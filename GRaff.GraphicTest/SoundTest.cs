using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;
using GRaff.Audio;

namespace GRaff.GraphicTest
{
	[Test(Order = -1)]
	class SoundTest : GameElement, IKeyPressListener
	{
		SoundBuffer buffer = SoundBuffer.Load(@"C:\test\sound.wav");
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
                if (instance?.State == SoundState.Paused)
                    instance.Resume();
                else
                    instance = buffer.Play(false, volume: 3);
            }
            else if (key == Key.L)
                instance = buffer.Play(true);
            else if (key == Key.O)
                instance = bufferWithOffset.Play(true);
            else if (key == Key.U)
                instance?.Pause();
            else if (key == Key.S)
            {
                instance?.Destroy();
                instance = null;
            }
		}

        public override void OnStep()
        {
            Window.Title = instance?.State.ToString() ?? "No sound is created";
        }

        public override void OnDraw()
        {
            if (instance != null)
            {
                var loc = instance.Completion * Room.Current.Width;
                Draw.Line(Colors.DarkGreen, loc, 0, loc, Room.Current.Height);
            }
        }

        public override void OnDestroy()
		{
            if (_Audio.ClearError())
            { }
            instance?.Destroy();
            _Audio.ErrorCheck();
			buffer.Dispose();
            _Audio.ErrorCheck();
        //    bufferWithOffset?.Dispose();
		}
	}
}
