

namespace GRaff.OpenGL
{
	/// <summary>
	/// Represents the equation 
	/// </summary>
	public enum BlendEquation
	{
		/// <summary>
		/// SrcColor + DestColor
		/// </summary>
		FuncAdd = 32774,

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
		FuncSubtract = 32778,

		/// <summary>
		/// DestColor - SrcColor
		/// </summary>
		FuncReverseSubtract = 32779
	}
}
