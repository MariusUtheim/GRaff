using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using GRaff.Synchronization;

namespace GRaff
{
	public class SpriteAtlas
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

		private Dictionary<string, Texture> _subTextures;

		public SpriteAtlas(TextureBuffer buffer, string xmlPath)
		{
			TextureAtlas atlasData;
			using (var stream = File.OpenText(xmlPath))
				atlasData = (TextureAtlas)serializer.Deserialize(stream);

			_subTextures = new Dictionary<string, Texture>(atlasData.SubTexture.Length);
			foreach (var sx in atlasData.SubTexture)
			{
				var texture = new Texture(buffer, new Rectangle(sx.x, sx.y, sx.width, sx.height));
				_subTextures.Add(sx.name, texture);
			}
		}


		public static SpriteAtlas Load(string texturePath, string xmlPath)
			=> new SpriteAtlas(TextureBuffer.Load(texturePath), xmlPath);

		public static IAsyncOperation<SpriteAtlas> LoadAsync(string texturePath, string xmlPath)
			=> TextureBuffer.LoadAsync(texturePath).Then(buffer => new SpriteAtlas(buffer, xmlPath));

		public TextureBuffer Buffer { get; private set; }

		public Texture this[string subTextureName] => _subTextures[subTextureName];

		public AnimationStrip CreateStrip(string prefix)
			=> new AnimationStrip(_subTextures.Keys.Where(key => key.StartsWith(prefix)).OrderBy(s => s).Select(key => _subTextures[key]).ToArray());



	}
}
