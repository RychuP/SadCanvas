namespace SadCanvas;

/// <summary>
/// Extension methods used by the library
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Converts <see cref="Microsoft.Xna.Framework.Rectangle"/> to <see cref="Rectangle"/>.
    /// </summary>
    public static Rectangle ToSadRectangle(this Microsoft.Xna.Framework.Rectangle rect) =>
        new(rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Returns a random <see cref="Point"/> with the area.
    /// </summary>
    /// <param name="area">Area of the <see cref="Rectangle"/>.</param>
    public static Point GetRandomPosition(this Rectangle area) =>
        (Canvas.GetRandomInt(area.X, area.X + area.Width), Canvas.GetRandomInt(area.Y, area.Y + area.Height));
}