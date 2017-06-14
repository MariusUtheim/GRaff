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
        SoundBuffer buffer;
        SoundBuffer bufferWithOffset = null;// SoundBuffer.LoadWithOffset(@"C:\test\sound.wav", 1);
		SoundElement instance;

		public SoundTest()
		{
            SoundBuffer.LoadAsync(@"Assets/Panacea.wav").ThenWait(b => this.buffer = b);
		}

		public void OnKeyPress(Key key)
		{
            switch (key)
            {
                case Key.P:
                    if (instance?.Source.State == SoundState.Paused)
                        instance.Source.Play();
                    else
                        instance = buffer.Play(false, volume: 3);
                    break;

                case Key.N:
                    buffer.Play(true);
                    break;

                case Key.O:
                    instance = bufferWithOffset.Play(true);
                    break;

                case Key.U:
                    instance?.Source.Pause();
                    break;

                case Key.S:
                    if (Keyboard.IsDown(Key.ShiftLeft))
                        buffer.StopAll();
                    else
                    {
                        instance?.Destroy();
                        instance = null;
                    }
                    break;
            }
        }

        public override void OnStep()
        {
            Window.Title = instance?.Source.State.ToString() ?? "No sound is created";
        }

        public override void OnDraw()
        {
            Draw.Clear(Colors.Black);
            if (instance != null && !instance.Source.IsDisposed)
            {
                var completion = instance.Source.Offset.Seconds / instance.Source.Buffer.Duration.Seconds;
                var loc = completion * Room.Current.Width;
                Draw.Line(Colors.DarkGreen, (loc, 0.0), (loc, Room.Current.Height));
            }
        }

        protected override void OnDestroy()
		{
        	buffer.Dispose();
        //    bufferWithOffset?.Dispose();
		}
	}
}
