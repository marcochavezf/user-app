using System;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace KangouMessenger.Core
{
	public delegate void TimerCallback(object state);

	public sealed class Timer : CancellationTokenSource, IDisposable
	{
		public Timer(TimerCallback callback, object state, int dueTime, int period)
		{
			Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
				{
					var tuple = (Tuple<TimerCallback, object>) s;

					while (true)
					{
						if (IsCancellationRequested)
							break;
						Task.Run(() => tuple.Item1(tuple.Item2));
						await Task.Delay(period);
					}

				}, Tuple.Create(callback, state), CancellationToken.None,
				TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
				TaskScheduler.Default);
		}

		public new void Dispose() { base.Cancel(); }
	}

	public class CountDownTimer
	{
		private Timer _timer;

		public CountDownTimer(int minutes, int seconds, Action callback){
			var step = 1000;
			var secondsPerMinute = 60;
			var timeRemaining = minutes * secondsPerMinute * step + (seconds * step);
			_timer = new Timer (_ => {

				timeRemaining -= step;

				var hasTimerFinished = timeRemaining <= 0;
				if(hasTimerFinished){
					timeRemaining = 0;

				}

				TickTime(ConvertToReadableTime(timeRemaining), hasTimerFinished);
			}, null, step, step);
		}

		public void Dispose(){
			_timer.Dispose ();
		}

		public event Action<string, bool> TickTime = delegate {};

		private string ConvertToReadableTime(int miliseconds){
			int totalSeconds = (int)(miliseconds * 0.001);
			int minutes = (int)totalSeconds / 60;
			int seconds = (int)totalSeconds % 60;

			var output = String.Format ("{0}:{1}", minutes, seconds);
			if(seconds <= 9)
				output = String.Format ("{0}:0{1}", minutes, seconds);

			return output;
		}
	}
}

