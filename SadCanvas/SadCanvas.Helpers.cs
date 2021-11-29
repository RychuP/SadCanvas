namespace SadCanvas;

// Helper methods unrelated to core functionality.
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Returns a random int including the <paramref name="maxValue"/>.
    /// </summary>
    public static int GetRandomInt(int maxValue) => Game.Instance.Random.Next(maxValue + 1);

    /// <summary>
    /// Returns a random int incuding both <paramref name="minValue"/> and <paramref name="maxValue"/>.
    /// </summary>
    public static int GetRandomInt(int minValue, int maxValue) => Game.Instance.Random.Next(minValue, maxValue + 1);

    /// <summary>
    /// Returns a random <see cref="MonoColor"/>.
    /// </summary>
    public static MonoColor GetRandomColor() =>
        new((byte)GetRandomInt(256), (byte)GetRandomInt(256), (byte)GetRandomInt(256));

    /// <summary>
    /// Checks if the pixel position is valid.
    /// </summary>
    /// <param name="position">Position to check.</param>
    /// <returns>True if the position is valid.</returns>
    public bool IsValidPosition(Point position) => Area.Contains(position);

    /// <summary>
    /// Returns a random <see cref="Point"/> on the surface.
    /// </summary>
    public Point GetRandomPosition() => Area.GetRandomPosition();
}