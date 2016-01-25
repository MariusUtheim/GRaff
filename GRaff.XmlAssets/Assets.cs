

namespace TestGame.Assets
{
	public static class Sprites
	{
		static Sprites()
		{
		}

		public static void LoadAll()
		{
		}

		public static global::GRaff.Sprite Particle { get; private set; }
		public static global::GRaff.Sprite Xujia { get; private set; }
	
	}

	public static class Sounds
	{
		static Sounds()
		{
			theSound = new global::GRaff.Sound(@"theSound.ogg", false, 0);
			theOtherSound = new global::GRaff.Sound(@"theOtherSound.ogg", false, 1.04312);
		}

		public static void LoadAll()
		{
			global::GRaff.GRaffExtensions.Load(theSound);
			global::GRaff.GRaffExtensions.Load(theOtherSound);
		}

		public static global::GRaff.Sound theSound { get; private set; }
		public static global::GRaff.Sound theOtherSound { get; private set; }
	}

	public static class Batches
	{
		static Batches()
		{
			MainMenu = new global::GRaff.AssetBatch();
		}

		public static void LoadAll()
		{
			global::GRaff.GRaffExtensions.Load(MainMenu);
			global::GRaff.GRaffExtensions.Load(Everything);
		}

		public static global::GRaff.AssetBatch MainMenu { get; private set; }
		public static global::GRaff.AssetBatch Everything { get; private set; }
	}
}

