using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	///  Defines an action that is performed whenever the animation of the sprite of the GRaff.Object is finished.
	/// </summary>
	public interface IAnimationEndListener
	{
		/// <summary>
		/// An action that is preformed when the animation of the sprite of this instance is finished.
		/// </summary>
		void AnimationEnd();
	}
}
