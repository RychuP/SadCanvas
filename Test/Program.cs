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
            Test(new Parrot());
        }

        public static Color RandomColor => Color.White.GetRandomColor(Game.Instance.Random);

        static void Test(Canvas s, string msg = "", Point? p = null)
        {
            var sc = Game.Instance.StartingConsole;

            var (x, y) = p ?? (1, 1);
            if (msg != "") sc.Print(x, y, msg);

            sc.Children.Add(s);

            s.Position = (Settings.Rendering.RenderWidth / 2 - s.Width / 2, Settings.Rendering.RenderHeight / 2 - s.Height / 2);
        }
    }
}