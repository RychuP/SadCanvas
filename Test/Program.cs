using System;
using SadCanvas;
using SadConsole;
using SadRogue.Primitives;

namespace Test
{
    static class Program
    {
        public static int Width = 80;
        public static int Height = 30;

        static void Main()
        {
            Settings.WindowTitle = "SadCanvas Test";

            // Setup the engine and create the main window.
            Game.Create(Width, Height);

            // Hook the start event so we can add consoles to the system.
            Game.Instance.OnStart = Init;

            // Start the game.
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        static void Init()
        {
            var sc = Game.Instance.StartingConsole;
            var c = new Canvas(100, 50, Color.Yellow.ToMonoColor()) { Position = (10, 10) };
            sc.Children.Add(c);
        }
    }
}