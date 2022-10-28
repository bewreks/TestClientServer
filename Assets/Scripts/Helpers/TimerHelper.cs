using System;
using UniRx;

namespace Helpers
{
	public static class TimerHelper
	{
		public static IDisposable StartTimer(float seconds, Action callback)
		{
			return Observable.Timer(TimeSpan.FromSeconds(seconds)).Subscribe(l => { callback?.Invoke(); });
		}
	}
}