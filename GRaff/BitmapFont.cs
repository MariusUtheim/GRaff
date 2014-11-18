using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public class BitmapFont : IAsset
	{
		public BitmapFont(string filename)
		{
			this.FileName = filename;
		}

		public string FileName { get; private set; }

		public Dictionary<char, FontCharacter> Map { get; private set; }

		public FontInfo Info
		{
			get;
			private set;
		}

		public AssetState ResourceState
		{
			get;
			private set;
		}

		public void Load()
		{
			throw new NotImplementedException();
		}

		public Task LoadAsync()
		{
			throw new NotImplementedException();
		}

		public void Unload()
		{
			throw new NotImplementedException();
		}
	}
}
