using SadCanvas.Shapes;

namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    readonly string OutOfRangeMsg = "Pixel position is out of range";

    /// <summary>
    /// Fills the area with a <see cref="Color"/>.
    /// </summary>
    /// <param name="color">Color to fill the <see cref="Canvas"/> with.</param>
    public void Fill(Color color) => Fill(color.ToMonoColor());

    /// <summary>
    /// Fills the area with a <see cref="MonoColor"/>.
    /// </summary>
    /// <param name="color"><see cref="MonoColor"/> to fill the <see cref="Canvas"/> with.</param>
    public void Fill(MonoColor color)
    {
        Array.Fill(Buffer, color);
        IsDirty = true;
    }

    /// <summary>
    /// Clears the area with <see cref="DefaultBackground"/>.
    /// </summary>
    public void Clear()
    {
        Fill(DefaultBackground);
    }

    /// <summary>
    /// Changes the <see cref="Color"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <param name="color"><see cref="Color"/> of the pixel.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public void SetPixel(Point position, Color color) => SetPixel(position, color.ToMonoColor());

    /// <summary>
    /// Changes the <see cref="MonoColor"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <param name="color"><see cref="MonoColor"/> of the pixel.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public void SetPixel(Point position, MonoColor color)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        Buffer[index] = color;
        IsDirty = true;
    }

    /// <summary>
    /// Retrieves the color of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="Color"/> of the pixel.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public Color GetPixel(Point position) => GetMonoColor(position).ToColor();

    /// <summary>
    /// Retrieves the color of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="MonoColor"/> of the pixel.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public MonoColor GetMonoColor(Point position)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        return Buffer[index];
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
