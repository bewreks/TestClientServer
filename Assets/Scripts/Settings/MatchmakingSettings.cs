using System;
using Settings.Interfaces;
using UnityEngine;

namespace Settings
{
	[Serializable]
	public class MatchmakingSettings : IMatchmakingSettings
	{
		[SerializeField] private uint minPlayerToStart;
		[SerializeField] private uint maxPlayerToStart;
		[SerializeField] private uint timeToRoomStart;

		public uint MinPlayerToStart => minPlayerToStart;
		public uint MaxPlayerToStart => maxPlayerToStart;
		public uint TimeToRoomStart  => timeToRoomStart;
	}
}