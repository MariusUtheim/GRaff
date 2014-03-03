using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public interface IKeyPressListener
	{
		void OnKeyPress(Key key);
	}
}
