using System;
using GRaff;

namespace GRaff.Showcase
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Game.Run(1024, 768, gameStart);
        }

        static void gameStart()
        {
            GlobalEvent.ExitOnEscape = true;
            Textures.LoadAll();

            Instance.Create(new TestController());
        }

    }
}
