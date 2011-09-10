using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raycaster.Enums;
using OpenTK;

namespace Raycaster.Entities
{
    /// <summary>
    /// Represents an intersection on the grid
    /// </summary>
    public class Intersection
    {
        #region Public properties
        /// <summary>
        /// X coordinate of the intersection
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate of the intersection
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The axe on which the intersection is located
        /// </summary>
        public IntersectionAxe IntersectsWith { get; set; }

        /// <summary>
        /// Gets or sets the distance between player and wall
        /// </summary>
        public double Distance { get; set; }
        #endregion

        public Intersection()
        {
        }

        /// <summary>
        /// Set the Distance property and returns value
        /// </summary>
        /// <param name="player"></param>
        public double SetDistance(Vector2d distanceFrom, Angle angle)
        {
            Distance = Math.Abs((double)Math.Abs(distanceFrom.X - X) / Math.Cos(angle.ToRadians())) + 0.0001; // Add some value to prevent division by zero
            return Distance;
        }
    }
}
