using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using GRaff.Synchronization;

namespace GRaff
{
	public class SpriteAtlas : IAsset
	{
		[XmlType]
		public class SubTexture
		{
			[XmlAttribute]
			public string name;
			[XmlAttribute]
			public float x;
			[XmlAttribute]
			public float y;
			[XmlAttribute]
			public float width;
			[XmlAttribute]
			public float height;
		}

		[XmlRoot]
		public class TextureAtlas
		{
			[XmlElement]
			public SubTexture[] SubTexture;
			
			[XmlAttribute]
			public string imagePath;
		}

		private static readonly XmlSerializer serializer = new XmlSerializer(typeof(TextureAtlas));

		private TextureBuffer _buffer;
		private Dictionary<string, Texture> _subTextures;
		private IAsyncOperation _loadingOperation;

		public SpriteAtlas(string xmlPath, string imagePath)
		{
			this.XmlPath = xmlPath;
			this.ImagePath = imagePath;
		}

		public string XmlPath { get; private set; }

		public string ImagePath { get; private set; }

		public Texture this[string subTextureName]
		{
			get
			{
				return _subTextures[subTextureName];
			}
		}

		public bool IsLoaded
		{
			get; private set;
		}

		public IAsyncOperation LoadAsync()
		{
			if (_loadingOperation != null)
				return _loadingOperation;

			TextureAtlas textureAtlas;
			using (var streamReader = File.OpenText(XmlPath))
				textureAtlas = (TextureAtlas)serializer.Deserialize(streamReader);

			if (ImagePath == null && textureAtlas.imagePath == null)
				return _loadingOperation = Async.Fail(new InvalidOperationException("Image path was not specified by the sprite atlas or the .xml file"));

#warning Image path from the xml file should be specified relative to the xml
			var imagePath = ImagePath ?? textureAtlas.imagePath;

			return TextureBuffer.LoadAsync(imagePath).Then(buffer => {
				IsLoaded = true;
				_subTextures = new Dictionary<string, Texture>(textureAtlas.SubTexture.Length);
				foreach (var sx in textureAtlas.SubTexture)
				{
					var tex = new Texture(buffer, sx.x / buffer.Width, (sx.y) / buffer.Height, (sx.x + sx.width) / buffer.Width, (sx.y + sx.height) / buffer.Height);
					_subTextures.Add(sx.name, tex);
				}
			});
		}

		public void Unload()
		{
			throw new NotImplementedException();
		}
	}
}
