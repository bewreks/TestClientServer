using System;
using System.Collections.Generic;
using Helpers;
using Pools;
using Settings.Interfaces;
using UnityEngine;
using Zenject;
using Enumerable = System.Linq.Enumerable;

namespace Server
{
	public class Matchmaking : IDisposable
	{
		[Inject] private IRoomSettings   _roomSettings;
		[Inject] private Pools.RoomsPool _roomsPool;

		private Algorythms.Queue<ulong> _clientsQueue = new();
		private List<RoomModel>              _roomsList    = new();
		private Dictionary<ulong, RoomModel> _clientRoom   = new();

		private IDisposable _roomTimer;
		private RoomModel        _roomModelToStart;

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

			if (_clientsQueue.Count < _roomSettings.MinPlayerToStart &&
			    _roomTimer != null)
			{
				_roomTimer?.Dispose();
				_roomTimer = null;
				_roomsList.Remove(_roomModelToStart);
				_roomsPool.Release(_roomModelToStart);
				_roomModelToStart = null;
			}
		}

		private void CheckForRoomCreation()
		{
			if (_clientsQueue.Count >= _roomSettings.MinPlayerToStart &&
			    _roomTimer == null)
			{
				var roomToStart = _roomsPool.Get();
				_roomsList.Add(roomToStart);
				_roomTimer = TimerHelper.StartTimer(_roomSettings.TimeToRoomStart,
				                                    () =>
				                                    {
					                                    StartRoom(roomToStart);
					                                    CheckForRoomCreation();
				                                    });
				_roomModelToStart = roomToStart;
			}

			if (_clientsQueue.Count >= _roomSettings.MaxPlayerToStart &&
			    _roomTimer != null)
			{
				StartRoom(_roomModelToStart);
			}
		}

		private void StartRoom(RoomModel roomModelToStart)
		{
			_roomTimer?.Dispose();
			_roomTimer   = null;
			_roomModelToStart = null;
			var clientsCount = Mathf.Min(_clientsQueue.Count, _roomSettings.MaxPlayerToStart);
			var clients      = new ulong[clientsCount];

			for (var i = 0; i < clientsCount; i++)
			{
				clients[i] = _clientsQueue.Dequeue();
			}

			roomModelToStart.Start(clients);
		}

		public void Dispose()
		{
			_roomTimer?.Dispose();
		}
	}
}