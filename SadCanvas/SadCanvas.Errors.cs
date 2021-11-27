namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Error messages used by local exceptions.
    /// </summary>
    internal static class Errors
    {
        public static readonly string CanvasDimensionsZeroOrNegative = "Canvas width and height cannot be 0 or negative.";
        public static readonly string ResizeStartPointOutOfBounds = "Start point for the resize is outside the bounds of the canvas.";
        public static readonly string PositionOutOfBounds = "Position is outside the bounds of the canvas.";
        public static readonly string FileNameEmpty = "File name is null or empty.";
        public static readonly string UnsupportedFileExtension = "File extension not supported by Texture2D.";
        public static readonly string FileNotFound = "File with the specified path does not exist.";
        public static readonly string Err = "";
    }
}