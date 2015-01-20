using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Represents the most general game element that is automatically handled by the engine.
	/// </summary>
	public abstract class GameElement
	{
		private int _depth;
		/// <summary>
		/// Gets or sets the depth of this GRaff.GameObject.
		/// Instances with higher depth take actions before and are drawn behind instances with lower depth.
		/// Changes to depth value are not reflected in the game before a new frame is drawn.
		/// </summary>
		public int Depth
		{
			get { return _depth; }
			set { _depth = value; Instance.NeedsSort = true; }
		}

		public bool IsVisible { get; set; } = true;

		/// <summary>
		/// Destroys the instance of this GRaff.GameObject, removing it from the game.
		/// The instance will stop performing automatic actions such as Step and Draw,
		/// but the C# object is not garbage collected while it is still being referenced.
		/// </summary>
		/// <param name="suppressDestroyEvent">If set to true, the OnDestroy() action will not be performed.</param>
		public void Destroy()
		{
			OnDestroy();
			Instance.Remove(this);
		}

		/// <summary>
		/// An action that is performed just before the instance is destroyed.
		/// </summary>
		public virtual void OnDestroy() { }


		public virtual void OnStep()
		{
		}

		public virtual void OnDraw()
		{
		}
	}
}
