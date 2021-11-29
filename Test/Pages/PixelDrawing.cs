using Test.Screen;
using SadCanvas.Shapes;
using Rectangle = SadCanvas.Shapes.Rectangle;

namespace Test.Pages;

internal class PixelDrawing : Page
{
    public PixelDrawing() : base("Pixel Drawing", "Implemented are methods for drawing various, primitive shapes.")
    {
        _ = new RandomShapes(MonoColor.Bisque)
        {
            Parent = this,
            Position = (33, 1),
        };

        _ = new RandomShapes(MonoColor.LightBlue)
        {
            Parent = this,
            Position = (1, 6),
        };

        _ = new RandomShapes(MonoColor.Green)
        {
            Parent = this,
            Position = (26, 13),
        };
    }

    internal class RandomShapes : Canvas
    {
        public RandomShapes(MonoColor color) : base(350, 250, color)
        {
            int totalNoOfAvailableShapes = Enum.GetNames(typeof(ShapeNames)).Length;
            int NoOfShapesToGenerate = totalNoOfAvailableShapes * 3;
            int shapeNamesCurrentNumber = 0;

            for (int i = 0; i < NoOfShapesToGenerate; i++)
            {
                ShapeNames shapeName = (ShapeNames) shapeNamesCurrentNumber;
                Shape randomShape = shapeName switch
                {
                    ShapeNames.Circle => Circle.GetRandomCircle(Area, 20, 150),
                    ShapeNames.Ellipse => Ellipse.GetRandomEllipse(Area, 20, 150, 100),
                    ShapeNames.Rectangle => Rectangle.GetRandomRectangle(Area, 20, 150),
                    ShapeNames.Square => Square.GetRandomSquare(Area, 20, 150),
                    ShapeNames.Triangle => Triangle.GetRandomTriangle(Area, 20, 150),
                    _ => Polygon.GetRandomPolygon(Area, 4, 7),
                };
                Draw(randomShape);
                if (++shapeNamesCurrentNumber >= totalNoOfAvailableShapes)
                    shapeNamesCurrentNumber = 0;
            }
        }
    }

    enum ShapeNames
    {
        Rectangle,
        Circle,
        Ellipse,
        Square,
        Triangle,
        Polygon
    }
}