global using System;
global using SadConsole;
global using SadRogue.Primitives;
global using MonoColor = Microsoft.Xna.Framework.Color;

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
    /// Cache of <see cref="MonoColor"/> in <see cref="Canvas"/> the backing texture.
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
    /// When <see cref="UsePixelPositioning"/> is set to false, <see cref="FontSize"/> is used in calculating <see cref="IScreenObject.Position"/>.
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
    /// Cache of <see cref="MonoColor"/> in <see cref="Canvas"/> the backing texture.
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
            SetDimensions();
        }
    }

    /// <summary>
    /// Uses texture width and height to set <see cref="Canvas"/> dimensions and <see cref="Buffer"/>.
    /// </summary>
    private void SetDimensions()
    {
        Area = new Rectangle(0, 0, _texture.Width, _texture.Height);
        Size = Width * Height;
        Buffer = new MonoColor[Size];
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
            // _texture.SetData(0, updateArea, arrayWithMonoColors, startIndex, pixelCount);
        }
    }

    /*
    public void Resize(int width, int height)
    {
        if (!ValidateDimensions(width, height)) throw new ArgumentException("Width and height must be more than 0.");

        if (width == Width && height == Height)
            return;
        else if (width > Width && height > Height)
        {

        }
        else if (width > Width && height <= Height)
        {

        }
        else if (width <= Width && height > Height)
        {

        }

        var newTexture = new Texture2D(Global.GraphicsDevice, width, height);

        Rectangle sourceRectangle = new Rectangle(0, 0, originalTexture.Width - 20, originalTexture.Height - 20);

        Texture2D cropTexture = new Texture2D(GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
        Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
        originalTexture.GetData(0, sourceRectangle, data, 0, data.Length);
        cropTexture.SetData(data);
    }
    */

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

        var drawCall = new DrawCallTexture(_texture, new Vector2(AbsolutePosition.X, AbsolutePosition.Y));
        GameHost.Instance.DrawCalls.Enqueue(drawCall);

        int count = ComponentsRender.Count;
        if (count > 0)
            for (int i = 0; i < count; i++)
                ComponentsRender[i].Render(this, delta);

        count = Children.Count;
        if (count > 0)
        {
            Children.IsLocked = true;
            for (int i = 0; i < count; i++)
                Children[i].Render(delta);
            Children.IsLocked = false;
        }
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