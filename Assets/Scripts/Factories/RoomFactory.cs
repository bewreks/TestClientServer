using Server;

namespace Factories
{
	public class RoomFactory : FactoryBase<Room>
	{
		public override Room Get()
		{
			return InternalGet();
		}

		public override void Release(Room room)
		{
			InternalRelease(room);
		}
	}
}