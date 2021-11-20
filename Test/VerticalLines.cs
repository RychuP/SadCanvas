using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;
using SadRogue.Primitives;
using SadCanvas;


namespace Test
{
    internal class VerticalLines : Canvas
    {
        int currentColumn = 0;

        public VerticalLines() : base(500, 20, Color.LightBlue.ToMonoColor()) { }

        public override void Update(TimeSpan delta)
        {
            var color = Program.RandomColor.ToMonoColor();

            currentColumn++;
            if(currentColumn > Width) currentColumn = 0;

            for (int y = 0; y < Height; y++)
            {
                Point position = new(currentColumn, y);
                SetPixel(position, color);
            }

            base.Update(delta);
        }
    }
}
