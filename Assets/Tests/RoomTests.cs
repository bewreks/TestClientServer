using Helpers;
using NUnit.Framework;
using Pools;
using Server;
using Zenject;

namespace Tests
{
	public class RoomTests
	{
		private DiContainer _diContainer;
		private RoomModel   _roomModel;

		[SetUp]
		public void Setup()
		{
			_diContainer = new DiContainer();
			_diContainer.BindPool<RoomsPool, RoomModel>();
			_diContainer.BindPool<PlayersPool, PlayerModel>();
			_roomModel = _diContainer.Resolve<RoomsPool>().Get();
		}

		[TearDown]
		public void TearDown()
		{
			_diContainer = null;
			_roomModel   = null;
		}

		[Test]
		public void AddPlayerTest()
		{
			_roomModel.Start(new[] { 0ul, 1ul });
			TestRoom(true, 2);
		}

		private void TestRoom(bool roomStarted, int playersCount)
		{
			Assert.AreEqual(roomStarted, _roomModel.Started);
		}
	}
}