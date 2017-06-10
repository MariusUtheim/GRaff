using GRaff.Graphics;


namespace GRaff.GraphicTest
{
	[Test]
	class TextureBufferTest : GameElement
	{
		Texture texture = TextureBuffers.Giraffe.Texture;
		Sprite sprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 2));
		TextureBuffer customTexture;

		public TextureBufferTest()
		{
			var data = new Color[100, 200];

			for (var x = 0; x < 100; x++)
				for (var y = 0; y < 200; y++)
					data[x, y] = Color.Merge(Colors.Blue.Transparent((x + y) / 300.0), Colors.Red, (x + y) / 300.0);

			customTexture = new TextureBuffer(data);
		}

		public override void OnDraw()
		{
			Draw.Clear(Colors.LightGray);
			Draw.Texture(texture, (0, 0));
			Draw.Sprite(sprite, Time.LoopCount / 20, (0.25 * texture.Width, 1.5 * texture.Height));
			Draw.Texture(customTexture.Texture, (texture.Width, 1));
		}
	}
}
