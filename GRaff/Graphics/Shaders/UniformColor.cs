using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class UniformColor : UniformVariable
    {
        public UniformColor(ShaderProgram program, string name)
            : base(program, name)
        { }

        public UniformColor(ShaderProgram program, string name, Color value)
            : base(program, name)
        {
            this.Value = value;
        }

        public static void Set(ShaderProgram program, string location, Color value)
            => new UniformColor(program, location, value);

        public static Color Get(ShaderProgram program, string location)
            => new UniformColor(program, location).Value;


        public Color Value
        {
            get
            {
                Verify();
                var values = new int[4];
                GL.GetUniform(Program.Id, Location, values);
                return Color.FromRgba(values[0], values[1], values[2], values[3]);
            }

            set
            {
                Verify();
                GL.ProgramUniform4(Program.Id, Location, value.R / 255.0f, value.G / 255.0f, value.B / 255.0f, value.A / 255.0f);
            }
        }
    }
}
