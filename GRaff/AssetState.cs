namespace GRaff
{
	public enum AssetState
	{
		/// <summary>
		/// Indicates that the resource is not loaded.
		/// </summary>
		NotLoaded,

		/// <summary>
		/// Indicates that an operation is currently loading the resource or part of the resource asynchronously.
		/// </summary>
		LoadingAsync,

		/// <summary>
		/// Indicates that some parts of the resource are loaded, while others are not loaded.
		/// </summary>
		PartiallyLoaded,

		/// <summary>
		/// Indicates that the resource is completely loaded.
		/// </summary>
		Loaded
	}
}