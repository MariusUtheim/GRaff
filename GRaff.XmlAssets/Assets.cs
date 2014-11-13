

namespace TestGame.Assets
{
	public static class Sprites
	{
		static Sprites()
		{
			Particle = new global::GRaff.Sprite(@"Star.png", 1, new global::GRaff.IntVector(12, 13), new global::GRaff.AnimationStrip(new global::System.Tuple<int, double>(0, 1), new global::System.Tuple<int, double>(4, 0.5)));
			Xujia = new global::GRaff.Sprite(@"Xujia.jpg", 1, new global::GRaff.IntVector(0, 0), new global::GRaff.AnimationStrip(1));
		}

		public static void LoadAll()
		{
			Particle.Load();
			Xujia.Load();
		}

		public static global::GRaff.Sprite Particle { get; private set; }
		public static global::GRaff.Sprite Xujia { get; private set; }
	
	}

	public static class Sounds
	{
		static Sounds()
		{
			theSound = new global::GRaff.Sound(@"theSound.ogg");
			theOtherSound = new global::GRaff.Sound(@"theOtherSound.ogg");
		}

		public static void LoadAll()
		{
			theSound.Load();
			theOtherSound.Load();
		}

		public static global::GRaff.Sound theSound { get; private set; }
		public static global::GRaff.Sound theOtherSound { get; private set; }
	}

	public static class Batches
	{
		static Batches()
		{
			MainMenu = new global::GRaff.AssetBatch();
			Everything = new global::GRaff.AssetBatch(global::TestGame.Assets.Sprites.Particle, global::TestGame.Assets.Sprites.Xujia, global::TestGame.Assets.Sounds.theSound, global::TestGame.Assets.Sounds.theOtherSound, global::TestGame.Assets.Batches.MainMenu);
		}

		public static void LoadAll()
		{
			MainMenu.Load();
			Everything.Load();
		}

		public static global::GRaff.AssetBatch MainMenu { get; private set; }
		public static global::GRaff.AssetBatch Everything { get; private set; }
	}
}

