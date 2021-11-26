using SadCanvas.Shapes;

namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    internal static class Errors
    {
        public static readonly string CanvasDimensionsZeroOrNegative = "Width and height cannot be 0 or negative.";
        public static readonly string ResizeStartPointOutOfBounds = "Start point for the resize is outside the bounds of the texture.";
        public static readonly string FileNameEmpty = "File name null or empty.";
        public static readonly string UnsupportedFileExtension = "File extension not supported by Texture2D.";
        public static readonly string FileNotFound = "File with the specified path does not exist.";
        public static readonly string Err = "";
    }
}