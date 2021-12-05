using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadCanvas
{
    internal interface ICanvasObject
    {
        /// <summary>
        /// The position of the object on the screen.
        /// </summary>
        Point Position { get; set; }

        /// <summary>
        /// A position that is based on the current <see cref="Position"/> and <see cref="Parent"/> position, in pixels.
        /// </summary>
        Point AbsolutePosition { get; }

        /// <summary>
        /// Sets a value for <see cref="AbsolutePosition"/> based on the <see cref="Position"/> of this instance and the <see cref="Parent"/> instance.
        /// </summary>
        void UpdateAbsolutePosition();

        /// <summary>
        /// The parent object that this instance is a child of.
        /// </summary>
        ICanvasObject Parent { get; set; }

        /// <summary>
        /// The child objects of this instance.
        /// </summary>
        ScreenObjectCollection Children { get; }
    }
}