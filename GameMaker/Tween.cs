using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public class TweenEventArgs<TValue> : EventArgs
	{
		public TweenEventArgs(double t, TValue value)
		{
			this.T = t;
			this.Value = value;
		}

		public double T { get; private set; }
		 
		public TValue Value { get; private set; }
	}

	public abstract class Tween : GameElement
	{
		public static Tween<double> Linear(int steps, Action<double> tweenAction)
		{
			return new Tween<double>(steps, t => t, tweenAction);
		}

		public static Tween<double> Linear(double from, double to, int steps, Action<double> tweenAction)
		{
			return new Tween<double>(steps, t => (1 - t) * from + t * to, tweenAction);
		}

		public static Tween<Point> Linear(Point from, Point to, int steps, Action<Point> tweenAction)
		{
			return new Tween<Point>(steps, t => (1 - t) * from + t * to, tweenAction);
		}

		public static Tween<double> Cubic(int steps, Action<double> tweenAction)
		{
			return new Tween<double>(steps, t => GMath.Pow(t, 3), tweenAction);
		}
	}

	public class Tween<T> : Tween
	{
		private double _progress;
		private double _dt;

		public Tween(int steps, Func<double, T> tweeningFunction, Action<T> tweenAction)
		{
			if (tweeningFunction == null)
				throw new ArgumentNullException(nameof(tweeningFunction));
			this.TweeningFunction = tweeningFunction;
			this.TweenAction = tweenAction;
		}

		public Action<T> TweenAction { get; private set; }

		public Func<double, T> TweeningFunction { get; private set; }

		public override void OnStep()
		{
			_progress += _dt;
			TweenAction?.Invoke(TweeningFunction(_progress));
			if (_progress == 1)
				Destroy();
		}
	}
}
