using System;
using System.Diagnostics;
using System.Threading;
using GameMaker;
using GameMaker.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class SoundTest
	{
		private readonly Sound theSound;
		private const string testWav = @"C:/test/testwav.wav";
		private const string testOgg = @"C:/test/testogg.ogg";
		private const int tolerance = 100;

		public SoundTest()
		{
			theSound = new Sound(testWav);
			theSound.Load();
		}

		[TestMethod]
		public void PlayPauseStopTest()
		{
			var instance = theSound.Play();
			Assert.AreEqual(SoundState.Playing, instance.State);
			instance.Pause();
			Assert.AreEqual(SoundState.Paused, instance.State);
			instance.Stop();
			Assert.AreEqual(SoundState.Stopped, instance.State);
		}

		[TestMethod]
		public void DurationTest()
		{
			int dt = (int)(1000 * theSound.Duration);
			
			var instance = theSound.Play();

			Thread.Sleep(dt - tolerance);
			Assert.AreEqual(SoundState.Playing, instance.State);
			Thread.Sleep(2 * tolerance);
			Assert.AreNotEqual(SoundState.Playing, instance.State);
		}

		[TestMethod]
		public void PitchTest()
		{
			int dt = (int)(1000 * theSound.Duration);

			var instance = theSound.Play(pitch: 0.5);
			Thread.Sleep(2 * dt - tolerance);
			Assert.AreEqual(SoundState.Playing, instance.State);
			Thread.Sleep(2 * tolerance);
			Assert.AreNotEqual(SoundState.Playing, instance.State);

			instance = theSound.Play(pitch: 2);
			Thread.Sleep((dt - tolerance) / 2);
			Assert.AreEqual(SoundState.Playing, instance.State);
			Thread.Sleep(tolerance);
			Assert.AreNotEqual(SoundState.Playing, instance.State);
		}

		[TestMethod]
		public void LoopTest()
		{
			int dt = (int)(1000 * theSound.Duration);

			var instance = theSound.Play(loop: true);

		}
	}
}
