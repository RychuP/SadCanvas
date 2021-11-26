global using System;
global using SadCanvas;
global using SadConsole;
global using SadRogue.Primitives;
global using Console = SadConsole.Console;

using Test.Screen;

namespace Test;

static class Program
{
    public static int Width = 80;
    public static int Height = 32;

    static void Main()
    {
        Settings.WindowTitle = "SadCanvas Test";
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;

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
        _ = new Demo();
    }

    public static Color RandomColor => Color.White.GetRandomColor(Game.Instance.Random);

    static Console GetSC() => Game.Instance.StartingConsole;
}