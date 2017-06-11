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
    class SpecialShadersTest : GameElement
    {
        CausticShaderProgram causticShader = new CausticShaderProgram(new Rectangle(0, 0, 500, 500));

        public override void OnDraw()
        {
            Draw.Clear(Colors.Black);
            using (causticShader.Use())
            {
                causticShader.Phase = Time.LoopCount;
                Draw.FillRectangle(Colors.Aqua, (0, 0), (500, 500));
                Draw.Texture(TextureBuffers.Giraffe.Texture, Colors.Lime, (500, 0));
            }
        }
    }
}
