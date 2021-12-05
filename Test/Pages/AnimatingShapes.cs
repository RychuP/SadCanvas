using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages
{
    internal class AnimatingShapes : Page
    {
        public AnimatingShapes() : base("Animation", "CPU drawing can manage filled shapes, but it's too slow animating them.")
        {
            Add(new Animation());
            
            var top = new ScreenSurface(25, 4)
            {
                Parent = this,
                Position = (1, 1),
            };
            top.Surface.DefaultForeground = Color.Brown;
            top.Surface.Clear();
            top.Surface.Print(0, 0, "When outer polygon gets");
            top.Surface.Print(0, 1, "filled with color");
            top.Surface.Print(0, 2, "animation slows");
            top.Surface.Print(0, 3, "down :(");

            var bottom = new ScreenSurface(25, 4)
            {
                Parent = this,
                Position = (1, 25),
            };
            bottom.Surface.DefaultForeground = Color.Brown;
            bottom.Surface.Clear();
            bottom.Surface.Print(0, 0, "Wait for the");
            bottom.Surface.Print(0, 1, "loop to remove");
            bottom.Surface.Print(0, 2, "color from outer poly");
            bottom.Surface.Print(0, 3, "and the stutter will end.");
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
                _time += delta;

                if (_time > TimeSpan.FromSeconds(10))
                {
                    _time = TimeSpan.Zero;
                    _p.FillColor = GetRandomColor();
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