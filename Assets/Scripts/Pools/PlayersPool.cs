using Server;

namespace Pools
{
	public class PlayersPool : BasePool<PlayerModel>
	{
		public override PlayerModel Get()
		{
			return InternalGet();
		}

		public override void Release(PlayerModel obj)
		{
			InternalRelease(obj);
		}
	}
}