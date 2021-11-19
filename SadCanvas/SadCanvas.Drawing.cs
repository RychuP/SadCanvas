using SadConsole;
using MonoColor = Microsoft.Xna.Framework.Color;

namespace SadCanvas
{
    public partial class Canvas : ScreenObject, IDisposable
    {
        public void Fill(MonoColor color)
        {
            Array.Fill(Cache, color);
        }
    }
}
