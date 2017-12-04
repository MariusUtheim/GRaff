using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class UniformVec2 : UniformVariable
    {
        public UniformVec2(ShaderProgram program, string name)
            : base(program, name)
        { }

        public UniformVec2(ShaderProgram program, string name, (double v1, double v2) value)
            : base(program, name)
        {
            this.Value = value;
        }

        public (double v1, double v2) Value
        {
            get
            {
                Verify();
                var value = new float[2];
                GL.GetUniform(Program.Id, Location, value);
                return (value[1], value[2]);
            }

            set
            {
                Verify();
                GL.ProgramUniform2(Program.Id, Location, (float)value.v1, (float)value.v2);
            }
        }
    }
}
