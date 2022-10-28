using System;
using System.Collections.Generic;

namespace Server
{
	public class Room
	{
		public event Action OnRoomStart;
		
		public bool _started;

		private Dictionary<ulong, PlayerModel> _players = new();

		public bool Started
		{
			get => _started;
		}

		public void Add(ulong clientId)
		{
			_players.Add(clientId, new PlayerModel
			                       {
				                       id = clientId
			                       });
		}

		public void Start()
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