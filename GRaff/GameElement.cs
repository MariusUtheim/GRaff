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

        public bool Exists { get; internal set; }

		public void Destroy()
		{
            if (Instance.Remove(this))
                OnDestroy();
		}

		protected virtual void OnDestroy() { }

		/// <summary>
		/// An action that is performed once every update loop.
		/// </summary>
		public virtual void OnStep() { }

		/// <summary>
		/// An action that is performed once every draw loop.
		/// </summary>
		/// <remarks>Draw loops may occur at different rates from update loops, and can be suppressed by setting IsVisible to false. Do not override this method with game logic, such as object motion.</remarks>
		public virtual void OnDraw() { }
	}
}
