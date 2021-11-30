using SadCanvas.Shapes;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace SadCanvas;

/// <summary>
/// Dedicated class for drawing filled polygons.
/// </summary>
internal class DrawingBoard
{
    readonly byte[] _data;
    readonly Rectangle _bounds;
    readonly Point _absolutePosition;

    public int Width => _bounds.Width;

    public int Height => _bounds.Height;

    public int Size => _data.Length;

    public DrawingBoard(Rectangle bounds)
    {
        _bounds = bounds;
        _data = new byte[bounds.Width * bounds.Height];
        _absolutePosition = (Math.Abs(_bounds.Position.X), Math.Abs(_bounds.Position.Y));
    }

    public bool HasWall(int index) => Helpers.HasFlag(_data[index], 1);

    public bool HasColor(int index) => Helpers.HasFlag(_data[index], 2);

    public void DrawLine(Line line)
    {
        Algorithms.Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y, processor);

        bool processor(int x, int y)
        {
            Point p = (x, y);
            Point p2 = p - _absolutePosition;
            if (_bounds.Contains(p))
                _data[p2.ToIndex(_bounds.Width)] = 1;
            return false;
        }
    }

    /// <summary>
    /// Creates markers for <see cref="Polygon.FillColor"/>.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Fill()
    {
        int nextLineIndexStart;
        bool lineWasDrawnThisPass;
        for (int y = 0; y < Height; y++)
        {
            lineWasDrawnThisPass = false;
            nextLineIndexStart = -1;
            for (int x = 0; x < Width; x++)
            {
                Point p = (x, y);
                int currentIndex = p.ToIndex(Width);

                byte current = _data[currentIndex];
                // 1. line is not started -> look for walls
                if (nextLineIndexStart == -1)
                {
                    // encountered wall -> start line
                    if (Helpers.HasFlag(current, 1))
                        nextLineIndexStart = currentIndex + 1;
                }
                // 1. line is started
                else
                {
                    // encountered wall -> finish drawing the line
                    if (Helpers.HasFlag(current, 1))
                    {
                        // two walls in a row -> move start of the line
                        int length = currentIndex - nextLineIndexStart;
                        if (length == 0)
                        {
                            nextLineIndexStart = currentIndex + 1;
                        }
                        // 2. line has some length
                        else
                        {
                            // reject lines in the first row
                            if (y == 0)
                            {
                                nextLineIndexStart = -1;
                            }
                            // 3. line is not on the first row
                            else
                            {
                                // 4. line has no gaps above it
                                if (CheckLineAbove(nextLineIndexStart, currentIndex, out int colorCount))
                                {
                                    // 5. first line since the last gap -> draw
                                    if (!lineWasDrawnThisPass)
                                    {
                                        DrawHorizontalLine(nextLineIndexStart, currentIndex);
                                    }
                                    // 5. not the first line this pass -> check
                                    else
                                    {
                                        // 6. line above does not consist of walls only -> draw
                                        if (colorCount > 0)
                                        {
                                            DrawHorizontalLine(nextLineIndexStart, currentIndex);
                                        }

                                        // 6. there is only walls above this line -> reset
                                        else
                                        {
                                            lineWasDrawnThisPass = false;
                                            nextLineIndexStart = currentIndex + 1;
                                        }
                                    }
                                }
                                // 4. there is only empty space above this line -> reset
                                else
                                {
                                    lineWasDrawnThisPass = false;
                                    nextLineIndexStart = currentIndex + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        // draws a horizontal line
        void DrawHorizontalLine(int startIndex, int endIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
                _data[i] |= 2;
            nextLineIndexStart = endIndex + 1;
            lineWasDrawnThisPass = true;
        }

        // checks if the line above has any empty spaces
        bool CheckLineAbove(int startIndex, int endIndex, out int colorCount)
        {
            startIndex -= Width;
            endIndex -= Width;
            colorCount = 0;

            // 0b0001 = wall, 0b0010 = fill color
            for (int i = startIndex; i < endIndex; i++)
            {
                bool hasWall = (_data[i] & 1) == 1;
                bool hasColor = (_data[i] & 2) == 2;
                if (!hasWall && !hasColor) 
                    return false;
                else if (hasColor)
                    colorCount++;
            }
            return true;
        }
    }
}