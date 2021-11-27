using SadCanvas.Shapes;

namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Resize options for the <see cref="Canvas"/>.
    /// </summary>
    public enum ResizeOptions
    {
        /// <summary>
        /// Stretches the output to fit the new size.
        /// </summary>
        Stretch,

        /// <summary>
        /// Scales output to fit the window as best as possible.
        /// </summary>
        Scale,

        /// <summary>
        /// Fits output to the window using padding and background color to maintain aspect ratio.
        /// </summary>
        Fit
    }
}