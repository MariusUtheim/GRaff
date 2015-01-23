

namespace GRaff
{
	/// <summary>
	/// Defines an action that is performed whenever the animation of the sprite of the GRaff.Object is finished.
	/// Only subclasses of the GRaff.GameObject class can listen to this event, as GRaff.GameElement does not define a sprite.
	/// </summary>
	public interface IAnimationEndListener
	{
		/// <summary>
		/// An action that is preformed when the animation of the sprite of this instance is finished.
		/// </summary>
		void AnimationEnd();
	}
}
