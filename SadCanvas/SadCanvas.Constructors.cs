using SadConsole.Host;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

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
        string message = "Size of the Canvas cannot be zero or negative.";
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(message);

        _texture = new Texture2D(Global.GraphicsDevice, width, height);
        SetDimensions();
    }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> and fills it with a <see cref="MonoColor"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color"><see cref="MonoColor"/> that will become a <see cref="DefaultBackground"/> and fill the area of <see cref="Canvas"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    /// <exception cref="FileNotFoundException">Thrown when the file is not found.</exception>
    /// <exception cref="FormatException">Thrown when the image file has an unsupported extension.</exception>
    public Canvas(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        if (!File.Exists(fileName)) throw new FileNotFoundException();
        if (!s_supportedFormats.Contains(extension)) throw new FormatException("Image file format is unsupported by Texture2D.");

        using (Stream stream = File.OpenRead(fileName))
            _texture = Texture2D.FromStream(Global.GraphicsDevice, stream);

        // this constructor does not initiate the buffer
        Area = new Rectangle(0, 0, _texture.Width, _texture.Height);
        Size = Width * Height;
    }
}