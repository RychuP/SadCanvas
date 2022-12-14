global using System;
global using SadCanvas;
global using SadConsole;
global using SadRogue.Primitives;
global using MonoColor = Microsoft.Xna.Framework.Color;
global using Vector2 = Microsoft.Xna.Framework.Vector2;

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

    public static int GetRandomInt(int maxValue)
    {
        if (maxValue < 1) throw new ArgumentOutOfRangeException("maxValue has to be a minimum of 1.");
        return Game.Instance.Random.Next(maxValue + 1);
    }

    public static int GetRandomInt(int minValue, int maxValue)
    {
        return Game.Instance.Random.Next(minValue, maxValue + 1);
    }

    public static Color GetRandomColor() =>
        new((byte)GetRandomInt(256), (byte)GetRandomInt(256), (byte)GetRandomInt(256));
}