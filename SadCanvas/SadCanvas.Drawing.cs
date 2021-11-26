using SadCanvas.Shapes;

namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    readonly string OutOfRangeMsg = "Pixel position is out of range";

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
    /// Retrieves the <see cref="MonoColor"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="MonoColor"/> of the pixel.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the position of the pixel is invalid.</exception>
    public MonoColor GetPixel(Point position)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        return Buffer[index];
    }

    /// <summary>
    /// Draws various shapes that implement <see cref="IShape"/> interface.
    /// </summary>
    /// <param name="shape"></param>
    public void Draw(IShape shape)
    {
        if (shape is Line line) DrawLine(line);
    }

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void DrawLine(Point start, Point end) => DrawLine(new Line(start, end));

    /// <summary>
    /// Draws a line.
    /// </summary>
    /// <param name="line"></param>
    public void DrawLine(Line line)
    {
        Func<int, int, bool> processor = (x, y) =>
        {
            //if (surface.IsValidCell(x, y, out int index))

            SetPixel((x, y), line.OutlineColor);

            return false;
        };

        Algorithms.Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y, processor);
    }

    /// <summary>
    /// Draws a circle.
    /// </summary>
    public void DrawCircle()
    {

    }

    /// <summary>
    /// Draws a box.
    /// </summary>
    public void DrawBox()
    {

    }
}
