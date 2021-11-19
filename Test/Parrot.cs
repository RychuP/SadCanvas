using System;
using System.Collections.Generic;
using System.Linq;
using SadConsole;
using SadRogue.Primitives;
using SadCanvas;

namespace Test
{
    internal class Parrot : Canvas
    {
        public Parrot() : base("Res/Images/parrot.jpg")
        {
            Children.Add(new Mario());
            Children.Add(new VerticalLines() { Position = (70, 430) });
            Children.Add(new VerticalLines() { Position = (70, -24) });
        }

        class Mario : ScreenSurface
        {
            public Mario() : base(16, 9)
            {
                var mario = new Canvas("Res/Images/mario.png") { Position = (16, 23) }
                Children.Add(mario);

                Surface.DrawBox(new Rectangle(0, 0, Surface.Width, Surface.Height),
                    ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick,
                    new ColoredGlyph(Color.Green, Color.Yellow))
                );

                Position = (4, 3);
            }
        }
    }
}
