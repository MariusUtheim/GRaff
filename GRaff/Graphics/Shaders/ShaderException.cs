using System;
using System.Runtime.Serialization;
using OpenTK;

namespace GRaff.Graphics.Shaders
{
	[Serializable]
	public class ShaderException : GraphicsException
	{

        public ShaderException(string message)
            : base(message)
        {
        }

		public ShaderException(string message, string sourceCode) : base(message)
		{
            this.SourceCode = sourceCode;
		}

        public string SourceCode { get; }
	}
}