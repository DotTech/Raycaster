using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using Raycaster.Enums;

namespace Raycaster.Entities
{
    /// <summary>
    /// Represents an object in the game world (a wall or sprite) that was hit during ray casting
    /// </summary>
    public class WorldObject: Intersection
    {
        /// <summary>
        /// X coordinate on the object where the ray hit (required for texture mapping)
        /// </summary>
        public int TextureX { get; set; }

        /// <summary>
        /// Index of the texture to be used for this wall
        /// </summary>
        public int TextureIndex { get; set; }


        public WorldObject(Intersection intersection)
        {
            X = intersection.X;
            Y = intersection.Y;
            IntersectsWith = intersection.IntersectsWith;
            Distance = intersection.Distance;
        }
    }
}
