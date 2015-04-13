using System;
using System.Diagnostics.Contracts;

namespace GRaff.Synchronization
{

	public class Tween : GameElement
	{
		private Action<double> _tweenAction;
		private Action _completeAction;

		public Tween(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction)
			: this(duration, tweeningFunction, tweenAction, null)
		{ }

		public Tween(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction, Action completeAction)
		{
			Contract.Requires(tweeningFunction != null);
			Duration = duration;
			TweeningFunction = tweeningFunction;
			_tweenAction = tweenAction;
			_completeAction = completeAction;
		}

		public static Tween Start(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, tweenAction));
			

		public static Tween Start(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction, Action completeAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, tweenAction, completeAction));

		public static Tween Linear(int duration, Action<double> tweenAction, Action completed = null)
			=> Instance.Create(new Tween(duration, t => t, tweenAction, completed));
		

		public static Tween Cubic(int duration, Action<double> tweenAction, Action completeAction = null)
			=> Instance.Create(new Tween(duration, t => t * t * (2 - t), tweenAction, completeAction));
	

		public static Tween Sine(int duration, Action<double> tweenAction, Action completeAction = null)
			=> Instance.Create(new Tween(duration, t => 0.5 * (1 - GMath.Cos(t * GMath.Pi)), tweenAction, completeAction));
		
		public int Duration { get; private set; }

		public int Progress { get; private set; }

		public Func<double,double> TweeningFunction { get; private set; }

		public sealed override void OnStep()
		{
			Progress++;
			_tweenAction?.Invoke(TweeningFunction((double)Progress / Duration));

			if (Progress == Duration)
			{
				_completeAction?.Invoke();
				Destroy();
			}
		}
	}
}
