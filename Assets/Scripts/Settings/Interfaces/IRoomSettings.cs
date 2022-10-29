namespace Settings.Interfaces
{
	public interface IRoomSettings
	{
		public int MinPlayerToStart { get; }
		public int MaxPlayerToStart { get; }
		public uint TimeToRoomStart  { get; }
	}
}