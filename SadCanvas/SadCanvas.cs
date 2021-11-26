global using System;
global using SadConsole;
global using SadRogue.Primitives;
global using MonoColor = Microsoft.Xna.Framework.Color;

using SadConsole.Host;
using SadConsole.Components;
using SadConsole.DrawCalls;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SadCanvas;

/// <summary>
/// A basic canvas surface that allows pixel manipulation with SadConsole and MonoGame host.<br></br>
/// Uses <see cref="MonoColor"/> instead of the SadConsole <see cref="Color"/>.
/// </summary>
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
    /// Cache of <see cref="MonoColor"/> pixels in the backing texture.
    /// </summary>
    private MonoColor[] _buffer = Array.Empty<MonoColor>();

    /// <summary>
    /// Used by the disposing logic.
    /// </summary>
    private bool _disposedValue = false;

    /// <summary>
    /// Area of the <see cref="Canvas"/> in pixels.
    /// </summary>
    public Rectangle Area { get; private set; }

    /// <summary>
    /// Area of the <see cref="Canvas"/> in cells (rounded down).
    /// </summary>
    public Rectangle CellArea { get; private set; }

    /// <summary>
    /// Width in pixels.
    /// </summary>
    public int Width => Area.Width;

    /// <summary>
    /// Height in pixels.
    /// </summary>
    public int Height => Area.Height;

    /// <summary>
    /// Total number of pixels.
    /// </summary>
    public int Size { get; private set; }

    /// <summary>
    /// Width in cells that will fit into this <see cref="Canvas"/> (based on <see cref="FontSize"/>).
    /// </summary>
    public int CellWidth => CellArea.Width;

    /// <summary>
    /// Height in cells that will fit into this <see cref="Canvas"/> (based on <see cref="FontSize"/>).
    /// </summary>
    public int CellHeight => CellArea.Height;

    /// <summary>
    /// Total number of cells that will fit into this <see cref="Canvas"/> (based on <see cref="FontSize"/>).
    /// </summary>
    public int CellSize { get; private set; }

    /// <summary>
    /// Used in calculating various cell based properties, ie: <see cref="IScreenObject.Position"/>, <see cref="CellArea"/>, etc.
    /// </summary>
    public Point FontSize { get; set; } = GameHost.Instance.DefaultFont.GetFontSize(IFont.Sizes.One);

    /// <summary>
    /// Treats the <see cref="IScreenObject.Position"/> as if it is pixels and not cells.
    /// </summary>
    public bool UsePixelPositioning { get; set; }

    /// <summary>
    /// To be implemented...
    /// </summary>
    public Color Tint { get; set; }

    /// <summary>
    /// To be implemented...
    /// </summary>
    public byte Opacity { get; set; }

    /// <summary>
    /// Default foreground <see cref="Color"/> used in drawing lines and outlines of shapes.
    /// </summary>
    public MonoColor DefaultForeground { get; set; } = MonoColor.White;

    /// <summary>
    /// Default background <see cref="Color"/> used mainly by Fill and Clear methods.
    /// </summary>
    public MonoColor DefaultBackground { get; set; } = MonoColor.Transparent;

    /// <summary>
    /// Indicates the texture cache has changed and <see cref="Canvas"/> needs to be redrawn.
    /// </summary>
    /// <remarks>This property will be set to true automatically when using any of the drawing methods.</remarks>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Cache of <see cref="MonoColor"/> pixels in the backing texture.
    /// </summary>
    /// <remarks>Remember to set the <see cref="IsDirty"/> flag to true when changing <see cref="Buffer"/> with outside methods.</remarks>
    protected MonoColor[] Buffer
    {
        get
        {
            if (_buffer.Length > 0) return _buffer;
            else
            {
                _buffer = new MonoColor[Size];
                return _buffer;
            }
        }
        private set => _buffer = value;
    }

    /// <summary>
    /// Backing texture used in rendering.
    /// </summary>
    public Texture2D Texture
    {
        get => _texture;
        set
        {
            _texture?.Dispose();
            _texture = value;
            Buffer = Array.Empty<MonoColor>();
            SetDimensions();
        }
    }

    /// <summary>
    /// Uses texture width and height to set <see cref="Canvas"/> dimensions.
    /// </summary>
    private void SetDimensions()
    {
        Area = new Rectangle(0, 0, Texture.Width, Texture.Height);
        CellArea = new Rectangle(0, 0, Width / FontSize.X, Height / FontSize.Y);
        Size = Width * Height;
        CellSize = CellWidth * CellHeight;
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
    /// Refreshes the entire <see cref="Canvas"/> with data from <see cref="Buffer"/> or only a selected update area.
    /// </summary>
    /// <param name="updateArea">Area of the <see cref="Canvas"/> to be refreshed (not yet implemented).</param>
    private void Refresh(Rectangle? updateArea = null)
    {
        if (updateArea is null)
            Texture.SetData(Buffer);
        else
        {
            // Texture.SetData(0, updateArea, arrayWithMonoColors, startIndex, pixelCount);
        }
    }

    /// <summary>
    /// Creates an empty <see cref="Texture2D"/>;
    /// </summary>
    /// <param name="width">Width of the texture.</param>
    /// <param name="height">Height of the texture.</param>
    /// <returns>An instance of <see cref="Texture2D"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Texture2D CreateTexture(int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException("Width and height cannot be 0 or negative.");
        return new(Global.GraphicsDevice, width, height);
    }

    /// <summary>
    /// Loads an image from file and converts it to <see cref="Texture2D"/>.
    /// </summary>
    /// <param name="fileName">File name to load.</param>
    /// <returns>An instance of <see cref="Texture2D"/>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="FileLoadException"></exception>
    public static Texture2D LoadTexture(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        if (String.IsNullOrEmpty(fileName)) throw new ArgumentNullException("File name null or empty.");
        if (!File.Exists(fileName)) throw new FileNotFoundException();
        if (!s_supportedFormats.Contains(extension)) throw new FileLoadException("File extension not supported by Texture2D.");
        return Texture2D.FromFile(Global.GraphicsDevice, fileName);
    }

    /// <summary>
    /// Resizes <see cref="Canvas"/> to the new dimensions.
    /// </summary>
    /// <param name="width">New width of the <see cref="Canvas"/>.</param>
    /// <param name="height">New height of the <see cref="Canvas"/>.</param>
    /// <param name="startPoint">Start point from where to begin the resize on the old texture.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Resize(int width, int height, Point? startPoint = null)
    {
        Point PointZero = (0, 0);
        Point cutOffPoint = startPoint ?? PointZero;

        string wrongSize = "Width and height cannot be 0 or negative.";
        string wrongStartPoint = "Start point for the resize is outside the bounds of the texture.";
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(wrongSize);
        if (!Area.Contains(cutOffPoint)) throw new ArgumentOutOfRangeException(wrongStartPoint);
        if (width == Width && height == Height) return;

        var newTexture = CreateTexture(width, height);

        // establish the size and position of the rectangle covering the area of the old texture that needs to be copied to the new one
        int reducedWidth = Width - cutOffPoint.X;
        int reducedHeight = Height - cutOffPoint.Y;
        int w = reducedWidth >= newTexture.Width ? newTexture.Width: reducedWidth;
        int h = reducedHeight >= newTexture.Height ? newTexture.Height : reducedHeight;
        Rectangle sourceRectangle = new(cutOffPoint.X, cutOffPoint.Y, w, h);

        // get the fragment of the old texture that will go into the new one
        MonoColor[] data = new MonoColor[w * h];
        Texture.GetData(0, sourceRectangle.ToMonoRectangle(), data, 0, data.Length);

        // fill the new texture with background color
        MonoColor[] defaultBackground = new MonoColor[newTexture.Width * newTexture.Height];
        Array.Fill(defaultBackground, DefaultBackground);
        newTexture.SetData(defaultBackground);

        // insert the fragment of the old texture in the new one
        var destinationRectangle = sourceRectangle.WithPosition(PointZero);
        newTexture.SetData(0, destinationRectangle.ToMonoRectangle(), data, 0, data.Length);

        // replace the old texture
        Texture = newTexture;
    }

    /// <inheritdoc/>
    public override void Update(TimeSpan delta)
    {
        if (!IsEnabled) return;

        if (IsDirty)
        {
            Refresh();
            IsDirty = false;
        }

        if (ComponentsUpdate.Count > 0)
            foreach (IComponent component in ComponentsUpdate.ToArray())
                component.Update(this, delta);

        if (Children.Count > 0)
            foreach (IScreenObject child in new List<IScreenObject>(Children))
                child.Update(delta);
    }

    /// <inheritdoc/>
    public override void Render(TimeSpan delta)
    {
        if (!IsVisible) return;

        var drawCall = new DrawCallTexture(Texture, new Vector2(AbsolutePosition.X, AbsolutePosition.Y));
        GameHost.Instance.DrawCalls.Enqueue(drawCall);

        int count = ComponentsRender.Count;
        for (int i = 0; i < count; i++)
            ComponentsRender[i].Render(this, delta);

        count = Children.Count;
        Children.IsLocked = true;
        for (int i = 0; i < count; i++)
            Children[i].Render(delta);
        Children.IsLocked = false;
    }

    #region IDisposable

    /// <summary>
    /// Disposes the backing texture.
    /// </summary>
    public void Dispose()
    {
        if (!_disposedValue)
        {
            _texture?.Dispose();
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Disposes the <see cref="Canvas"/> instance.
    /// </summary>
    ~Canvas() => Dispose();

    #endregion
}