using System;
using System.Collections.Generic;
using Pools;
using Settings.Interfaces;
using Zenject;

namespace Server
{
	public class RoomModel
	{
		[Inject] private PlayersPool          _playersPool;
		[Inject] private IRoomSettings _roomSettings;

		public event Action OnRoomStart;

		private bool _started;

		private Dictionary<ulong, PlayerModel> _players = new();

		public bool Started
		{
			get => _started;
		}

		public void Start(ulong[] clients)
		{
			_started = true;
			OnRoomStart?.Invoke();
		}
	}

	public class PlayerModel
	{
		public ulong id { get; set; }
	}
}