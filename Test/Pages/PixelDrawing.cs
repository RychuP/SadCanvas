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
                ShapeNames shapeName = (ShapeNames) shapeNamesCurrentNumber; // Game.Instance.Random.Next(totalNoOfAvailableShapes);
                Shape randomShape = shapeName switch
                {
                    ShapeNames.Ellipse => new Ellipse(this),
                    ShapeNames.Rectangle => new Rectangle(this),
                    ShapeNames.Square => new Square(this),
                    ShapeNames.Triangle => new Triangle(this),
                    _ => new Circle(this),
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
        Triangle
    }
}