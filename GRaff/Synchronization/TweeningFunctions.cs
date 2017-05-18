using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public delegate double TweeningFunction(double t);

	public partial class Tween
	{

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
	}
}
