using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace GRaff.Synchronization
{
	public delegate double TweeningFunction(double t);

	public class Tween : GameElement
	{
		private Action<double> _tweenAction;
		private Action _completeAction;

		public Tween(int duration, TweeningFunction tweeningFunction, Action<double> tweenAction)
			: this(duration, tweeningFunction, tweenAction, null)
		{ }

		public Tween(int duration, TweeningFunction tweeningFunction, Action<double> tweenAction, Action completeAction)
		{
			Contract.Requires(tweeningFunction != null);
			Duration = duration;
			TweeningFunction = tweeningFunction;
			_tweenAction = tweenAction;
			_completeAction = completeAction;
		}

		public static Tween Start(int duration, TweeningFunction tweeningFunction, Action<double> tweenAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, tweenAction));
			

		public static Tween Start(int duration, TweeningFunction tweeningFunction, Action<double> tweenAction, Action completeAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, tweenAction, completeAction));

		public static TweeningFunction Linear { get; } = t => t;

		public static TweeningFunction Cubic { get; } = t => t * t * (3 - 2 * t);

		public static TweeningFunction Sine { get; } = t => 0.5 * (1 - GMath.Cos(t * GMath.Pi));

		private static Action<double> _setter<TTarget, TValue>(TTarget target, Expression<Func<TTarget, TValue>> property, Func<double, TValue, TValue> setter)
		{
			var expression = (MemberExpression)property.Body;
			dynamic member = expression.Member;
			TValue initialValue = member.GetValue(target);
			Action<double> action = t => member.SetValue(target, setter(t, initialValue));
			return action;
		}

		public static Tween Animate<T>(T target, Expression<Func<T, double>> property, double finalValue, int duration, TweeningFunction f)
		{
			var s = _setter(target, property, (double t, double initialValue) => initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, s);
		}

		public static Tween Animate<T>(T target, Expression<Func<T, Point>> property, Point finalValue, int duration, TweeningFunction f)
		{
			var s = _setter(target, property, (double t, Point initialValue) => initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, s);
		}

		public static Tween Animate<T>(T target, Expression<Func<T, Color>> property, Color finalValue, int duration, TweeningFunction f)
		{
			var s = _setter(target, property, (double t, Color initialValue) => initialValue.Merge(finalValue, t));
			return Start(duration, f, s);
		}

		public static Tween Animate<T>(T target, Expression<Func<T, Vector>> property, Vector finalValue, int duration, TweeningFunction f)
		{
			var s = _setter(target, property, (double t, Vector initialValue) => initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, s);
		}

		public static Tween Animate<T>(T target, Expression<Func<T, Angle>> property, Angle finalValue, int duration, TweeningFunction f)
		{
			var s = _setter(target, property, (double t, Angle initialValue) => initialValue + t * Angle.Acute(initialValue, finalValue));
			return Start(duration, f, s);
		}


		public int Duration { get; private set; }

		public int Progress { get; private set; }

		public TweeningFunction TweeningFunction { get; private set; }

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
