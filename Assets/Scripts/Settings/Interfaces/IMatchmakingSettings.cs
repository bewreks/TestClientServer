namespace Settings.Interfaces
{
	public interface IMatchmakingSettings
	{
		public uint MinPlayerToStart { get; }
		public uint MaxPlayerToStart { get; }
		public uint TimeToRoomStart  { get; }
	}
}