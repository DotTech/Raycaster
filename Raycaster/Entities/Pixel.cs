using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Raycaster.Entities
{
    public class Pixel
    {
        /// <summary>
        /// X coordinate of the pixel
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate of the pixel
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Color of the pixel
        /// </summary>
        public Color Color { get; set; }
    }
}
