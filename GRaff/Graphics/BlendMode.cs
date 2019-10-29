using System;
using OpenTK.Graphics.OpenGL4;
using GLBlendingFactor = OpenTK.Graphics.OpenGL4.BlendingFactor;


namespace GRaff.Graphics
{
    public sealed class BlendMode
	{

		public BlendMode(BlendEquation equation, BlendingFactor sourceFactor, BlendingFactor destinationFactor)
		{
			this.Equation = equation;
			this.SourceFactor = sourceFactor;
			this.DestinationFactor = destinationFactor;
		}

		public IDisposable Use()
		{
            return UseContext.CreateAt(
                $"{typeof(BlendMode).FullName}.{nameof(Use)}",
                Current,
                () => Current = this,
                previous => Current = previous
            );
		}

		public static BlendMode AlphaBlend { get { return new BlendMode(BlendEquation.Add, BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); } }

		public static BlendMode Additive { get { return new BlendMode(BlendEquation.Add, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public static BlendMode Subtractive { get { return new BlendMode(BlendEquation.ReverseSubtract, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public static BlendMode Max { get { return new BlendMode(BlendEquation.Max, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }


		public static BlendMode Current
		{
			get
			{

				return new BlendMode((BlendEquation)GL.GetInteger(GetPName.BlendEquationRgb), (BlendingFactor)GL.GetInteger(GetPName.BlendSrc), (BlendingFactor)GL.GetInteger(GetPName.BlendDst));
			}
            
			set
			{
				GL.BlendFunc((GLBlendingFactor)value.SourceFactor, (GLBlendingFactor)value.DestinationFactor);
				GL.BlendEquation((BlendEquationMode)value.Equation);
			}
		}

        

		public BlendingFactor DestinationFactor { get; private set; }

		public BlendEquation Equation { get; private set; }

		public BlendingFactor SourceFactor { get; private set; }

		public override bool Equals(object? obj)
		{
            if (obj is BlendMode other)
                return DestinationFactor == other.DestinationFactor && Equation == other.Equation && SourceFactor == other.SourceFactor;
            else
                return false;
		}

		public override int GetHashCode()
		{
			return GMath.HashCombine(DestinationFactor.GetHashCode(), SourceFactor.GetHashCode(), Equation.GetHashCode());
		}
	}
}
