using System;
using Settings.Interfaces;
using UnityEngine;

namespace Settings
{
	[Serializable]
	public class RoomSettings : IRoomSettings
	{
		[SerializeField] private int  minPlayerToStart;
		[SerializeField] private int  maxPlayerToStart;
		[SerializeField] private uint timeToRoomStart;

		public int  MinPlayerToStart => minPlayerToStart;
		public int  MaxPlayerToStart => maxPlayerToStart;
		public uint TimeToRoomStart  => timeToRoomStart;
	}
}