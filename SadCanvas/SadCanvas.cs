global using System;
global using SadConsole;
global using SadRogue.Primitives;
global using MonoColor = Microsoft.Xna.Framework.Color;

using SadConsole.Host;
using SadConsole.DrawCalls;
using SadConsole.Components;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace SadCanvas;

/// <summary>
/// A basic canvas surface that allows pixel manipulation with SadConsole and MonoGame host.
/// </summary>
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Formats supported by the MonoGame <see cref="Texture2D"/>.
    /// </summary>
    static readonly string[] s_supportedFormats = { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

    /// <summary>
    /// Texture used in rendering <see cref="Canvas"/>.
    /// </summary>
    readonly Texture2D _texture;

    /// <summary>
    /// Used by the disposing logic.
    /// </summary>
    bool _disposedValue = false;

    /// <summary>
    /// Area of the <see cref="Canvas"/> in pixels.
    /// </summary>
    Rectangle _area;

    /// <summary>
    /// Cache of the pixels in <see cref="Canvas"/> backing texture.
    /// </summary>
    /// <remarks>Remember to set the <see cref="IsDirty"/> flag to true when changing <see cref="Cache"/> with outside methods.</remarks>
    public MonoColor[] Cache { get; private set; }

    /// <summary>
    /// Width in pixels.
    /// </summary>
    public int Width => _area.Width;

    /// <summary>
    /// Height in pixels.
    /// </summary>
    public int Height => _area.Height;

    /// <summary>
    /// Total number of pixels.
    /// </summary>
    public int Size { get; private set; } 

    /// <summary>
    /// When <see cref="UsePixelPositioning"/> is set to false, <see cref="IScreenObject.Position"/> is based on this value.
    /// </summary>
    public Point FontSize { get; set; }

    /// <summary>
    /// Treats the <see cref="IScreenObject.Position"/> as if it is pixels and not cells.
    /// </summary>
    public bool UsePixelPositioning { get; set; }

    /// <summary>
    /// To be implemented...
    /// </summary>
    public Color Tint { get; set; }

    /// <summary>
    /// Indicates the texture cache has changed and <see cref="Canvas"/> needs to be redrawn.
    /// </summary>
    /// <remarks>This property will be set to true automatically when using any of the drawing methods.</remarks>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> of given dimensions.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Canvas(int width, int height)
    {
        string message = "Size of the Canvas cannot be zero or negative.";
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(message);

        _texture = new Texture2D(Global.GraphicsDevice, width, height);
        Cache = SetDimensions();

        FontSize = GameHost.Instance.DefaultFont.GetFontSize(IFont.Sizes.One);
    }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> of given dimensions and fills it with the given <see cref="Color"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color"><see cref="Color"/> used to fill the area of the <see cref="Canvas"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Canvas(int width, int height, Color color) : this(width, height)
    {
        Array.Fill(Cache, color.ToMonoColor());
        Refresh();
    }

    /// <summary>
    /// Constructor that creates an empty <see cref="Canvas"/> of given dimensions and fills it with the given <see cref="Color"/>.
    /// </summary>
    /// <param name="width">Width in pixels.</param>
    /// <param name="height">Height in pixels.</param>
    /// <param name="color"><see cref="MonoColor"/> used to fill the area of the <see cref="Canvas"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Canvas(int width, int height, MonoColor color) : this(width, height)
    {
        Array.Fill(Cache, color);
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

        Cache = SetDimensions();
    }

    /// <summary>
    /// Sets dimensions used by the <see cref="Canvas"/> based on the underlying texture.
    /// </summary>
    /// <returns>A new cache array of MonoColors.</returns>
    MonoColor[] SetDimensions()
    {
        _area = new Rectangle(0, 0, _texture.Width, _texture.Height);
        Size = Width * Height;
        return new MonoColor[Size];
    }

    /// <summary>
    /// Updates <see cref="IScreenObject.AbsolutePosition"/>.
    /// </summary>
    public override void UpdateAbsolutePosition()
    {
        if (UsePixelPositioning)
            AbsolutePosition = Position + (Parent?.AbsolutePosition ?? new Point(0, 0));
        else
            AbsolutePosition = (FontSize * Position) + (Parent?.AbsolutePosition ?? new Point(0, 0));

        int count = Children.Count;
        for (int i = 0; i < count; i++)
            Children[i].UpdateAbsolutePosition();
    }

    /// <summary>
    /// Refreshes the entire <see cref="Canvas"/> with data from <see cref="Cache"/> or only a selected update area.
    /// </summary>
    /// <param name="updateArea">Area of the <see cref="Canvas"/> to be refreshed (not yet implemented).</param>
    private void Refresh(Rectangle? updateArea = null)
    {
        if (updateArea is null)
            _texture.SetData(Cache);
        else
        {
            // _texture.SetData(0, updateArea, arrayWithMonoColors, startIndex, pixelCount);
        }
    }

    public override void Update(TimeSpan delta)
    {
        if (!IsEnabled) return;

        if (IsDirty)
        {
            Refresh();
            IsDirty = false;
        }

        foreach (IComponent component in ComponentsUpdate.ToArray())
            component.Update(this, delta);

        foreach (IScreenObject child in new List<IScreenObject>(Children))
            child.Update(delta);
    }

    public override void Render(TimeSpan delta)
    {
        if (!IsVisible) return;

        var drawCall = new DrawCallTexture(_texture, new Vector2(AbsolutePosition.X, AbsolutePosition.Y));
        GameHost.Instance.DrawCalls.Enqueue(drawCall);

        int count = ComponentsRender.Count;
        for (int i = 0; i < count; i++)
            ComponentsRender[i].Render(this, delta);

        Children.IsLocked = true;
        count = Children.Count;
        for (int i = 0; i < count; i++)
            Children[i].Render(delta);
        Children.IsLocked = false;
    }

    #region IDisposable

    public void Dispose()
    {
        if (!_disposedValue)
        {
            _texture?.Dispose();
            _disposedValue = true;
        }
    }

    ~Canvas() => Dispose();

    #endregion
}