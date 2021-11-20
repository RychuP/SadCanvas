namespace SadCanvas;

public static class Extensions
{
    public static Color ToColor(this MonoColor color) => new(color.R, color.G, color.B, color.A);
}

