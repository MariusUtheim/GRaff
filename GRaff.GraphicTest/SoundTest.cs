using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff.GraphicTest
{
	[Test]
	class SoundTest : GameElement, IKeyPressListener
	{
		SoundBuffer buffer = SoundBuffer.Load(@"C:\test\test.wav");
		SoundBuffer bufferWithOffset;
		SoundElement instance;

		public SoundTest()
		{
			bufferWithOffset = new SoundBuffer(buffer, 1);
		}

		public void OnKeyPress(Key key)
		{
			if (key == Key.P)
				instance = buffer.Play(false, volume: 3);
			else if (key == Key.L)
				instance = buffer.Play(true);
			else if (key == Key.O)
				instance = bufferWithOffset.Play(true);
			else if (key == Key.U)
			{
				instance?.Pause();
				Async.Delay(1).ThenQueue(() => instance.Play());
			}
			else if (key == Key.S)
			{
				instance?.Stop();
				instance = null;
			}
		}

		public override void OnDestroy()
		{
			buffer.Dispose();
		}
	}
}
