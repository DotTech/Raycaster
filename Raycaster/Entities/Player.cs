using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using Raycaster.Enums;

namespace Raycaster.Entities
{
    public class Player
    {
        public readonly int StepSize = 10;
        private int[][] walls;

        #region Properties
        /// <summary>
        /// Location of the player on the map
        /// </summary>
        public Vector2d Position { get; set; }
        
        /// <summary>
        /// The angle (in degrees) that the player is facing towards
        /// </summary>
        /// <remarks>
        /// 0=east, 90=north, 180=west, 270=south
        /// </remarks>
        public Angle Angle { get; set; }

        public MovingState MovingState { get; set; }

        public TurningState TurningState { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map">the world map</param>
        public Player(int[][] wallsGrid)
        {
            this.walls = wallsGrid;
        }

        /// <summary>
        /// Handles turning and moving the player according to its TurningState & MovingState
        /// </summary>
        public void Move()
        {
            if (TurningState == Enums.TurningState.Left)
            {
                TurnLeft();
            }

            if (TurningState == Enums.TurningState.Right)
            {
                TurnRight();
            }

            if (MovingState == Enums.MovingState.Forward)
            {
                MoveForward();
            }

            if (MovingState == Enums.MovingState.Backward)
            {
                MoveBackward();
            }
        }

        #region Private movement methods
        private void TurnRight()
        {
            Angle.Turn(-StepSize / 2);
        }

        private void TurnLeft()
        {
            Angle.Turn(StepSize / 2);
        }

        private void MoveBackward()
        {
            double stepSize = -StepSize;
            double deltaX = Math.Cos(Angle.ToRadians()) * stepSize;
            double deltaY = Math.Sin(Angle.ToRadians()) * stepSize;

            if (!HasWall((int)(Position.X + deltaX), (int)(Position.Y - deltaY)))
            {
                Position = new Vector2d(Position.X + deltaX, Position.Y - deltaY);
            }
        }

        private void MoveForward()
        {
            double stepSize = StepSize;
            double deltaX = Math.Cos(Angle.ToRadians()) * stepSize;
            double deltaY = Math.Sin(Angle.ToRadians()) * stepSize;

            if (!HasWall((int)(Position.X + deltaX), (int)(Position.Y - deltaY)))
            {
                Position = new Vector2d(Position.X + deltaX, Position.Y - deltaY);
            }
        }

        private bool HasWall(int x, int y)
        {
            int mapX = x / 64;
            int mapY = y / 64;

            return (mapY < 0 || mapY > walls.Length)
                   || (mapX < 0 && mapX > walls[0].Length)
                   || walls[mapY][mapX] != 0;
        }
        #endregion
    }
}
