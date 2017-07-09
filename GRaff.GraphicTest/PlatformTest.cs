using System;
using GRaff.Platformer;

namespace GRaff.GraphicTest
{
    [Test]
    public class PlatformTest : GameElement
    {
        public PlatformTest()
        {
            for (var i = 0; i < 20; i++)
                Instance.Create(new Block(Polygon.Rectangle(32, 32), (100 + 32 * i, 400)));

            Instance<PlatformObject>.Create(150, 340);

            Instance.Create(new Background { Color = Colors.White });
        }


    }
}
