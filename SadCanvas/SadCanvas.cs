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
    /// MonoGame canvas class that allows pixel manipulations.
    /// </summary>
    public class Canvas : ScreenObject, IDisposable
    {
        Texture2D _texture;
        bool _disposedValue = false;

        public MonoColor[] Cache { get; init; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Size { get; private set; }

        public Canvas(int width, int height, MonoColor? color = null)
        {
            Width = width;
            Height = height;
            Size = width * height;
            Cache = new MonoColor[width * height];
            _texture = new Texture2D(Global.GraphicsDevice, width, height);

            if (color != null)
            {
                Fill(color.Value);
                Draw();
            }
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

            var drawCall = new DrawCallTexture(_texture, new Vector2(Position.X, Position.Y));
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