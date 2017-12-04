using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class UniformVariable
    {
        private bool _isVerified = false;

        internal UniformVariable(ShaderProgram program, string name)
        {
            Name = name;
			Program = program;
            Location = GL.GetUniformLocation(program.Id, name);
            if (Location < 0)
                _Graphics.ErrorCheck();
        }

        public ShaderProgram Program { get; }

        public string Name { get; }

        public int Location { get; }

        public void Verify()
        {
            if (!_isVerified && Location < 0)
            {
                _Graphics.ErrorCheck();
                throw new ArgumentException($"The uniform variable '{Name}' does not exist.");
            }
        }
    }
}
