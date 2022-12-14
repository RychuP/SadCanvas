namespace SadCanvas;

// Class constructors.
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Creates a surface from the <paramref name="texture"/>.
    /// </summary>
    /// <param name="texture"><see cref="Texture2D"/> to use a surface.</param>
    public Canvas(Texture2D texture)
    {
        _texture = texture;
        SetDimensions();
    }

    /// <summary>
    /// Creates a transparent surface of given <paramref name="width"/> and <paramref name="height"/>.
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
    /// Creates a surface of given <paramref name="width"/> and <paramref name="height"/> and fills it with <paramref name="color"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color">Color to be used as a <see cref="DefaultBackground"/>.</param>
    public Canvas(int width, int height, Color color) : this(width, height)
    {
        DefaultBackground = color;
        Fill(color);
        _texture.SetData(_buffer);
    }

    /// <summary>
    /// Creates a surface from an image file.
    /// </summary>
    /// <param name="fileName">Path to an image file.</param>
    public Canvas(string fileName)
    {
        _texture = LoadTexture(fileName);
        SetDimensions();
    }
}