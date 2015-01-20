namespace GRaff
{
	/// <summary>
	/// Represents the loading state of an IAsset.
	/// </summary>
	public enum AssetState
	{
		/// <summary>
		/// Indicates that the resource is not loaded.
		/// </summary>
		NotLoaded = 0,

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