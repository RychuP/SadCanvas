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
}