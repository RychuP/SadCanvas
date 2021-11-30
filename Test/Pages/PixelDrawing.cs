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

        var c = new Canvas(350, 250, MonoColor.Green)
        {
            Parent = this,
            Position = (26, 13),
        };

        Triangle triangle = new(
            (120, 150),
            (23, 235),
            (155, 230),
            MonoColor.Salmon
            )
        {
            FillColor = MonoColor.SkyBlue
        };
        c.Draw(triangle, true);

        Circle circle = new((210, 45), 35, MonoColor.Yellow)
        {
            FillColor = MonoColor.OrangeRed
        };
        c.Draw(circle, true);

        Square square = new((267, 161), 65, Canvas.GetRandomColor())
        {
            FillColor = MonoColor.Chartreuse
        };
        c.Draw(square, true);

        Polygon polygon = new(new Point[] {
            (37, 166),
            (113, 50),
            (47, 78),
            (304, 109),
            (294, 17),
            (219, 233),
            (130, 108)
            },
            MonoColor.LightCoral)
        {
            FillColor = MonoColor.SandyBrown
        };
        c.Draw(polygon, true);
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
                Polygon randomShape = shapeName switch
                {
                    ShapeNames.Circle => new Circle(Area, 20, 150),
                    ShapeNames.Ellipse => new Ellipse(Area, 20, 150, 100),
                    ShapeNames.Rectangle => new Rectangle(Area, 20, 150),
                    ShapeNames.Square => new Square(Area, 20, 150),
                    ShapeNames.Triangle => new Triangle(Area, 20, 150),
                    _ => new Polygon(Area, 4, 7),
                };
                Draw(randomShape, true);
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