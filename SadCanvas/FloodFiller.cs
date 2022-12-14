using SadCanvas.Shapes;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace SadCanvas;

/// <summary>
/// Dedicated class for drawing filled polygons.
/// </summary>
class FloodFiller
{
    Cell[,] _cells = new Cell[0, 0];
    Point _origin = new Point(0, 0);
    int _width = 0;
    int _height = 0;

    public FloodFiller(Polygon polygon)
    {
        Draw(polygon);
    }

    public void Draw(Polygon polygon)
    {
        _width = polygon.Bounds.Width;
        _height = polygon.Bounds.Height;
        _origin = polygon.Bounds.Position;
        if (_cells.GetLength(0) < _width || _cells.GetLength(1) < _height)
        {
            _cells = new Cell[_width, _height];
        }
        else
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _cells[x, y] = Cell.Empty;
                }
            }
        }   
        
        // draw an outline
        foreach (var line in polygon.Edges)
            DrawLine(line);

        // fill the outline with color
        if (polygon is Ellipse || polygon is Shapes.Rectangle || polygon is Triangle)
            SimpleFill();
    }

    public Cell this[int x, int y] => _cells[x, y];

    void SimpleFill()
    {
        for (int y = 0; y < _height; y++)
        {
            int startX = -1;
            int endX = -1;

            for (int x = 0; x < _width; x++)
            {
                Cell cell = _cells[x, y];
                if (cell is Cell.Wall)
                {
                    if (startX == -1)
                    {
                        startX = x + 1;
                    }
                    else if (startX == x)
                    {
                        startX = x + 1;
                    }
                    else if (endX > startX)
                    {
                        break;
                    }
                }
                else
                {
                    endX = x + 1;
                }
            }

            // draw the line if it has any length
            if (endX > startX && endX != _width)
            {
                for (int x = startX; x < endX; x++)
                {
                    _cells[x, y] = Cell.Color;
                }
            }
        }
    }

    /// <summary>
    /// Creates markers for <see cref="Polygon.FillColor"/>.
    /// </summary>
    void Fill()
    {
        int prevLineEnd = -1;

        for (int y = 0; y < _height; y++)
        {
            bool lineWasDrawnThisPass = false;
            int nextLineStart = -1;

            for (int x = 0; x < _width; x++)
            {
                Cell cell = _cells[x, y];

                // line is not started 
                if (nextLineStart == -1 && cell is Cell.Wall)
                {
                    nextLineStart = x + 1;
                }
                // line is started
                else
                {
                    // keep going until a wall is found
                    if (cell is Cell.Wall)
                    {
                        // two walls in a row -> move start point to the next column
                        if (nextLineStart == x)
                        {
                            nextLineStart = x + 1;
                        }
                        // line has some length
                        else
                        {
                            // reject lines in the first row
                            if (y == 0)
                            {
                                nextLineStart = -1;
                            }
                            // line is not in the first row
                            else
                            {
                                // line above is made of colors or walls
                                if (LineHasNoEmptyCells(nextLineStart, x, y - 1, out int colorCount))
                                {
                                    // first line since the last gap -> draw
                                    if (!lineWasDrawnThisPass)
                                    {
                                        DrawHorizontalLine(nextLineStart, x, y);
                                        nextLineStart = x + 1;
                                        lineWasDrawnThisPass = true;
                                        prevLineEnd = x - 1;
                                    }
                                    // not the first line this pass -> check
                                    else
                                    {
                                        // 6. line above does not consist of walls only -> draw
                                        if (colorCount > 0)
                                        {
                                            DrawHorizontalLine(nextLineStart, x, y);
                                            nextLineStart = x + 1;
                                            lineWasDrawnThisPass = true;
                                            prevLineEnd = x - 1;
                                        }

                                        // 6. there are only walls above this line -> check prev line
                                        else
                                        {
                                            if (lineWasDrawnThisPass) // && PathToPrevLineIsOpen())
                                            {
                                                DrawHorizontalLine(nextLineStart, x, y);
                                                nextLineStart = x + 1;
                                                lineWasDrawnThisPass = true;
                                                prevLineEnd = x - 1;
                                            }
                                            else
                                            {
                                                lineWasDrawnThisPass = false;
                                                nextLineStart = x + 1;
                                            }
                                        }
                                    }
                                }
                                // 4. there is only empty space above this line -> reset
                                else
                                {
                                    lineWasDrawnThisPass = false;
                                    nextLineStart = x + 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //bool PathToPrevLineIsOpen()
    //{
    //    int startIndex = prevLineEnd + Width;
    //    int endIndex = nextLineStart + Width;

    //    for (int i = startIndex; i <= endIndex; i++)
    //    {
    //        if (GetCell(i) is Cell.Wall)
    //            return false;
    //    }
    //    return true;
    //}

    // checks if the line above has any empty spaces
    bool LineHasNoEmptyCells(int startX, int endX, int y, out int colorCount)
    {
        colorCount = 0;
        for (int x = startX; x < endX; x++)
        {
            Cell cell = _cells[x, y];
            if (cell is Cell.Empty)
                return false;
            else if (cell is Cell.Color)
                colorCount++;
        }
        return true;
    }

    // draws a line in any direction
    void DrawLine(Line line)
    {
        Algorithms.Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y, processor);

        bool processor(int x, int y)
        {
            _cells[x - _origin.X, y - _origin.Y] = Cell.Wall;
            return false;
        }
    }

    // draws a horizontal line
    void DrawHorizontalLine(int startX, int endX, int y)
    {
        for (int x = startX; x < endX; x++)
            _cells[x, y] = Cell.Color;
    }
}