using GRaff.Graphics;
using GRaff.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
    [Test]
    class ShadersTest : GameElement, IKeyPressListener
    {
        private ColorMatrixShaderProgram blackWhiteShader = new ColorMatrixShaderProgram(0.333, 0.333, 0.333, 0.333, 0.333, 0.333, 0.333, 0.333, 0.333);
        private ColorMatrixShaderProgram sepiaShader = new ColorMatrixShaderProgram(0.393, 0.349, 0.272, 0.769, 0.686, 0.534, 0.189, 0.168, 0.131);
        private CausticShaderProgram causticShader = new CausticShaderProgram(new Rectangle(0, 0, 500, 500));
        private SpotlightShaderProgram lightShader = new SpotlightShaderProgram(200, 300);
        private ShaderProgram _currentProgram;
        
        public ShadersTest()
        {
            _setShader(ShaderProgram.Default, "Default");
        }

        public override void OnDraw()
        {
            lightShader.Origin = (Mouse.WindowX * Window.DisplayScale.X, Mouse.WindowY * Window.DisplayScale.Y);
            causticShader.Phase = Time.LoopCount;

            using (_currentProgram.Use())
            {
                Draw.FillRectangle(Colors.Black, Room.Current.ClientRectangle);
                Draw.FillTriangle(Colors.Blue, (200, 50), (400, 300), (100, 400));
                Draw.FillRectangle(Colors.ForestGreen, (100, 500), (400, 400 / GMath.Phi));
                Draw.Texture(TextureBuffers.Giraffe.Texture, (500, 0));
            }
            
        }

        private void _setShader(ShaderProgram program, string name)
        {
            _currentProgram = program;
            Window.Title = "ShaderTest - " + name;
        }

        public void OnKeyPress(Key key)
        {
            switch(key)
            {
                case Key.Number1: _setShader(ShaderProgram.Default, "Default"); break;
                case Key.Number2: _setShader(blackWhiteShader, "Black/White"); break;
                case Key.Number3: _setShader(sepiaShader, "Sepia"); break;
                case Key.Number4: _setShader(causticShader, "Caustic"); break;
                case Key.Number5: _setShader(lightShader, "Spotlight"); break;
            }
        }
    }
}
