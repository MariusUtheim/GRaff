using System.Threading.Tasks;


namespace GRaff
{
	public interface IAsset
	{
		void Load();
		Task LoadAsync();
		void Unload();
		AssetState AssetState { get; }
	}
}
