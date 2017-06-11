using GRaff.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
    [Test]
    class ColorShaderTest : GameElement, IKeyPressListener
    {
        private ShaderProgram _program = ShaderProgram.Default;
        private Background _background = new Background { Buffer = TextureBuffers.Giraffe, IsTiled = true };

        public ColorShaderTest()
        {
            _setTitle("Default");
        }

        private void _setTitle(string program) => Window.Title = "ColorShaderTest - Shader: " + program;

        public void OnKeyPress(Key key)
        {
            switch (key)
            {
                case Key.Number1:
                    _program = ShaderProgram.Default;
                    _setTitle("Default");
                    break;
            
                case Key.Number2:
                    _program = ShaderProgram.BlackWhite;
                    _setTitle("Black/White");
                    break;
            
                case Key.Number3:
                    _program = ShaderProgram.Sepia;
                    _setTitle("Sepia");
                    break;
            }
        }

        public override void OnDraw()
        {
            using (_program.Use())
                _background.OnDraw();
        }
    }
}
