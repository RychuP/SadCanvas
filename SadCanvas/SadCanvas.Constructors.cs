namespace SadCanvas;

public partial class Canvas : PixelSurface
{
    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    public Canvas(int width, int height) : base(width, height)
    {
        Buffer = new MonoColor[Size];
    }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> and fills it with a <see cref="Color"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color"><see cref="Color"/> used to fill the area of the <see cref="Canvas"/>.</param>
    public Canvas(int width, int height, Color color) : this(width, height)
    {
        Array.Fill(Buffer, color.ToMonoColor());
        Refresh();
    }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> and fills it with a <see cref="MonoColor"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color"><see cref="MonoColor"/> used to fill the area of the <see cref="Canvas"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Canvas(int width, int height, MonoColor color) : this(width, height)
    {
        Array.Fill(Buffer, color);
        Refresh();
    }

    /// <summary>
    /// Constructor that creates a <see cref="Canvas"/> from an image file.
    /// </summary>
    /// <param name="fileName">File containing an image.</param>
    /// <exception cref="FileNotFoundException">Thrown when the file is not found.</exception>
    /// <exception cref="FormatException">Thrown when the image file has an unsupported extension.</exception>
    public Canvas(string fileName) : base(fileName)
    {
        Buffer = new MonoColor[Size];
    }
}