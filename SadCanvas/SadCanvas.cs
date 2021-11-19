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

namespace SadCanvas
{
    /// <summary>
    /// Canvas class that allows pixel manipulations with MonoGame host.
    /// </summary>
    public class Canvas : ScreenObject, IDisposable
    {
        static string[] s_supportedFormats = { ".bmp", ".gif", ".jpg", ".png", ".tif", ".dds" };

        Texture2D _texture;
        bool _disposedValue = false;

        public MonoColor[] Cache { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Size { get; private set; }

        public Canvas(int width, int height, MonoColor? color = null)
        {
            _texture = new Texture2D(Global.GraphicsDevice, width, height);
            Cache = SetDimensions();

            if (color != null)
            {
                Fill(color.Value);
                Draw();
            }
        }

        public Canvas(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (!File.Exists(fileName)) throw new FileNotFoundException();
            if (!s_supportedFormats.Contains(extension)) throw new FormatException("Image file format is unsupported by Texture2D.");

            using (Stream stream = File.OpenRead(fileName))
            {
                _texture = Texture2D.FromStream(Global.GraphicsDevice, stream);
            }

            Cache = SetDimensions();
        }

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

            var position = Parent is null ? Position : Parent.AbsolutePosition + Position;
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