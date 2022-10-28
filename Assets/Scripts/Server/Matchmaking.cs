using System;
using System.Collections.Generic;
using Factories;
using Helpers;
using Settings.Interfaces;
using UnityEngine;
using Zenject;
using Enumerable = System.Linq.Enumerable;

namespace Server
{
	public class Matchmaking : IDisposable
	{
		[Inject] private IMatchmakingSettings _matchmakingSettings;
		[Inject] private RoomFactory          _roomFactory;

		private Algorythms.Queue<ulong> _clientsQueue = new();
		private List<Room>              _roomsList    = new();
		private Dictionary<ulong, Room> _clientRoom   = new();

		private IDisposable _roomTimer;
		private Room        _roomToStart;

		public int InQueueCount         => _clientsQueue.Count;
		public int TotalRoomsCount      => _roomsList.Count;
		public int NotStartedRoomsCount => Enumerable.Count(_roomsList, _ => !_.Started);
		public int StartedRoomsCount    => Enumerable.Count(_roomsList, _ => _.Started);

		public void EnterToLobby(ulong clientId)
		{
			if (!_clientsQueue.Contains(clientId))
			{
				_clientsQueue.Enqueue(clientId);
			}

			CheckForRoomCreation();
		}

		public void ExitFromLobby(ulong clientId)
		{
			if (_clientsQueue.Contains(clientId))
			{
				_clientsQueue.Remove(clientId);
			}

			if (_clientsQueue.Count < _matchmakingSettings.MinPlayerToStart &&
			    _roomTimer != null)
			{
				_roomTimer?.Dispose();
				_roomTimer = null;
				_roomsList.Remove(_roomToStart);
				_roomFactory.Release(_roomToStart);
				_roomToStart = null;
			}
		}

		private void CheckForRoomCreation()
		{
			if (_clientsQueue.Count >= _matchmakingSettings.MinPlayerToStart &&
			    _roomTimer == null)
			{
				var roomToStart = _roomFactory.Get();
				_roomsList.Add(roomToStart);
				_roomTimer = TimerHelper.StartTimer(_matchmakingSettings.TimeToRoomStart,
				                                    () =>
				                                    {
					                                    StartRoom(roomToStart);
					                                    CheckForRoomCreation();
				                                    });
				_roomToStart = roomToStart;
			}

			if (_clientsQueue.Count >= _matchmakingSettings.MaxPlayerToStart &&
			    _roomTimer != null)
			{
				StartRoom(_roomToStart);
			}
		}

		private void StartRoom(Room roomToStart)
		{
			_roomTimer?.Dispose();
			_roomTimer   = null;
			_roomToStart = null;
			var clientsCount = Mathf.Min(_clientsQueue.Count, _matchmakingSettings.MaxPlayerToStart);

			for (var i = 0; i < clientsCount; i++)
			{
				roomToStart.Add(_clientsQueue.Dequeue());
			}

			roomToStart.Start();
		}

		public void Dispose()
		{
			_roomTimer?.Dispose();
		}
	}
}