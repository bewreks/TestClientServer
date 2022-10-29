using System.Collections.Generic;
using Zenject;

namespace Pools
{
	public abstract class BasePool<T>
		where T : class, new()
	{
		[Inject] protected DiContainer _container;
		
		protected readonly Queue<T> _objects = new();

		public abstract T    Get();
		public abstract void Release(T obj);

		protected void InternalRelease(T obj)
		{
			_objects.Enqueue(obj);
		}

		protected T InternalGet()
		{
			if (!_objects.TryDequeue(out var obj))
			{
				obj = new T();
			}
			
			_container.Inject(obj);

			return obj;
		}
	}
}