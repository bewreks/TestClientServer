using System;
using Algorythms;
using NUnit.Framework;

namespace Tests
{
	public class QueueTests
	{
		private Queue<int> _queue;

		[SetUp]
		public void Setup()
		{
			_queue = new Queue<int>();
		}

		[TearDown]
		public void TearDown()
		{
			_queue = null;
		}

		[Test]
		public void EnqueueElementTest()
		{
			_queue.Enqueue(0);
			Assert.AreEqual(1, _queue.Count);
		}

		[Test]
		public void EnqueueDequeueElementTest()
		{
			_queue.Enqueue(0);
			var dequeue = _queue.Dequeue();
			Assert.AreEqual(0, _queue.Count);
			Assert.AreEqual(0, dequeue);
		}

		[Test]
		public void TwoEnqueueDequeueElementTest()
		{
			_queue.Enqueue(0);
			_queue.Enqueue(1);
			Assert.AreEqual(2, _queue.Count);
			var dequeue = _queue.Dequeue();
			Assert.AreEqual(1, _queue.Count);
			Assert.AreEqual(0, dequeue);
		}

		[Test]
		public void FourEnqueueElementTest()
		{
			Assert.AreEqual(1, _queue.HeapSize);
			_queue.Enqueue(0);
			Assert.AreEqual(1, _queue.HeapSize);
			_queue.Enqueue(1);
			Assert.AreEqual(3, _queue.HeapSize);
			_queue.Enqueue(2);
			Assert.AreEqual(3, _queue.HeapSize);
			_queue.Enqueue(3);
			Assert.AreEqual(7, _queue.HeapSize);
			Assert.AreEqual(4, _queue.Count);
		}

		[Test]
		public void FourEnqueueFourDequeueElementTest()
		{
			var count = 4;

			for (var i = 0; i < count; i++)
			{
				_queue.Enqueue(i);
				Assert.AreEqual(i + 1, _queue.Count);
			}

			for (var i = 0; i < count; i++)
			{
				Assert.AreEqual(i,             _queue.Dequeue());
				Assert.AreEqual(count - i - 1, _queue.Count);
			}
		}

		[Test]
		public void ContainsElementTest()
		{
			_queue.Enqueue(0);
			_queue.Enqueue(1);
			_queue.Enqueue(2);
			_queue.Enqueue(3);
			_queue.Dequeue();

			Assert.IsFalse(_queue.Contains(0));
			Assert.IsTrue(_queue.Contains(1));
			Assert.IsTrue(_queue.Contains(2));
			Assert.IsTrue(_queue.Contains(3));
		}

		[Test]
		public void RemoveElementTest()
		{
			_queue.Enqueue(0);
			_queue.Enqueue(1);
			_queue.Enqueue(2);
			_queue.Enqueue(3);
			_queue.Remove(2);

			Assert.IsTrue(_queue.Contains(0));
			Assert.IsTrue(_queue.Contains(1));
			Assert.IsFalse(_queue.Contains(2));
			Assert.IsTrue(_queue.Contains(3));
		}

		[Test]
		public void TestIdOverflow1()
		{
			_queue = new Queue<int>(uint.MaxValue - 1);
			_queue.Enqueue(1);
			Assert.AreEqual(uint.MaxValue, _queue[0].Id);
			_queue.Enqueue(2);
			Assert.AreEqual(0, _queue[0].Id);
			Assert.AreEqual(1, _queue[1].Id);
			_queue.Enqueue(3);
			Assert.AreEqual(0, _queue[0].Id);
			Assert.AreEqual(1, _queue[1].Id);
			Assert.AreEqual(2, _queue[2].Id);
			Assert.AreEqual(3, _queue.Count);
			Assert.AreEqual(1, _queue.Dequeue());
		}

		[Test]
		public void TestIdOverflow2()
		{
			_queue = new Queue<int>(uint.MaxValue - 1);
			_queue.Enqueue(1);
			Assert.AreEqual(uint.MaxValue, _queue[0].Id);
			_queue.Dequeue();
			_queue.Enqueue(1);
			Assert.AreEqual(0, _queue[0].Id);
			_queue.Dequeue();
			_queue.Enqueue(1);
			Assert.AreEqual(1, _queue[0].Id);
			Assert.AreEqual(1, _queue.Count);
			Assert.AreEqual(1, _queue.Dequeue());
		}
	}
}