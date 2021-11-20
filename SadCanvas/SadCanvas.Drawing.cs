using SadCanvas.Shapes;

namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    readonly string OutOfRangeMsg = "Pixel position is out of range";

    /// <summary>
    /// Fills the area with <see cref="Color"/>.
    /// </summary>
    /// <param name="color">Color to fill the <see cref="Canvas"/> with.</param>
    public void Fill(Color color)
    {
        Array.Fill(Cache, color.ToMonoColor());
        IsDirty = true;
    }

    /// <summary>
    /// Changes the <see cref="Color"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <param name="color"><see cref="Color"/> of the pixel.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public void SetPixel(Point position, Color color)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        Cache[index] = color.ToMonoColor();
        IsDirty = true;
    }

    /// <summary>
    /// Retrieves the color of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="Color"/> of the pixel.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public Color GetPixel(Point position)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        MonoColor color = Cache[index];
        return new Color(color.R, color.G, color.B, color.A);
    }

    public void Draw(Shape shape)
    {

    }

    public void DrawLine()
    {

    }

    public void DrawCircle()
    {

    }

    public void DrawBox()
    {

    }
}
