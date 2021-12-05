using SadCanvas.Shapes;
using Rectangle = SadRogue.Primitives.Rectangle;

namespace SadCanvas;

/// <summary>
/// Dedicated class for drawing filled polygons.
/// </summary>
internal class DrawingBoard : IDisposable
{
    // drawing data (0b0001 = wall, 0b0010 = fill color)
    static byte[] s_data = new byte[500 * 500];
    static bool s_isLocked = false;

    // bounding rectangle of a polygon
    readonly Rectangle _bounds;

    bool _disposedValue = false;

    public int Width => _bounds.Width;

    public int Height => _bounds.Height;

    public int Size { get; init; }

    internal enum Cell
    {
        Empty,
        Wall,
        Color
    }

    public DrawingBoard(Rectangle bounds)
    {
        if (s_isLocked) throw new InvalidOperationException("DrawingBoard can only have one instance at a time.");

        _bounds = bounds;
        Size = Width * Height;
        s_isLocked = true;

        if (Size > s_data.Length)
        {
            s_data = new byte[Size];
        }
        else
            Array.Clear(s_data, 0, Size);
    }

    /// <summary>
    /// Sets or gets appropriate flags in s_data with index converted from Point p.
    /// </summary>
    public Cell this[int index]
    {
        get
        {
            byte b = s_data[index];
            return Helpers.HasFlag(b, 1) ? Cell.Wall : Helpers.HasFlag(b, 2) ? Cell.Color : Cell.Empty;
        }
        set
        {
            byte b = s_data[index];
            if (value is Cell.Wall)
                s_data[index] = (byte)(b | 1);
            else if (value is Cell.Color)
                s_data[index] = (byte)(b | 2);
        }
    }

    public void DrawLine(Line line)
    {
        Algorithms.Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y, processor);

        bool processor(int x, int y)
        {
            Point globalPoint = (x, y);
            Point localPoint = globalPoint - _bounds.Position;
            if (_bounds.Contains(globalPoint))
                this[localPoint.ToIndex(Width)] = Cell.Wall;
            else
                throw new Exception();
            return false;
        }
    }

    /// <summary>
    /// Creates markers for <see cref="Polygon.FillColor"/>.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void Fill()
    {
        int nextLineStart, prevLineEnd = -1;
        bool lineWasDrawnThisPass;

        for (int y = 0; y < Height; y++)
        {
            lineWasDrawnThisPass = false;
            nextLineStart = -1;

            for (int x = 0; x < Width; x++)
            {
                Point currentPosition = (x, y);
                int currentIndex = currentPosition.ToIndex(Width);
                Cell cell = this[currentIndex];

                // line is not started 
                if (nextLineStart == -1 && cell is Cell.Wall)
                {
                    nextLineStart = currentIndex + 1;
                }
                // line is started
                else
                {
                    // keep going until a wall is found
                    if (cell is Cell.Wall)
                    {
                        // two walls in a row -> move start point to the next column
                        if (nextLineStart == currentIndex)
                        {
                            nextLineStart = currentIndex + 1;
                        }
                        // line has some length
                        else
                        {
                            // reject lines in the first row
                            if (y == 0)
                            {
                                nextLineStart = -1;
                            }
                            // line is not on the first row
                            else
                            {
                                // line has no gaps above it
                                if (LineAboveHasNoGaps(currentIndex, out int colorCount))
                                {
                                    // first line since the last gap -> draw
                                    if (!lineWasDrawnThisPass)
                                    {
                                        DrawHorizontalLine(nextLineStart, currentIndex);
                                    }
                                    // not the first line this pass -> check
                                    else
                                    {
                                        // 6. line above does not consist of walls only -> draw
                                        if (colorCount > 0)
                                        {
                                            DrawHorizontalLine(nextLineStart, currentIndex);
                                        }

                                        // 6. there are only walls above this line -> check prev line
                                        else
                                        {
                                            if (lineWasDrawnThisPass && PathToPrevLineIsOpen())
                                            {
                                                DrawHorizontalLine(nextLineStart, currentIndex);
                                            }
                                            else
                                            {
                                                lineWasDrawnThisPass = false;
                                                nextLineStart = currentIndex + 1;
                                            }
                                        }
                                    }
                                }
                                // 4. there is only empty space above this line -> reset
                                else
                                {
                                    lineWasDrawnThisPass = false;
                                    nextLineStart = currentIndex + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        // draws a horizontal line (end is a wall)
        void DrawHorizontalLine(int startIndex, int endIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
                this[i] = Cell.Color;
            nextLineStart = endIndex + 1;
            lineWasDrawnThisPass = true;
            prevLineEnd = endIndex - 1;
        }

        bool PathToPrevLineIsOpen()
        {
            int startIndex = prevLineEnd + Width;
            int endIndex = nextLineStart + Width;

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (this[i] is Cell.Wall)
                    return false;
            }
            return true;
        }

        // checks if the line above has any empty spaces
        bool LineAboveHasNoGaps(int endIndex, out int colorCount)
        {
            int startOfLineAbove = nextLineStart - Width;
            int endOfLineAbove = endIndex - Width;
            colorCount = 0;

            for (int i = startOfLineAbove; i < endOfLineAbove; i++)
            {
                if (this[i] is Cell.Empty)
                    return false;
                else if (this[i] is Cell.Color)
                    colorCount++;
            }
            return true;
        }
    }

    public void Dispose()
    {
        if (!_disposedValue)
        {
            s_isLocked = false;
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Disposes the <see cref="Canvas"/> instance.
    /// </summary>
    ~DrawingBoard() => Dispose();
}