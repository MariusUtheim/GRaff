using System;


namespace GRaff.Synchronization
{
    public delegate double TweenFunction(double t);

	public static class TweenFunctions
	{

        public static TweenFunction Linear { get; } = t => t;

		public static TweenFunction Quadratic { get; } = t => t * t;

		public static TweenFunction Cubic { get; } = t => t * t * t;

		public static TweenFunction Quartic { get; } = t => t * t * t * t;

		public static TweenFunction Quintic { get; } = t => t * t * t * t * t;

		public static TweenFunction Power(double n) => t => GMath.Pow(t, n);

		public static TweenFunction Exponential(double rate = 10) => t => (GMath.Exp(t * rate) - 1) / (GMath.Exp(rate) - 1);

		public static TweenFunction Sine { get; } = t => 0.5 * (1 - GMath.Cos(t * GMath.Pi));

		public static TweenFunction Spring(double frequency, double dampening) => t => 1 - (1 - t) * GMath.Exp(-dampening * t) * GMath.Cos(GMath.Tau * frequency * t);

		public static TweenFunction Bounce(int bounces) =>
			bounces == -1
			? (TweenFunction)(t => (t == 1 ? 1 : 0))
			: (TweenFunction)(t => GMath.Ceiling(t * (bounces + 0.5)) / (bounces + 1) * GMath.Abs(GMath.Sin(t * (bounces + 0.5) * GMath.Pi)));

		public static TweenFunction Elastic(double frequency, double dampening) => t => t * GMath.Cos(GMath.Tau * frequency * (1 - t)) * GMath.Pow((GMath.Pow(2, t) - 1), dampening);

		public static TweenFunction Circle { get; } = t => 1 - GMath.Sqrt(1 - t * t);


        /// <summary>
        /// Gives a function that performs this GRaff.TweeningFunction for the specified fraction of the animation, followed by the next GRaff.TweeningFunction for the remainder of the animation.
        /// </summary>
        /// <param name="f">The GRaff.TweeningFunction to perform first.</param>
        /// <param name="next">The GRaff.TweeningFunction to perform second.</param>
        /// <param name="atTime">The fraction of the animation during which the first tween should be peformed. This should be in the range [0, 1].</param>
        /// <returns>A GRaff.Synchronization.TweningFunction representing the combination of the two tweening functions.</returns>
        public static TweenFunction CombineWith(this TweenFunction f, TweenFunction next, double atTime = 0.5)
        {
            Contract.Requires<ArgumentOutOfRangeException>(0 <= atTime && atTime <= 1);
            return t => t < atTime ? (atTime * f(t / atTime)) : (atTime + (1 - atTime) * next((t - atTime) / (1 - atTime)));
        }

        /// <summary>
        /// Tweens using the same tweening function, but starting at the endpoint instead of the start point.
        /// </summary>
        public static TweenFunction Backwards(this TweenFunction f) => t => 1 - f(t);

        /// <summary>
        /// Tweens using the tweening function in reverse.
        /// </summary>
        /// <param name="f">The GRaff.TweeningFunction to tween out.</param>
        /// <returns>A GRaff.TweeningFunction representing this GRaff.TweeningFunction being tweened out.</returns>
        public static TweenFunction Inverse(this TweenFunction f) => t => f(1 - t);

        /// <summary>
        /// Tweens the whole animation in reverse. This is equivalent to performing the inverse tween function backwards.
        /// </summary>
        public static TweenFunction Reverse(this TweenFunction f) => t => 1 - f(1 - t);

        /// <summary>
        /// Performs the tween from the start point to the endpoint, followed by the tween in reverse.
        /// This does not change the total time span of the function; therefore, animation will appear faster.
        /// </summary>
        public static TweenFunction ForwardThenReverse(this TweenFunction f) => f.CombineWith(f.Reverse());

        /// <summary>
        /// Performs the tween from the start point to the endpoint, followed by the same tween from the endpoint to the start point.
        /// </summary>
        public static TweenFunction ForwardThenBackwards(this TweenFunction f) => f.CombineWith(f.Backwards());

    }
}
