namespace SadCanvas;

/// <summary>
/// Extension methods used by the library
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Converts <see cref="MonoColor"/> to <see cref="Color"/>.
    /// </summary>
    /// <param name="color"><see cref="MonoColor"/> instance.</param>
    /// <returns>Converted <see cref="Color"/>.</returns>
    public static Color ToColor(this MonoColor color) => new(color.R, color.G, color.B, color.A);
}

