

namespace GRaff.Graphics
{
	/// <summary>
	/// Represents the equation 
	/// </summary>
	public enum BlendEquation
	{
		/// <summary>
		/// SrcColor + DestColor
		/// </summary>
		Add = 32774,

		/// <summary>
		/// Min(SrcColor, DestColor);
		/// </summary>
		Min = 32775,

		/// <summary>
		/// Max(SrcColor, DestColor);
		/// </summary>
		Max = 32776,

		/// <summary>
		///  SrcColor - DestColor
		/// </summary>
		Subtract = 32778,

		/// <summary>
		/// DestColor - SrcColor
		/// </summary>
		ReverseSubtract = 32779
	}
}
