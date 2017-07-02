using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics
{
    public class ShaderUniformLocation
    {
        internal ShaderUniformLocation(ShaderProgram program, string name)
        {
			Program = program;
            Location = GL.GetUniformLocation(program.Id, name);
            if (Location < 0)
            {
                _Graphics.ErrorCheck();
                //throw new ArgumentException($"The uniform variable '{name}' does not exist.");
            }
        }

        public ShaderProgram Program { get; }

        public int Location { get; }
    }
}
