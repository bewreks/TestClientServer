using Pools;
using Zenject;

namespace Helpers
{
	public static class PoolsHelper
	{
		public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindPool<T, T1>(this DiContainer container)
			where T : BasePool<T1>, new()
			where T1 : class, new()
		{
			var pool = new T();
			container.Inject(pool);
			return container.Bind<T>().FromInstance(pool);
		}
	}
}