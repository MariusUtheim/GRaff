﻿using OpenTK.Graphics.OpenGL4;


namespace GRaff.Graphics.Shaders
{
    public class UniformInt32 : UniformVariable
    {
        public UniformInt32(ShaderProgram program, string name)
            : base(program, name)
        { }

        public UniformInt32(ShaderProgram program, string name, int value)
            : base(program, name)
        {
            this.Value = value;
        }


        public static void Set(ShaderProgram program, string location, int value)
            => new UniformInt32(program, location, value);

        public static int Get(ShaderProgram program, string location)
            => new UniformInt32(program, location).Value;


        public int Value
        {
            get
            {
                GL.GetUniform(Program.Id, Location, out int value);
                return value;
            }

            set
            {
                GL.ProgramUniform1(Program.Id, Location, value);
            }
        }
    }
}
