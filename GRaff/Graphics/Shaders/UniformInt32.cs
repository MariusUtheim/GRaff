using OpenTK.Graphics.OpenGL4;


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

        public int Value
        {
            get
            {
                Verify();
                GL.GetUniform(Program.Id, Location, out int value);
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
