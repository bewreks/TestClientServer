using System;
using System.Collections.Generic;

namespace Algorythms
{
	public class Queue<T>
	{
		private uint _id;
		private int  _size;

		private QueueElement<T>[]              _heap;
		private Dictionary<T, QueueElement<T>> _map = new();

		public Queue()
		{
			IncreaseSize();
		}

		public Queue(uint startId) : this()
		{
			_id = startId;
		}

		public int Count    { get; private set; }
		public int HeapSize => _heap.Length;

		private void IncreaseSize()
		{
			Array.Resize(ref _heap, (1 << ++_size) - 1);
		}

		public void Enqueue(T value)
		{
			if (Count + 1 >= 1 << _size)
			{
				IncreaseSize();
			}

			if (++_id == uint.MinValue)
			{
				FixIds();
			}

			_heap[Count] = new QueueElement<T>
			               {
				               Id       = _id,
				               Position = Count,
				               Value    = value
			               };
			_map.Add(value, _heap[Count]);
			FloatUp(Count++);
		}

		private void FixIds()
		{
			_id = 0;
			if (_heap[0] == null) return;

			var toSub = _heap[0].Id;

			foreach (var element in _map.Values)
			{
				element.Id -= toSub;

				if (_id < element.Id)
				{
					_id = element.Id;
				}
			}

			_id++;
		}

		public T Dequeue()
		{
			var queueElement = _heap[0];
			Remove(0);
			_map.Remove(queueElement.Value);
			return queueElement.Value;
		}

		public void Remove(T value)
		{
			if (_map.TryGetValue(value, out var element))
			{
				Remove(element.Position);
				_map.Remove(element.Value);
			}
		}

		private void Remove(int position)
		{
			_heap[position] = _heap[--Count];
			_heap[Count]    = null;
			FloatDown(position);
		}

		public bool Contains(T value)
		{
			return _map.ContainsKey(value);
		}

		public QueueElement<T> this[int index]
		{
			get => _heap[index];
		}

		private void FloatUp(int position)
		{
			var parent = position >> 1;

			while (position != 0)
			{
				if (_heap[parent].Id > _heap[position].Id)
				{
					Swap(position, parent);

					position = parent;
				}
				else
				{
					break;
				}
			}
		}

		private void FloatDown(int position)
		{
			var parent = position;
			if (Count == 0) return;

			while (_heap[parent] != null)
			{
				var left  = (1 << parent) + 0;
				var right = (1 << parent) + 1;

				if (_heap.Length <= left ||
				    _heap.Length <= right ||
				    _heap[left] == null && _heap[right] == null)
				{
					break;
				}

				if (_heap[left] == null && _heap[right] != null)
				{
					position = right;
				}
				else if (_heap[left] != null && _heap[right] == null)
				{
					position = left;
				}
				else
				{
					position = _heap[left]?.Id < _heap[right]?.Id ? left : right;
				}

				if (_heap[parent].Id > _heap[position].Id)
				{
					Swap(position, parent);

					parent = position;
				}
				else
				{
					break;
				}
			}
		}

		private void Swap(int position, int parent)
		{
			(_heap[parent], _heap[position]) = (_heap[position], _heap[parent]);
			_heap[parent].Position           = parent;
			_heap[position].Position         = position;
		}
	}

	public class QueueElement<T>
	{
		public uint Id;
		public int  Position;
		public T    Value;
	}
}