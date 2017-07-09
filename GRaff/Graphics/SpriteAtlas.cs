using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GRaff.Graphics;
using GRaff.Synchronization;

namespace GRaff.Graphics
{
	public class SpriteAtlas
	{
        [XmlType("SubTexture")]
		public class SubTextureData
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
			public SubTextureData[] SubTexture;
			
			[XmlAttribute]
			public string imagePath;
		}

		private static readonly XmlSerializer serializer = new XmlSerializer(typeof(TextureAtlas));

        private Dictionary<string, IntRectangle> _subTextures;

		public SpriteAtlas(Texture buffer, string xml)
			: this(buffer, new MemoryStream(Encoding.UTF8.GetBytes(xml)))
		{ }

        public SpriteAtlas(Texture texture, Stream xmlStream)
		{
			var atlasData = (TextureAtlas)serializer.Deserialize(xmlStream);

            this.Texture = texture;
            _subTextures = new Dictionary<string, IntRectangle>(atlasData.SubTexture.Length);
			foreach (var sx in atlasData.SubTexture)
			{
                var region = new IntRectangle((int)sx.x, (int)sx.y, (int)sx.width, (int)sx.height);
                _subTextures.Add(sx.name, region);
			}
		}

        public SpriteAtlas(params (Texture texture, string name)[] textureMappings)
        {
            IntVector bounds;
            var rects = RectanglePacker.Pack(textureMappings.Select(tm => tm.texture.Size), out bounds);

            _subTextures = new Dictionary<string, IntRectangle>(textureMappings.Length);

            var framebuffer = new Framebuffer(bounds);
            using (framebuffer.Use())
            {
                for (var i = 0; i < textureMappings.Length; i++)
                {
                    Draw.Texture(textureMappings[i].texture.SubTexture(), rects[i].TopLeft);
                    _subTextures.Add(textureMappings[i].name, rects[i]);
                }
            }

            this.Texture = framebuffer.Texture;
        }

		public static SpriteAtlas Load(string texturePath, string xmlPath = null)
		{
			if (xmlPath == null)
			{
				var index = texturePath.LastIndexOf('.');
				if (index == -1)
					xmlPath = texturePath + ".xml";
				else
					xmlPath = texturePath.Substring(0, index) + ".xml";
			}
			var texture = Texture.Load(texturePath);
			var xml = File.ReadAllText(xmlPath);
			return new SpriteAtlas(texture, xml);
		}

		public static IAsyncOperation<SpriteAtlas> LoadAsync(string texturePath, string xmlPath = null)
		{
			if (xmlPath == null)
			{
				var index = texturePath.LastIndexOf('.');
				if (index == -1)
					xmlPath = texturePath + ".xml";
				else
					xmlPath = texturePath.Substring(0, index) + ".xml";
			}
			return
						Texture.LoadAsync(texturePath)
				.ThenAsync(async buffer =>
				{
					var xml = await Task.Run(() => File.ReadAllText(xmlPath));
					return new SpriteAtlas(buffer, new MemoryStream(Encoding.UTF8.GetBytes(xml)));
				});
		}

		public Texture Texture { get; private set; }

        public SubTexture SubTexture(string subtextureName) => Texture.SubTexture(_subTextures[subtextureName]);

        public SubTexture this[string subtextureName] => Texture.SubTexture(_subTextures[subtextureName]);

		public AnimationStrip AnimationStrip(string prefix)
		{
			var textures = _subTextures.Keys.Where(key => key.StartsWith(prefix)).OrderBy(s => s).Select(key => _subTextures[key]).ToArray();
			if (textures.Length == 0)
				throw new InvalidOperationException($"Did not find any subtextures with the prefix '{prefix}'.");
            throw new NotImplementedException();
            //return new AnimationStrip(textures);
		}



	}
}
