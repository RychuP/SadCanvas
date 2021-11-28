using SadConsole.Host;

namespace SadCanvas;

// Methods relating to working with a texture.
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Formats supported by the MonoGame <see cref="Texture2D"/>.
    /// </summary>
    static readonly string[] s_supportedFormats = { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

    /// <summary>
    /// Backing texture used in rendering.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// Setter that replaces current surface texture with a new one.
    /// </summary>
    public Texture2D Texture
    {
        get => _texture;
        set
        {
            _texture?.Dispose();
            _texture = value;
            FreeBuffer();
            SetDimensions();
        }
    }

    /// <summary>
    /// Creates an empty <see cref="Texture2D"/>;
    /// </summary>
    /// <param name="width">Width of the texture.</param>
    /// <param name="height">Height of the texture.</param>
    /// <returns>An empty <see cref="Texture2D"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Texture2D CreateTexture(int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(Errors.TextureDimensionsZeroOrNegative);
        return new(Global.GraphicsDevice, width, height);
    }

    /// <summary>
    /// Loads an image from a file.
    /// </summary>
    /// <param name="fileName">File name to load.</param>
    /// <returns>An instance of <see cref="Texture2D"/>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="FileLoadException"></exception>
    public static Texture2D LoadTexture(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(Errors.FileNameEmpty);
        if (!File.Exists(fileName)) throw new FileNotFoundException(Errors.FileNotFound);
        if (!s_supportedFormats.Contains(extension)) throw new FileLoadException(Errors.UnsupportedFileExtension);
        return Texture2D.FromFile(Global.GraphicsDevice, fileName);
    }
}