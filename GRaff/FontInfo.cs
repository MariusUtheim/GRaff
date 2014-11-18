namespace GRaff
{
	public class FontInfo
	{
		public string Face { get; private set; }
		public int Size { get; private set; }
		public bool IsBold { get; private set; }
		public bool IsItalic { get; private set; }
		public int LineHeight { get; private set; }
		public int Base { get; private set; }
		public int ScaleW { get; private set; }
		public int ScaleH { get; private set; }
	}
}