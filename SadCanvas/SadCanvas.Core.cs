using SadConsole.Components;
using SadConsole.DrawCalls;

namespace SadCanvas;

/// <summary>
/// A basic canvas surface that allows pixel manipulation with SadConsole and MonoGame host.
/// </summary>
public partial class Canvas : ScreenObject, IDisposable
{
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
    /// Area in cells (rounded down).
    /// </summary>
    public Rectangle CellArea { get; private set; }

    /// <summary>
    /// Number of cells that will fit into the pixel <see cref="Width"/> (based on <see cref="FontSize"/>).
    /// </summary>
    public int CellWidth => CellArea.Width;

    /// <summary>
    /// Number of cells that will fit into the pixel <see cref="Height"/> (based on <see cref="FontSize"/>).
    /// </summary>
    public int CellHeight => CellArea.Height;

    /// <summary>
    /// Total number of cells that will fit into the pixel <see cref="Area"/> (based on <see cref="FontSize"/>).
    /// </summary>
    public int CellSize { get; private set; }

    /// <summary>
    /// Used for calculating various cell based properties (<see cref="IScreenObject.Position"/>, <see cref="CellArea"/>, etc).
    /// </summary>
    public Point FontSize { get; set; } = GameHost.Instance.DefaultFont.GetFontSize(IFont.Sizes.One);

    /// <summary>
    /// Treats the <see cref="IScreenObject.Position"/> as if it is pixels and not cells.
    /// </summary>
    public bool UsePixelPositioning { get; set; }

    /*
    public Color Tint { get; set; }

    public byte Opacity { get; set; }
    */

    /// <summary>
    /// Default background <see cref="Color"/>.
    /// </summary>
    public Color DefaultBackground { get; set; } = Color.Transparent;

    /// <summary>
    /// Indicates the texture buffer has changed and <see cref="Canvas"/> needs to be redrawn.
    /// </summary>
    /// <remarks>This property will be set to true automatically when using any of the drawing methods.<br></br>
    /// Set this flag to true only when you want to send data from <see cref="Buffer"/> to <see cref="Texture"/>.</remarks>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Sets <see cref="Canvas"/> dimensions according to the surface texture dimensions.
    /// </summary>
    private void SetDimensions()
    {
        Area = _texture.Bounds.ToSadRectangle();
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
    /// Resizes <see cref="Canvas"/> to the new <paramref name="width"/> and <paramref name="height"/>. 
    /// Fragment of the prev texture beginning with the <paramref name="startPoint"/> is copied to the new one.
    /// </summary>
    /// <param name="width">New width in pixels.</param>
    /// <param name="height">New height in pixels.</param>
    /// <param name="startPoint">Start point in pixels from where to begin the resize on the old texture.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Resize(int width, int height, Point? startPoint = null)
    {
        Point PointZero = (0, 0);
        Point cutOffPoint = startPoint ?? PointZero;
        
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(Errors.CanvasDimensionsZeroOrNegative);
        if (!IsValidPosition(cutOffPoint)) throw new ArgumentOutOfRangeException(Errors.ResizeStartPointOutOfBounds);
        if (width == Width && height == Height) return;

        var newTexture = CreateTexture(width, height);

        // establish the size and position of the rectangle covering the area of the old texture
        // that needs to be copied to the new one
        int reducedWidth = Width - cutOffPoint.X;
        int reducedHeight = Height - cutOffPoint.Y;
        int w = reducedWidth >= newTexture.Width ? newTexture.Width: reducedWidth;
        int h = reducedHeight >= newTexture.Height ? newTexture.Height : reducedHeight;
        Rectangle sourceRectangle = new(cutOffPoint.X, cutOffPoint.Y, w, h);

        // get the fragment of the old texture that will go into the new one
        MonoColor[] data = new MonoColor[w * h];
        _texture.GetData(0, sourceRectangle.ToMonoRectangle(), data, 0, data.Length);

        // fill the new texture with background color
        MonoColor[] defaultBackground = new MonoColor[newTexture.Width * newTexture.Height];
        Array.Fill<MonoColor>(defaultBackground, DefaultBackground.ToMonoColor());
        newTexture.SetData(defaultBackground);

        // insert the fragment of the old texture into the new one
        var destinationRectangle = sourceRectangle.WithPosition(PointZero);
        newTexture.SetData(0, destinationRectangle.ToMonoRectangle(), data, 0, data.Length);

        // replace the old texture
        Texture = newTexture;
    }

    /// <summary>
    /// Resizes <see cref="Canvas"/> to the new dimensions. Previous texture is scaled according to <paramref name="resizeOption"/>.
    /// </summary>
    /// <param name="width">New width in pixels.</param>
    /// <param name="height">New height in pixels.</param>
    /// <param name="resizeOption">Option for the texture scaling.</param>
    public void Resize(int width, int height, ResizeOptions resizeOption)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void Update(TimeSpan delta)
    {
        if (!IsEnabled) return;

        // set this flag to true only when you want to synchronise buffer with the texture
        if (IsDirty)
        { 
            if (_buffer.Length != Size) throw new InvalidOperationException(Errors.BufferSizeMismatch);
            _texture.SetData(_buffer);
            IsDirty = false;
        }

        base.Update(delta);
    }

    /// <inheritdoc/>
    public override void Render(TimeSpan delta)
    {
        if (!IsVisible) return;

        var drawCall = new DrawCallTexture(_texture, new Vector2(AbsolutePosition.X, AbsolutePosition.Y));
        GameHost.Instance.DrawCalls.Enqueue(drawCall);

        base.Render(delta);
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
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Disposes the <see cref="Canvas"/> instance.
    /// </summary>
    ~Canvas() => Dispose();

    #endregion
}