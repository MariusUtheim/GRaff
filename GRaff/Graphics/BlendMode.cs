using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public static BlendMode AlphaBlend { get { return new BlendMode(BlendEquation.Add, BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); } }

		public static BlendMode Additive { get { return new BlendMode(BlendEquation.Add, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public static BlendMode Max { get { return new BlendMode(BlendEquation.Max, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public BlendingFactor DestinationFactor { get; private set; }
		public BlendEquation Equation { get; private set; }
		public BlendingFactor SourceFactor { get; private set; }

		public override bool Equals(object obj)
		{
			var otherMode = obj as BlendMode;
			if (otherMode == null)
				return false;
			return DestinationFactor == otherMode.DestinationFactor && Equation == otherMode.Equation && SourceFactor == otherMode.SourceFactor;
		}

		public override int GetHashCode()
		{
			return DestinationFactor.GetHashCode() ^ GMath.BitBlockShift(SourceFactor.GetHashCode()) ^ Equation.GetHashCode();
		}
	}
}
