namespace GRaff.Audio
{
	internal class WaveFile
	{
		public byte[] riffID;
		public uint size;
		public byte[] wavID;
		public byte[] fmtID;
		public uint fmtSize;
		public ushort compressionCode;
		public ushort channels;
		public uint sampleRate;
		public uint bytesPerSecond;
		public ushort blockAlign;
		public ushort bitrate;
		public byte[] dataID; 
		public uint dataSize;
		public byte[] buffer;
	}
}