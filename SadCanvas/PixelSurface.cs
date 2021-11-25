using SadConsole.Host;
using SadConsole.DrawCalls;
using SadConsole.Components;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace SadCanvas;

/// <summary>
/// Wraps Texture2D without the use of a buffer. Good for loading and displaying images that won't need any changes.
/// </summary>
public partial class PixelSurface : ScreenObject, IDisposable
{
    /// <summary>
    /// Formats supported by the MonoGame <see cref="Texture2D"/>.
    /// </summary>
    static readonly string[] s_supportedFormats = { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

    /// <summary>
    /// Backing texture used in rendering.
    /// </summary>
    Texture2D _texture;

    /// <summary>
    /// Used by the disposing logic.
    /// </summary>
    bool _disposedValue = false;

    /// <summary>
    /// Area of the <see cref="PixelSurface"/> in pixels.
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
    /// Backing texture used in rendering.
    /// </summary>
    public Texture2D Texture
    {
        get => _texture;
        set
        {
            _texture?.Dispose();
            _texture = value;
            Area = new Rectangle(0, 0, _texture.Width, _texture.Height);
            Size = Width * Height;
        }
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

    /// <inheritdoc/>
    public override void Update(TimeSpan delta)
    {
        if (!IsEnabled) return;

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

    public void Dispose()
    {
        if (!_disposedValue)
        {
            _texture?.Dispose();
            _disposedValue = true;
        }
    }

    ~PixelSurface() => Dispose();

    #endregion
}