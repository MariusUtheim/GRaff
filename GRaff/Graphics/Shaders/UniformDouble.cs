using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class UniformDouble : UniformVariable
    {
        public UniformDouble(ShaderProgram program, string name)
            : base(program, name)
        { }

        public UniformDouble(ShaderProgram program, string name, double value)
            : base(program, name)
        {
            this.Value = value;
        }

        public double Value
        {
            get
            {
                Verify();
                GL.GetUniform(Program.Id, Location, out double value);
                return value;
            }

            set
            {
                Verify();
                GL.ProgramUniform1(Program.Id, Location, value);
            }
        }
    }
}
