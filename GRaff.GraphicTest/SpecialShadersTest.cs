using GRaff.Graphics;
using GRaff.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
    [Test(Order = -1)]
    class SpecialShadersTest : GameElement, IKeyPressListener
    {
        private CausticShaderProgram causticShader = new CausticShaderProgram(new Rectangle(0, 0, 500, 500));
        private LightShaderProgram lightShader = new LightShaderProgram(200, 300);
        private ShaderProgram _currentProgram = ShaderProgram.Default;
        

        public override void OnDraw()
        {
            lightShader.Origin = (Mouse.WindowX * Window.DisplayScale.X, Mouse.WindowY * Window.DisplayScale.Y);
            causticShader.Phase = Time.LoopCount;

            using (_currentProgram.Use())
            {
                Draw.FillRectangle(Colors.Black, Room.Current.ClientRectangle);
                Draw.FillTriangle(Colors.Blue, (0, 0), (500, 500), (300, 800));
                Draw.Texture(TextureBuffers.Giraffe.Texture, (500, 0));
            }
            
        }

        public void OnKeyPress(Key key)
        {
            switch(key)
            {
                case Key.Number1: _currentProgram = ShaderProgram.Default; break;
                case Key.Number2: _currentProgram = causticShader; break;
                case Key.Number3: _currentProgram = lightShader; break;
            }
        }
    }
}
