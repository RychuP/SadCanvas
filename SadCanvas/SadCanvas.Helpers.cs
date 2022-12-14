namespace SadCanvas;

// Helper methods unrelated to core functionality.
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Checks if the pixel position is valid.
    /// </summary>
    /// <param name="position">Position to check.</param>
    /// <returns>True if the position is valid.</returns>
    public bool IsValidPosition(Point position) => Area.Contains(position);
}