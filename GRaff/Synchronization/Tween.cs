using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace GRaff.Synchronization
{
	public delegate double TweeningFunction(double t);

	public class Tween : GameElement
	{
		private event EventHandler<double> _step;
		private event EventHandler _complete;
		private Dictionary<int, List<Action>> _sentinels = new Dictionary<int, List<Action>>();

		public Tween(int duration, TweeningFunction tweeningFunction, Action<double> stepAction)
			: this(duration, tweeningFunction, stepAction, null)
		{ }

		public Tween(int duration, TweeningFunction tweeningFunction, Action<double> stepAction, Action completeAction)
		{
			Contract.Requires<ArgumentNullException>(tweeningFunction != null);
			Duration = duration;
			TweeningFunction = tweeningFunction;
			_step += (sender, e) => stepAction(e);
			_complete += (sender, e) => completeAction?.Invoke();
		}

		public static Tween Start(int duration, TweeningFunction tweeningFunction, Action<double> stepAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, stepAction));

		public static Tween Start(int duration, TweeningFunction tweeningFunction, Action<double> stepAction, Action completeAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, stepAction, completeAction));

		public static TweeningFunction Linear { get; } = t => t;

		public static TweeningFunction Quadratic { get; } = t => t * t;

		public static TweeningFunction Cubic { get; } = t => t * t * t;

		public static TweeningFunction Quartic { get; } = t => t * t * t * t;

		public static TweeningFunction Quintic { get; } = t => t * t * t * t * t;

		public static TweeningFunction Power(double n) => t => GMath.Pow(t, n);

		public static TweeningFunction Exponential(double rate = 10) => t => (GMath.Exp(t * rate) - 1) / (GMath.Exp(rate) - 1);

		public static TweeningFunction Sine { get; } = t => 0.5 * (1 - GMath.Cos(t * GMath.Pi));

		public static TweeningFunction Spring(double frequency, double dampening) => t => 1 - (1 - t) * GMath.Exp(-dampening * t) * GMath.Cos(GMath.Tau * frequency * t);

		public static TweeningFunction Bounce(int bounces) =>
			bounces == -1
			? (TweeningFunction)(t => (t == 1 ? 1 : 0))
			: (TweeningFunction)(t => GMath.Ceiling(t * (bounces + 0.5)) / (bounces + 1) * GMath.Abs(GMath.Sin(t * (bounces + 0.5) * GMath.Pi)));

		public static TweeningFunction Elastic(double frequency, double dampening) => t => t * GMath.Cos(GMath.Tau * frequency * (1 - t)) * GMath.Pow((GMath.Pow(2, t) - 1), dampening);

		public static TweeningFunction Circle { get; } = t => 1 - GMath.Sqrt(1 - t * t);

		private static Action<double> _setter<TValue>(Expression<Func<TValue>> property, Func<double, TValue, TValue> setter)
		{
			Contract.Requires<ArgumentNullException>(property != null);
			Contract.Requires<ArgumentNullException>(setter != null);

			var expression = (MemberExpression)property.Body;

			object target;
			if (expression.Expression is ConstantExpression)
				target = ((ConstantExpression)expression.Expression).Value;
			else
			{
				var fieldExpression = (MemberExpression)expression.Expression;
				var m = fieldExpression.Member;

				var classExpression = (ConstantExpression)fieldExpression.Expression;
				dynamic val = classExpression?.Value;
				target = ((dynamic)m).GetValue(val);
			}

			dynamic member = expression.Member;
			TValue initialValue = member.GetValue(target);
			return t => member.SetValue(target, setter(t, initialValue));
		}

		public static Tween Animate(int duration, TweeningFunction f, Expression<Func<int>> property, int finalValue, Action completeAction = null)
		{
			var s = _setter(property, (double t, int initialValue) => (int)(initialValue * (1 - t) + finalValue * t));
			return Start(duration, f, s, completeAction);
		}
		
		public static Tween Animate(int duration, TweeningFunction f, Expression<Func<double>> property, double finalValue, Action completeAction = null)
		{
			var s = _setter(property, (double t, double initialValue) => initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, s, completeAction);
		}

		public static Tween Animate(int duration, TweeningFunction f, Expression<Func<Point>> property, Point finalValue, Action completeAction = null)
		{
			var s = _setter(property, (double t, Point initialValue) => initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, s, completeAction);
		}

		public static Tween Animate(int duration, TweeningFunction f, Expression<Func<Color>> property, Color finalValue, Action completeAction = null)
		{
			var s = _setter(property, (double t, Color initialValue) => initialValue.Merge(finalValue, t));
			return Start(duration, f, s, completeAction);
		}

		public static Tween Animate(int duration, TweeningFunction f, Expression<Func<Vector>> property, Vector finalValue, Action completeAction = null)
		{
			var s = _setter(property, (double t, Vector initialValue) => initialValue * (1 - t) + finalValue * t);
			return Start(duration, f, s, completeAction);
		}

		public static Tween Animate(int duration, TweeningFunction f, Expression<Func<Angle>> property, Angle finalValue, Action completeAction = null)
		{
			var s = _setter(property, (double t, Angle initialValue) => initialValue + t * Angle.Acute(initialValue, finalValue));
			return Start(duration, f, s, completeAction);
		}


		public event EventHandler<double> Step
		{
			add { _step += value; } 
			remove { _step -= value; }
		}

		public event EventHandler Complete
		{
			add { _complete += value; }
			remove { _complete -= value; }
		}

		public void AtStep(int step, Action action)
		{
			if (step < 0 || step >= Duration || action == null)
				return;

			List<Action> actions;
			if (_sentinels.TryGetValue(step, out actions))
				actions.Add(action);
			else
				_sentinels[step] = new List<Action>(new[] { action });
		}

		public int Duration { get; private set; }

		public int Progress { get; private set; }

		public TweeningFunction TweeningFunction { get; private set; }

		public sealed override void OnStep()
		{
			Progress++;
			_step?.Invoke(null, TweeningFunction((double)Progress / Duration));

			List<Action> sentinels;
			if (_sentinels.TryGetValue(Progress, out sentinels))
				foreach (var sentinel in sentinels)
					sentinel.Invoke();
			
			if (Progress == Duration)
			{
				_complete?.Invoke(null, null);
				Destroy();
			}
		}

	}
}
