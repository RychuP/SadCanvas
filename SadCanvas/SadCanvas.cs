using System;
using SadConsole;
using SadConsole.Host;
using SadConsole.Renderers;
using SadRogue.Primitives;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Renderers.Constants;
using SadConsole.DrawCalls;
using SadConsole.Components;
using Microsoft.Xna.Framework;
using MonoColor = Microsoft.Xna.Framework.Color;
using Point = SadRogue.Primitives.Point;
using Color = SadRogue.Primitives.Color;

namespace SadCanvas
{
    /// <summary>
    /// Canvas class that allows pixel manipulations with MonoGame host.
    /// </summary>
    public partial class Canvas : ScreenObject, IDisposable
    {
        /// <summary>
        /// Formats supported by the MonoGame <see cref="Texture2D"/>.
        /// </summary>
        static string[] s_supportedFormats = { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

        /// <summary>
        /// Texture used in rendering <see cref="Canvas"/>.
        /// </summary>
        Texture2D _texture;

        /// <summary>
        /// Used by the disposing logic.
        /// </summary>
        bool _disposedValue = false;

        /// <summary>
        /// Cache of the texture pixels.
        /// </summary>
        public MonoColor[] Cache { get; private set; }

        /// <summary>
        /// Width in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height in pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Total number of pixels.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Used in positioning. To be implemented...
        /// </summary>
        public Point FontSize { get; set; }

        /// <summary>
        /// Turns on using AbsolutePosition rather than Position during rendering. To be implemented...
        /// </summary>
        public bool UsePixelPositioning { get; set; }

        /// <summary>
        /// To be implemented...
        /// </summary>
        public Color Tint { get; set; }

        /// <summary>
        /// Constructor that creates an empty <see cref="Canvas"/> of given dimensions and fills it with optional <see cref="MonoColor"/>.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="color"><see cref="MonoColor"/> used to fill the area of the <see cref="Canvas"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Canvas(int width, int height, MonoColor? color = null)
        {
            string? message = "Size of the Canvas cannot be negative.";
            if (width < 0 || height < 0) throw new ArgumentOutOfRangeException(message);
            
            _texture = new Texture2D(Global.GraphicsDevice, width, height);
            Cache = SetDimensions();

            FontSize = GameHost.Instance.DefaultFont.GetFontSize(IFont.Sizes.One);

            if (color != null)
            {
                Fill(color.Value);
                Draw();
            }
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
            Width = _texture.Width;
            Height = _texture.Height;
            Size = Width * Height;
            return new MonoColor[Size];
        }

        public override void Update(TimeSpan delta)
        {
            if (!IsEnabled) return;

            foreach (IComponent component in ComponentsUpdate.ToArray())
                component.Update(this, delta);

            foreach (IScreenObject child in new List<IScreenObject>(Children))
                child.Update(delta);
        }

        public override void Render(TimeSpan delta)
        {
            if (!IsVisible) return;

            // This will be changed to use Position and AbsolutePosition...
            var position = Parent is null ? Position : 
                Parent is Canvas ? Parent.Position + Position :
                Parent.AbsolutePosition + Position;
            var drawCall = new DrawCallTexture(_texture, new Vector2(position.X, position.Y));
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

        public void Fill(MonoColor color)
        {
            Array.Fill(Cache, color);
        }

        public void Draw()
        {
            _texture.SetData(Cache);
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
}