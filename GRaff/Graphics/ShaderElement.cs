using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
using coords = System.Double;
#else
using OpenTK.Graphics.ES30;
using coords = System.Single;
#endif

namespace GRaff.Graphics
{
	public class ShaderElement : GameElement
	{
		private ShaderProgram program;
		private ColoredRenderSystem _renderSystem = new ColoredRenderSystem();
		private Queue<KeyValuePair<string, int>> _intUniforms = new Queue<KeyValuePair<string, int>>();
		private Queue<KeyValuePair<string, coords>> _floatUniforms = new Queue<KeyValuePair<string, coords>>();
		private List<KeyValuePair<string, Func<int>>> _automaticIntUniforms = new List<KeyValuePair<string, Func<int>>>();
		private List<KeyValuePair<string, Func<coords>>> _automaticFloatUniforms = new List<KeyValuePair<string, Func<coords>>>();

		public ShaderElement(string source)
		{
			using (var fragmentShader = new Shader(false, source))
			{
				program = new ShaderProgram(Shader.DefaultVertexShader, fragmentShader);

			}
		}

		protected void SetPrimitive(PrimitiveType primitiveType, params GraphicsPoint[] coordinates)
		{
			_renderSystem.SetVertices(UsageHint.StreamDraw, coordinates);
		}

		protected void SetColors(params Color[] colors)
		{
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);
		}

		protected void SetUniform(string name, int value)
		{
			_intUniforms.Enqueue(new KeyValuePair<string, int>(name, value));
		}

		protected void SetUniform(string name, double value)
		{
			_floatUniforms.Enqueue(new KeyValuePair<string, coords>(name, (coords)value));
		}

		protected void AutomaticUniform(string name, Func<int> value)
		{
			_automaticIntUniforms.Add(new KeyValuePair<string, Func<int>>(name, value));
		}

		protected void AutomaticUniform(string name, Func<double> value)
		{
			_automaticFloatUniforms.Add(new KeyValuePair<string, Func<coords>>(name, () => (coords)value()));
		}

		public override void OnDraw()
		{
			using (new ShaderProgramContext(program))
			{
				while (_intUniforms.Count > 0)
				{
					var uniform = _intUniforms.Dequeue();
					GL.Uniform1(GL.GetUniformLocation(program.Id, uniform.Key), uniform.Value);
				}

				while (_floatUniforms.Count > 0)
				{
					var uniform = _floatUniforms.Dequeue();
					GL.Uniform1(GL.GetUniformLocation(program.Id, uniform.Key), uniform.Value);
				}

				foreach (var uniform in _automaticIntUniforms)
					GL.Uniform1(GL.GetUniformLocation(program.Id, uniform.Key), uniform.Value());

				foreach (var uniform in _automaticFloatUniforms)
					GL.Uniform1(GL.GetUniformLocation(program.Id, uniform.Key), uniform.Value());

				_renderSystem.Render(PrimitiveType.Quads);

			}
		}
	}
}
