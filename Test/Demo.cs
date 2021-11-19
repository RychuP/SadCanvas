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
    internal class Demo : Canvas
    {
        int currentColumn = 0;

        public Demo() : base(300, 100, Color.LightBlue.ToMonoColor())
        {
            
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            var c = Program.RandomColor.ToMonoColor();

            currentColumn++;
            if(currentColumn > Width) currentColumn = 0;

            for (int y = 0; y < Height; y++)
            {
                Point p = new(currentColumn, y);
                int i = p.ToIndex(Width);
                if (i < Size)
                    Cache[i] = c;
            }

            Draw();
        }
    }
}
