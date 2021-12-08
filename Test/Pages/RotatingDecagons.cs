using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages
{
    internal class RotatingDecagons : Page
    {
        public RotatingDecagons() : base("Animated Decagons", "Iterating through rotations in Update to produce a simple animation.")
        {
            Add(new Animation());
            
            // small note in the top left corner
            var d = new ScreenSurface(25, 4)
            {
                Parent = this,
                Position = (1, 1),
            };
            d.Surface.DefaultForeground = Color.Brown;
            d.Surface.Clear();
            Print("Drawing routines are CPU",
                  "based. Do not try",
                  "this in debug",
                  "mode.");

            // small note in the bottom left corner
            d = new ScreenSurface(25, 4)
            {
                Parent = this,
                Position = (1, 25),
            };
            d.Surface.DefaultForeground = Color.Brown;
            d.Surface.Clear();
            Print("Build",
                  "the release",
                  "version instead.",
                  "This will get smooth.");

            void Print(params string[] t)
            {
                for (int i = 0; i < t.Length; i++)
                    d?.Surface.Print(0, i, t[i]);
            }
        }

        internal class Animation : Canvas
        {
            readonly Polygon _p, _p2;
            bool drawOuterFilled, drawInnerFilled;
            readonly float _angle = (float)Math.Tau / 200;
            TimeSpan _time = TimeSpan.Zero;

            public Animation() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
            {
                Point center = (Width / 2, Height / 2);
                _p = new Circle(center, 230, true, 10);
                _p2 = new Circle(center, 130, true, 10);
            }

            public override void Update(TimeSpan delta)
            {
                _time += delta;

                if (_time > TimeSpan.FromSeconds(8))
                {
                    _time = TimeSpan.Zero;
                    drawInnerFilled = false;
                }
                else if (_time > TimeSpan.FromSeconds(6))
                {
                    _p2.FillColor = GetRandomColor();
                    drawOuterFilled = false;
                }
                else if (_time > TimeSpan.FromSeconds(4))
                    drawOuterFilled = true;
                else if (_time > TimeSpan.FromSeconds(2))
                    drawInnerFilled = true;

                Clear();
                _p.Rotate(_angle);
                _p2.Rotate(-_angle);
                Draw(_p, drawOuterFilled);
                Draw(_p2, drawInnerFilled);
                base.Update(delta);
            }
        }
    }
}