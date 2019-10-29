using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace GRaff.Synchronization
{

    public class TweenEventArgs : EventArgs
    {
        public TweenEventArgs(double amount)
        {
            this.Amount = amount;
        }

        public double Amount { get; }
    }

	public partial class Tween : GameElement
	{
        private Dictionary<int, List<Action>> _sentinels = new Dictionary<int, List<Action>>();

		public Tween(TweenFunction tweeningFunction, int duration, Action<double> stepAction)
			: this(tweeningFunction, duration, stepAction, null)
		{ }

		public Tween(TweenFunction tweeningFunction, int duration, Action<double> stepAction, Action? completeAction)
		{
			Duration = duration;
			TweeningFunction = tweeningFunction;
			Step += (sender, e) => stepAction(e.Amount);
			Complete += (sender, e) => completeAction?.Invoke();
		}

        public static Tween Start(TweenFunction tweeningFunction, int duration, Action<double> stepAction, Action? completeAction = null)
        {
            var tween = Instance.Create(new Tween(tweeningFunction, duration, stepAction, completeAction));
            return tween;
        }

        #region Animator tweens

        private static Action<double> _setter<TValue>(Expression<Func<TValue>> property, Func<double, TValue, TValue> setter)
        {
            var expression = (MemberExpression)property.Body;

            object target;
            if (expression.Expression is ConstantExpression)
                target = ((ConstantExpression)expression.Expression).Value;
            else
            {
                var fieldExpression = (MemberExpression)expression.Expression;
                var m = fieldExpression.Member;

                var classExpression = (ConstantExpression)fieldExpression.Expression;
                dynamic val = classExpression.Value;
                target = ((dynamic)m).GetValue(val);
            }

            dynamic member = expression.Member;
            TValue initialValue = member.GetValue(target);
            return t => member.SetValue(target, setter(t, initialValue));
        }

        public static Tween Animate(TweenFunction f, int duration, Expression<Func<int>> property, int finalValue, Action? completeAction = null)
        {
            var s = _setter(property, (double t, int initialValue) => (int)(initialValue * (1 - t) + finalValue * t));
            return Start(f, duration, s, completeAction);
        }

        public static Tween Animate(TweenFunction f, int duration, Expression<Func<double>> property, double finalValue, Action? completeAction = null)
        {
            var s = _setter(property, (double t, double initialValue) => initialValue * (1 - t) + finalValue * t);
            return Start(f, duration, s, completeAction);
        }

        public static Tween Animate(TweenFunction f, int duration, Expression<Func<Point>> property, Point finalValue, Action? completeAction = null)
        {
            var s = _setter(property, (double t, Point initialValue) => initialValue * (1 - t) + finalValue * t);
            return Start(f, duration, s, completeAction);
        }

        public static Tween Animate(TweenFunction f, int duration, Expression<Func<Color>> property, Color finalValue, Action? completeAction = null)
        {
            var s = _setter(property, (double t, Color initialValue) => initialValue.Merge(finalValue, t));
            return Start(f, duration, s, completeAction);
        }

        public static Tween Animate(TweenFunction f, int duration, Expression<Func<Vector>> property, Vector finalValue, Action? completeAction = null)
        {
            var s = _setter(property, (double t, Vector initialValue) => initialValue * (1 - t) + finalValue * t);
            return Start(f, duration, s, completeAction);
        }

        public static Tween Animate(TweenFunction f, int duration, Expression<Func<Angle>> property, Angle finalValue, Action? completeAction = null)
        {
            var s = _setter(property, (double t, Angle initialValue) => initialValue + t * Angle.Acute(initialValue, finalValue));
            return Start(f, duration, s, completeAction);
        }

        #endregion

        public event EventHandler<TweenEventArgs> Step;

        public event EventHandler Complete;

		public void AtStep(int step, Action action)
		{
			if (step < 0 || step >= Duration || action == null)
				return;

			if (_sentinels.TryGetValue(step, out var actions))
				actions.Add(action);
			else
				_sentinels.Add(step, new List<Action>(new[] { action }));
		}

		public int Duration { get; private set; }

		public int Progress { get; private set; }

		public TweenFunction TweeningFunction { get; private set; }

		public sealed override void OnStep()
		{
			Progress++;
			if (Progress >= Duration)
                Step?.Invoke(this, new TweenEventArgs(TweeningFunction(1)));
			else
                Step?.Invoke(this, new TweenEventArgs(TweeningFunction((double)Progress / Duration)));

			if (_sentinels.TryGetValue(Progress, out var sentinels))
				foreach (var sentinel in sentinels)
					sentinel.Invoke();
			
			if (Progress >= Duration)
			{
				Complete?.Invoke(this, new EventArgs());
				Destroy();
			}
		}

	}
}
