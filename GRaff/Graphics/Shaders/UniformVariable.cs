using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class UniformVariable
    {
        internal UniformVariable(ShaderProgram program, string name)
        {
            Name = name;
			Program = program;
            Location = GL.GetUniformLocation(program.Id, name);
            _Graphics.ErrorCheck();
            if (Location < 0)
                throw new ShaderException($"The uniform variable '{Name}' does not exist.");
        }

        public ShaderProgram Program { get; }

        public string Name { get; }

        public int Location { get; }

    }
}
