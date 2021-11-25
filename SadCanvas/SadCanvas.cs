global using System;
global using SadConsole;
global using SadRogue.Primitives;
global using MonoColor = Microsoft.Xna.Framework.Color;

namespace SadCanvas;

/// <summary>
/// A basic canvas surface that allows pixel manipulation with SadConsole and MonoGame host.
/// </summary>
public partial class Canvas : PixelSurface
{
    /// <summary>
    /// Cache of pixels in <see cref="Canvas"/> backing texture.
    /// </summary>
    /// <remarks>Remember to set the <see cref="IsDirty"/> flag to true when changing <see cref="Buffer"/> with outside methods.</remarks>
    public MonoColor[] Buffer { get; private set; }

    /// <summary>
    /// Default foreground <see cref="Color"/> used in drawing lines and outlines of shapes.
    /// </summary>
    public Color DefaultForeground { get; set; } = Color.White;

    /// <summary>
    /// Default background <see cref="Color"/> used mainly by Fill and Clear methods.
    /// </summary>
    public Color DefaultBackground { get; set; } = Color.Transparent;

    /// <summary>
    /// Indicates the texture cache has changed and <see cref="Canvas"/> needs to be redrawn.
    /// </summary>
    /// <remarks>This property will be set to true automatically when using any of the drawing methods.</remarks>
    public bool IsDirty { get; set; }

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

        base.Update(delta);
    }
}