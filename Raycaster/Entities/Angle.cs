using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raycaster.Utils;

namespace Raycaster.Entities
{
    public class Angle
    {
        private double value;

        /// <summary>
        /// Angle in degrees (0=east, 90=north, 180=west, 270=south)
        /// </summary>
        public double Value 
        {
            get
            {
                return value;
            }
            set
            {
                this.value = Math.Round(value, 4);
                
                if (this.value < 0)
                {
                    this.value = 359 + this.value;
                }
                else if (Value > 359)
                {
                    this.value = this.value - 360;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">angle in degrees</param>
        public Angle(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Converts the angle from degrees to radians
        /// </summary>
        /// <returns></returns>
        public double ToRadians()
        {
            return Value.ToRadians();
        }

        /// <summary>
        /// Turns the angle with specified degrees counter-clockwise
        /// </summary>
        public void Turn(double degrees)
        {
            Value += degrees;
        }

        /// <summary>
        /// Returns true if angle is facing towards the northern region of the map
        /// </summary>
        /// <returns></returns>
        public bool FacingNorth()
        {
            return Value < 180 && Value > 0;
        }

        /// <summary>
        /// Returns true if angle is facing towards the southern region of the map
        /// </summary>
        /// <returns></returns>
        public bool FacingSouth()
        {
            return Value > 180 && Value <= 359;
        }

        /// <summary>
        /// Returns true if angle is facing towards the eastern region of the map
        /// </summary>
        /// <returns></returns>
        public bool FacingEast()
        {
            return (Value < 90 && Value >= 0) || (Value > 270 && Value <= 359);
        }

        /// <summary>
        /// Returns true if angle is facing towards the western region of the map
        /// </summary>
        /// <returns></returns>
        public bool FacingWest()
        {
            return (Value > 90 && Value < 270);
        }
    }
}
