using System.Threading.Tasks;
using GRaff.Synchronization;


namespace GRaff
{
	public interface IAsset
	{
		void Load();
		Task LoadAsync();
		void Unload();
		AssetState ResourceState { get; }
	}
}
