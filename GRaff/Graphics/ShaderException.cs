using System;
using System.Runtime.Serialization;

namespace GRaff.Graphics
{
	[Serializable]
	public class ShaderException : Exception
	{

        public ShaderException(string message)
            : base(message)
        {
        }

		public ShaderException(string message, string sourceCode) : base(message)
		{
            this.SourceCode = sourceCode;
		}

		protected ShaderException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

        public string SourceCode { get; }
	}
}