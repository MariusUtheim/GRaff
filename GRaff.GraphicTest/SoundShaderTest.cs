using System;
using System.Linq;
using GRaff.Audio;
using GRaff.Graphics.Shaders;

namespace GRaff.GraphicTest
{
    [Test]
    public class SoundShaderTest : GameElement
    {
        private SoundVisualizerShaderProgram program;
        private SoundBuffer sound;
        private SoundElement instance;

        public SoundShaderTest()
        {
            sound = SoundBuffer.Load("Assets/PanaceaLong.wav");
            instance = sound.Play(true);

            program = new SoundVisualizerShaderProgram(sound.Buffer.ToArray(), Room.Current.Size, Room.Current.Width);
            program.Origin = Room.Current.Center + (0, 50);
         //   program.Orientation = Angle.Zero;
        }

        public override void OnStep()
        {
            program.Offset = instance.Source.ByteOffset / 2;
            program.Origin = Room.Current.Center + new Vector(50 * GMath.Sin(Time.LoopCount / 2000.0), 50 + GMath.Cos(Time.LoopCount / 1740.0));
            program.Orientation = Angle.Rad(-GMath.Sin(Time.LoopCount / 1000.0));
        }

        public override void OnDraw()
        {
            Draw.Clear(Colors.Black);
            using (program.Use())
                Draw.FillRectangle(Room.Current.ClientRectangle,
                                   Color.Hsv(Angle.Deg(Time.LoopCount * 1.2), 1, 1),
                                   Color.Hsv(Angle.Deg(Time.LoopCount * 1.4 + 60), 1, 1),
                                   Color.Hsv(Angle.Deg(Time.LoopCount * 0.8 + 120), 1, 1),
                                   Color.Hsv(Angle.Deg(Time.LoopCount + 180), 1, 1));
        }

        protected override void OnDestroy()
        {
            sound.Dispose();
            program.Dispose();
        }
    }
}
