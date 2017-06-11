using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
    class PlingEffect : GameObject
    {
        private const int _initialLifetime = 120, _radiusDifference = 15;
        private const double _maxRadius = 60;
        private int _expansion = 0;

        public override void OnStep()
        {
            if (_expansion++ > _initialLifetime)
                Destroy();
        }

        public override void OnDraw()
        {
            for (int count = 0, expansion = _expansion; count < 3 && expansion > 0; count++, expansion -= _radiusDifference)
            {
                Draw.FillCircle(Colors.Invisible, Colors.White.Transparent(1 - expansion / _maxRadius), Location, expansion);
            }
        }

    }
}
