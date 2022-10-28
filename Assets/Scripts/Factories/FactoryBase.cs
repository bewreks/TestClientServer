using System.Collections.Generic;

namespace Factories
{
	public abstract class FactoryBase<T>
		where T : class, new()
	{
		protected readonly Queue<T> _objects = new();

		public abstract T    Get();
		public abstract void Release(T obj);

		protected void InternalRelease(T obj)
		{
			_objects.Enqueue(obj);
		}

		protected T InternalGet()
		{
			if (_objects.Count == 0)
			{
				return new T();
			}

			return _objects.Dequeue();
		}
	}
}