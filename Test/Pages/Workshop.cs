﻿using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages
{
    internal class Workshop : Page
    {
        public Workshop() : base("Workshop", "Tests of various features in development.")
        {
            Add(new DrawingBoard());
        }

        internal class DrawingBoard : Canvas
        {
            public DrawingBoard() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
            {
                var line = new Line((10, 10), (300, 170), GetRandomColor());
                DrawLine(line);
                SetPixel(line.GetCenter(), MonoColor.Red);

                line.Offset((150, 70));
                line.Color = GetRandomColor();
                Draw(line);

                line.Rotate(1.5f);
                line.Color = GetRandomColor();
                Draw(line);

                line.Scale(0.5f);
                line.Offset(50, 0);
                Draw(line);

                var square = new Square((10, 10), 70)
                {
                    FillColor = GetRandomColor(),
                };
                Draw(square);

                square.Color = GetRandomColor();
                square.Rotate(0.4f);
                square.Offset(70, 20);
                Draw(square);
            }
        }
    }
}