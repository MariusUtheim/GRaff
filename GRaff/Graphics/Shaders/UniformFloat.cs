using OpenTK.Graphics.OpenGL4;


namespace GRaff.Graphics.Shaders
{
    public class UniformFloat : UniformVariable
    {
        public UniformFloat(ShaderProgram program, string name)
            : base(program, name)
        { }

        public UniformFloat(ShaderProgram program, string name, double value)
            : base(program, name)
        {
            this.Value = value;
        }

        public static void Set(ShaderProgram program, string location, double value)
            => new UniformDouble(program, location, value);

        public static double Get(ShaderProgram program, string location)
            => new UniformDouble(program, location).Value;

        public double Value
        {
            get
            {
                Verify();
                GL.GetUniform(Program.Id, Location, out float value);
                return value;
            }

            set
            {
                Verify();
                GL.ProgramUniform1(Program.Id, Location, (float)value);
            }
        }
    }
}
