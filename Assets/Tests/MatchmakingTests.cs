using System.Collections;
using NUnit.Framework;
using Server;
using Settings.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests
{
	public class MatchmakingTests
	{
		private Matchmaking          _matchmaking;
		private IMatchmakingSettings _matchmakingSettings;
		private DiContainer          _container;

		[SetUp]
		public void Setup()
		{
			_container   = new DiContainer();
			_matchmaking = new Matchmaking();
			_matchmakingSettings = new MatchmakingSettingsTest
			                       {
				                       MinPlayerToStart = 3,
				                       MaxPlayerToStart = 4,
				                       TimeToRoomStart  = 1
			                       };
			_container.BindInterfacesTo<MatchmakingSettingsTest>().FromInstance(_matchmakingSettings).AsSingle();
			_container.Inject(_matchmaking);
		}

		[TearDown]
		public void TearDown()
		{
			_container           = null;
			_matchmaking         = null;
			_matchmakingSettings = null;
		}

		[Test]
		public void AddClientTest()
		{
			_matchmaking.EnterToLobby(0);
			TestMatchmaking(1);
		}

		[Test]
		public void AddSameClientTest()
		{
			_matchmaking.EnterToLobby(0);
			_matchmaking.EnterToLobby(0);
			TestMatchmaking(1);
		}

		[Test]
		public void AddTwoClientTest()
		{
			_matchmaking.EnterToLobby(0);
			_matchmaking.EnterToLobby(1);
			TestMatchmaking(2);
		}

		[UnityTest]
		public IEnumerator AddMinimumPlayersTest()
		{
			for (uint i = 0; i < _matchmakingSettings.MinPlayerToStart; i++)
			{
				TestMatchmaking(i);
				_matchmaking.EnterToLobby(i);
			}

			TestMatchmaking(_matchmakingSettings.MinPlayerToStart, 1, 1);
			yield return new WaitForSeconds(_matchmakingSettings.TimeToRoomStart);
			TestMatchmaking(0, 1, 0, 1);
		}

		[Test]
		public void AddMaximumPlayersTest()
		{
			for (uint i = 0; i < _matchmakingSettings.MinPlayerToStart; i++)
			{
				TestMatchmaking(i);
				_matchmaking.EnterToLobby(i);
			}

			TestMatchmaking(_matchmakingSettings.MinPlayerToStart, 1, 1);

			for (uint i = 0; i < _matchmakingSettings.MaxPlayerToStart - _matchmakingSettings.MinPlayerToStart; i++)
			{
				var id = _matchmakingSettings.MinPlayerToStart + i;
				TestMatchmaking(id, 1, 1);
				_matchmaking.EnterToLobby(id);
			}

			TestMatchmaking(0, 1, 0, 1);
		}

		[UnityTest]
		public IEnumerator AddPlayersForToRoomsTest()
		{
			for (uint i = 0; i < _matchmakingSettings.MinPlayerToStart; i++)
			{
				TestMatchmaking(i);
				_matchmaking.EnterToLobby(i);
			}

			TestMatchmaking(_matchmakingSettings.MinPlayerToStart, 1, 1);

			for (uint i = 0; i < _matchmakingSettings.MaxPlayerToStart - _matchmakingSettings.MinPlayerToStart; i++)
			{
				var id = _matchmakingSettings.MinPlayerToStart + i;
				TestMatchmaking(id, 1, 1);
				_matchmaking.EnterToLobby(id);
			}

			TestMatchmaking(0, 1, 0, 1);

			for (uint i = 0; i < _matchmakingSettings.MinPlayerToStart; i++)
			{
				TestMatchmaking(i, 1, 0, 1);
				_matchmaking.EnterToLobby(_matchmakingSettings.MaxPlayerToStart + i);
			}

			TestMatchmaking(3, 2, 1, 1);
			yield return new WaitForSeconds(_matchmakingSettings.TimeToRoomStart);
			TestMatchmaking(0, 2, 0, 2);
		}

		[Test]
		public void RemoveFromEmptyQueue()
		{
			_matchmaking.ExitFromLobby(0);

			TestMatchmaking(0);
		}

		[Test]
		public void RemoveFromNonEmptyQueue()
		{
			_matchmaking.EnterToLobby(0);
			_matchmaking.ExitFromLobby(0);

			TestMatchmaking(0);
		}

		[UnityTest]
		public IEnumerator RemoveFromStartedQueue()
		{
			_matchmaking.EnterToLobby(0);
			_matchmaking.EnterToLobby(1);
			_matchmaking.EnterToLobby(2);
			TestMatchmaking(3, 1, 1);
			_matchmaking.ExitFromLobby(0);
			yield return new WaitForSeconds(_matchmakingSettings.TimeToRoomStart);

			TestMatchmaking(2);
		}


		private void TestMatchmaking(uint inQueue, uint totalRooms = 0, uint notStartedRooms = 0, uint startedRooms = 0)
		{
			Assert.AreEqual(inQueue,         _matchmaking.InQueueCount,         "Players in queue");
			Assert.AreEqual(totalRooms,      _matchmaking.TotalRoomsCount,      "Total rooms");
			Assert.AreEqual(notStartedRooms, _matchmaking.NotStartedRoomsCount, "Not started rooms");
			Assert.AreEqual(startedRooms,    _matchmaking.StartedRoomsCount,    "Started rooms");
		}
	}

	internal class MatchmakingSettingsTest : IMatchmakingSettings
	{
		public uint MinPlayerToStart { get; set; }
		public uint MaxPlayerToStart { get; set; }
		public uint TimeToRoomStart  { get; set; }
	}
}