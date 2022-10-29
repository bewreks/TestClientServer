using Server;

namespace Pools
{
	public class RoomsPool : BasePool<RoomModel>
	{
		public override RoomModel Get()
		{
			return InternalGet();
		}

		public override void Release(RoomModel roomModel)
		{
			InternalRelease(roomModel);
		}
	}
}