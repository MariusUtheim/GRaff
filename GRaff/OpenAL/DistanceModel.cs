namespace GRaff.OpenAL
{
	public enum DistanceModel
	{
		//
		// Summary:
		//     Bypasses all distance attenuation calculation for all Sources.
		None,
		//
		// Summary:
		//     InverseDistance is equivalent to the IASIG I3DL2 model with the exception that
		//     ALSourcef.ReferenceDistance does not imply any clamping.
		InverseDistance = 53249,
		//
		// Summary:
		//     InverseDistanceClamped is the IASIG I3DL2 model, with ALSourcef.ReferenceDistance
		//     indicating both the reference distance and the distance below which gain will
		//     be clamped.
		InverseDistanceClamped = 53250,
		//
		// Summary:
		//     AL_EXT_LINEAR_DISTANCE extension.
		LinearDistance = 53251,
		//
		// Summary:
		//     AL_EXT_LINEAR_DISTANCE extension.
		LinearDistanceClamped = 53252,
		//
		// Summary:
		//     AL_EXT_EXPONENT_DISTANCE extension.
		ExponentDistance = 53253,
		//
		// Summary:
		//     AL_EXT_EXPONENT_DISTANCE extension.
		ExponentDistanceClamped = 53254
	}
}
