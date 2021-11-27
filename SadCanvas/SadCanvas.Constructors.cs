namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Canvas(int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(Errors.CanvasDimensionsZeroOrNegative);
        _texture = CreateTexture(width, height);
        SetDimensions();
    }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> and fills it with a <see cref="MonoColor"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color"><see cref="MonoColor"/> that will become a <see cref="DefaultBackground"/> and fill the area of <see cref="Canvas"/>.</param>
    public Canvas(int width, int height, MonoColor color) : this(width, height)
    {
        DefaultBackground = color;
        Array.Fill(Buffer, color);
        Refresh();
    }

    /// <summary>
    /// Constructor that creates a <see cref="Canvas"/> from an image file.
    /// </summary>
    /// <param name="fileName">File containing an image.</param>
    public Canvas(string fileName)
    {
        _texture = LoadTexture(fileName);
        SetDimensions();
    }
}