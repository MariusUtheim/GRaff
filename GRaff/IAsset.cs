using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff
{
	/// <summary>
	/// Defines an external asset that can be loaded and unloaded.
	/// </summary>
	public interface IAsset
	{
		/// <summary>
		/// Loads the asset into memory.
		/// </summary>
		void Load();

		/// <summary>
		/// Loads the asset asynchronously.
		/// </summary>
		/// <returns>A System.Threading.Tasks.Task that will complete when the asset is completely loaded.</returns>
		IAsyncOperation LoadAsync();

		/// <summary>
		/// Unloads the asset from memory.
		/// </summary>
		void Unload();

		/// <summary>
		/// Gets whether this GRaff.IAsset is loaded in memory.
		/// </summary>
		bool IsLoaded { get; }
	}
}
