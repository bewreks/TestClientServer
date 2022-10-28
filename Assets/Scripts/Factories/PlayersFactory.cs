using Server;

namespace Factories
{
	public class PlayersFactory : FactoryBase<PlayerModel>
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