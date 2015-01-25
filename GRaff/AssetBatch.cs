﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff
{
	#if !PUBLISH
	#warning Missing documentation
	public sealed class AssetBatch : IAsset
	{
		private List<IAsset> _resources;

		public AssetBatch()
		{
			_resources = new List<IAsset>();
		}

		public AssetBatch(params IAsset[] resources)
		{
			_resources = new List<IAsset>(resources);
		}

		public void Load()
		{
			foreach (var resource in _resources)
				resource.Load();
		}

		public IAsyncOperation LoadAsync()
		{
			throw new NotImplementedException();
		}

		public void Unload()
		{
			foreach (var resource in _resources)
				resource.Unload();
		}

		public AssetState AssetState
		{
			get
			{
				bool loaded = false, loadingAsync = false, notLoaded = false, partiallyLoaded = false;
				foreach (var resource in _resources)
					switch (resource.AssetState)
					{
						case AssetState.Loaded: loaded = true; break;
						case AssetState.LoadingAsync: loadingAsync = true; break;
						case AssetState.NotLoaded: notLoaded = true; break;
						case AssetState.PartiallyLoaded: partiallyLoaded = true; break;
						default: throw new NotSupportedException(String.Format("GRaff.ResourceState.{0} is not supported in GRaff.ResourceBatch.ResourceState.get", Enum.GetName(typeof(AssetState), resource.AssetState)));
                    }

				if (!loadingAsync && !notLoaded && !partiallyLoaded)
					return AssetState.Loaded; // Note: This is the default state for no resources

				if (loadingAsync)
					return AssetState.LoadingAsync;

				if (partiallyLoaded || (loaded && notLoaded))
					return AssetState.PartiallyLoaded;

				if (notLoaded)
					return AssetState.NotLoaded;

				throw new NotSupportedException(String.Format("A case was not considered: {0}{1}{2}{3}", loaded ? 1 : 0, loadingAsync ? 1 : 0, notLoaded ? 1 : 0, partiallyLoaded ? 1 : 0));
			}
		}
	}
	#endif
}
