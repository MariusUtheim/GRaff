using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public sealed class TweenEventArgs : EventArgs
	{
		public TweenEventArgs(int loopCount, double t)
		{
			this.LoopCount = loopCount;
			this.T = t;
		}

		public int LoopCount { get; private set; }

		public double T { get; private set; }
	}

	public sealed class Tween : GameElement
	{
		private int _progress;
		public event EventHandler<TweenEventArgs> TweenAction;
		public event EventHandler<TweenEventArgs> Complete;

		public Tween(int duration, Func<double, double> tweeningFunction, EventHandler<TweenEventArgs> tweenAction)
		{
			if (tweeningFunction == null)
				throw new ArgumentNullException("tweeningFunction");
			this.Duration = duration;
			this.TweeningFunction = tweeningFunction;
			this.TweenAction += tweenAction;
		}

		public static Tween Linear(int duration, Action<double> tweenAction)
		{
			return new Tween(duration, t => t, (sender, e) => tweenAction.Invoke(e.T)); /*C#6.0*/
		}

		public static Tween Cubic(int duration, Action<double> tweenAction)
		{
			return new Tween(duration, t => (GMath.Sqr(t) + GMath.Sqr(1 - t)) * t * (1 - t), (sender, e) => tweenAction.Invoke(e.T));
		}

		public static Tween Sine(int duration, Action<double> tweenAction)
		{
			return new Tween(duration, t => 0.5 * (1 - GMath.Cos(t * GMath.Pi)), (sender, e) => tweenAction.Invoke(e.T));
		}

		public Tween Inverse()
		{
			return new Tween(Duration, t => 1 - this.TweeningFunction(t), TweenAction);
		}

		public Tween TwoSidedStep()
		{
			return new Tween(Duration, t => (t < 0.5) ? TweeningFunction(2 * t) / 2.0 : 1 - TweeningFunction(2 - 2 * t) / 2.0, TweenAction);
		}

		public Tween TwoSidedInterpolate()
		{
			return new Tween(Duration, t => TweeningFunction(t) * (1 - t) + TweeningFunction(1 - t) * t, TweenAction);
        }

		public int Duration { get; private set; }

		public Func<double,double> TweeningFunction { get; private set; }

		public override void OnStep()
		{
			_progress++;
			if (TweenAction != null) /*C#6.0*/
				TweenAction.Invoke(this, new TweenEventArgs(_progress, TweeningFunction((double)_progress / Duration)));
			if (_progress == Duration)
			{
				if (Complete != null)
					Complete.Invoke(this, new TweenEventArgs(_progress, 1.0));
				Destroy();
			}
		}
	}
}
