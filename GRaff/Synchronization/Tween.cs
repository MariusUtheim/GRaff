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

		public static TweeningFunction Cubic { get; } = t => t * t * (2 - t);

		public static TweeningFunction Sine { get; } = t => 0.5 * (1 - GMath.Cos(t * GMath.Pi));

		public static Tween To<T>(T target, Expression<Func<T, double>> property, double finalValue, int duration, TweeningFunction f)
		{
			var expression = (MemberExpression)property.Body;
			var propertyInfo = (PropertyInfo)expression.Member;
			var initialValue = (double)propertyInfo.GetValue(target);
			Action<double> action = t => propertyInfo.SetValue(target, initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, action);
		}

		public static Tween To<T>(T target, Expression<Func<T, Point>> property, Point finalValue, int duration, TweeningFunction f)
		{
			var expression = (MemberExpression)property.Body;
			var propertyInfo = (PropertyInfo)expression.Member;
			var initialValue = (Point)propertyInfo.GetValue(target);
			Action<double> action = t => propertyInfo.SetValue(target, initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, action);
		}

		/*
				public static Tween Linear(int duration, Action<double> tweenAction, Action completed = null)
					=> Instance.Create(new Tween(duration, t => t, tweenAction, completed));

				public static Tween Cubic(int duration, Action<double> tweenAction, Action completeAction = null)
					=> Instance.Create(new Tween(duration, t => t * t * (2 - t), tweenAction, completeAction));

				public static Tween Sine(int duration, Action<double> tweenAction, Action completeAction = null)
					=> Instance.Create(new Tween(duration, t => 0.5 * (1 - GMath.Cos(t * GMath.Pi)), tweenAction, completeAction));
		*/

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
