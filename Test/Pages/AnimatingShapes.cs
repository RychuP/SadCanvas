using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages
{
    internal class AnimatingShapes : Page
    {
        public AnimatingShapes() : base("Animation", "CPU drawing can manage filled shapes, but it's too slow animating them.")
        {
            Add(new Animation());
            var d = new ScreenSurface(25, 4)
            {
                Parent = this,
                Position = (1, 1),
            };
            d.Surface.DefaultForeground = Color.Brown;
            d.Surface.Clear();
            d.Surface.Print(0, 0, "Skip to the next page");
            d.Surface.Print(0, 1, "before the outer");
            d.Surface.Print(0, 2, "polygon gets filled");
            d.Surface.Print(0, 3, "with color.");
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
                _p = new Circle(center, 230, GetRandomColor(), 10)
                {
                    FillColor = GetRandomColor()
                };

                _p2 = new Circle(center, 130, GetRandomColor(), 10)
                {
                    FillColor = GetRandomColor()
                };
            }

            public override void Update(TimeSpan delta)
            {
                if (_time <= TimeSpan.FromSeconds(5))
                {
                    _time += delta;
                    if (_time > TimeSpan.FromSeconds(5))
                        drawOuterFilled = true;
                    else if (_time > TimeSpan.FromSeconds(2))
                        drawInnerFilled = true;
                }

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