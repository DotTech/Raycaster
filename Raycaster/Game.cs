using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;
using OpenTK;
using Raycaster.Entities;
using Raycaster.Utils;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Raycaster.Enums;

namespace Raycaster
{
    public class Game : GameWindow
    {
        #region Constants
        private readonly Size windowSize = Size.Empty;
        private readonly Size viewPort = new Size(640, 400);
        private readonly double wallSize = 64;
        private readonly double fieldOfView = 66;
        private readonly Point miniMapLocation = new Point(660, 10);
        private readonly double playerStartAngle = 351;
        private readonly double targetUpdatesPerSecond = 20;

        private readonly int[][] wallsGrid =
        {
            new int[] { 1, 1, 1, 5, 1, 1, 1, 5, 1, 1, 1, 5, 1, 1, 1, 5, 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 5, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            new int[] { 1, 0, 0, 0, 2, 2, 2, 0, 0, 4, 4, 4, 4, 0, 0, 4, 4, 4, 4, 0, 0, 0, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 5, 0, 6, 0, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 },
            new int[] { 1, 0, 6, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 6, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 5, 0, 6, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 6, 0, 3, 3, 3, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 1 },
            new int[] { 1, 0, 6, 0, 3, 0, 3, 0, 2, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1 },
            new int[] { 1, 0, 6, 0, 3, 3, 3, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 1 },
            new int[] { 5, 0, 6, 0, 0, 0, 0, 0, 2, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 1 },
            new int[] { 1, 0, 6, 6, 6, 6, 6, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 3, 1 },
            new int[] { 1, 0, 6, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 0, 0, 0, 3, 0, 0, 3, 0, 1 },
            new int[] { 5, 0, 6, 6, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 3, 0, 3, 0, 0, 1 },
            new int[] { 1, 0, 6, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 7, 0, 0, 0, 3, 0, 3, 3, 3, 1 },
            new int[] { 1, 0, 6, 6, 6, 6, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 0, 0, 0, 3, 3, 3, 3, 3, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 1, 1, 1, 1, 1, 1 },
        };
        #endregion

        #region Private fields
        private Point centerOfViewPort;
        private double distanceToViewPort;
        private Angle angleBetweenRays;
        private double timeSinceLastUpdate;

        private double fps;
        private long durationRenderCalculation;
        private long durationRenderDrawing;

        private OpenTK.Graphics.TextPrinter textPrinter;
        private Font debuggerFont;

        private Player player;
        private Textures textures;
        #endregion

        #region Constructor
        /// <summary>
        /// Game constructor
        /// </summary>
        /// <param name="windowSize">size of the game window</param>
        public Game(Size windowSize): base(windowSize.Width, windowSize.Height)
        {
            centerOfViewPort = new Point(viewPort.Width / 2, viewPort.Height / 2);
            distanceToViewPort = Math.Round((double)viewPort.Width / 2 / Math.Tan((fieldOfView / 2).ToRadians()), 0);
            angleBetweenRays = new Angle(fieldOfView / (double)viewPort.Width);
            this.windowSize = windowSize;

            // Setup the player in the world
            player = new Player(wallsGrid)
            {
                Angle = new Angle(playerStartAngle),
                Position = new Vector2d { X = 168, Y = 112 }
            };

            // Setup textprinter used for font rendering
            textPrinter = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Medium);
            debuggerFont = new Font(new FontFamily("Lucida Console"), 8.0f);
        }
        #endregion

        #region GameWindow events
        protected override void OnLoad(EventArgs e)
        {
            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyDown);
            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyUp);

            // Setup 2D mode with 0,0 coordinate being top left corner of the window
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, windowSize.Width, windowSize.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);

            // Load test texture
            textures = new Textures();
            textures.Add("redbrick.png", 1);
            textures.Add("wood.png", 2);
            textures.Add("mossy.png", 3);
            textures.Add("purplestone.png", 4);
            textures.Add("eagle.png", 5);
            textures.Add("colorstone.png", 6);
            textures.Add("greystone.png", 7);
            textures.Add("pillar.png", 8);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            timeSinceLastUpdate += e.Time;

            // Limit the updates per second
            if (timeSinceLastUpdate >= 1 / targetUpdatesPerSecond)
            {
                // Update player direction and position
                player.Move();
                
                timeSinceLastUpdate = 0;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            fps = 1 / e.Time;

            Render();
        }

        /// <summary>
        /// Handle key presses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                player.TurningState = Enums.TurningState.Left;
            }

            if (e.Key == Key.Right)
            {
                player.TurningState = Enums.TurningState.Right;
            }

            if (e.Key == Key.Up)
            {
                player.MovingState = Enums.MovingState.Forward;
            }

            if (e.Key == Key.Down)
            {
                player.MovingState = Enums.MovingState.Backward;
            }
        }

        /// <summary>
        /// Handle key releases
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                player.TurningState = Enums.TurningState.Idle;
            }

            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                player.MovingState = Enums.MovingState.Idle;
            }
        }
        #endregion

        #region Rendering
        /// <summary>
        /// Render current frame
        /// </summary>
        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.Black);

            //DrawMap(new Point(0, 0), 64);
            DrawMap(miniMapLocation);
            DrawWorld();
            DrawDebugInfo();

            this.SwapBuffers();
        }

        /// <summary>
        /// Draw the 3D view of the world
        /// </summary>
        private void DrawWorld()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Draw solid floor and ceiling
            GLDraw.DrawRectangle(new Rectangle(0, 0, viewPort.Width, viewPort.Height / 2), Color.FromArgb(50, 50, 50));
            GLDraw.DrawRectangle(new Rectangle(0, viewPort.Height / 2, viewPort.Width, viewPort.Height / 2), Color.Gray);

            // All the generated pixels are stored in this buffer before they are rendered
            Pixel[] drawingBuffer = new Pixel[viewPort.Width * viewPort.Height + 1];

            // Render the screen as vertical scanlines
            for (int scanline = 0; scanline < viewPort.Width; scanline++)
            {
                DrawScanline(scanline, drawingBuffer);
            }

            durationRenderCalculation = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            // Draw the buffer to the screen
            GLDraw.DrawPixels(drawingBuffer);
            
            durationRenderDrawing = stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Draw walls that are found for the current vertical scanline
        /// </summary>
        /// <param name="scanline"></param>
        /// <param name="drawingBuffer"></param>
        private void DrawScanline(int scanline, Pixel[] drawingBuffer)
        {
            Angle angle = new Angle(player.Angle.Value + fieldOfView / 2);
            angle.Turn(-angleBetweenRays.Value * (double)scanline);

            // Find the nearest visible wall (no null check, there is always a wall)
            WorldObject wall = FindObject(angle, wallsGrid);

            // Draw the visible objects
            DrawObject(scanline, wall, drawingBuffer);
        }

        /// <summary>
        /// Draw walls that are found for the current vertical scanline
        /// </summary>
        /// <param name="scanline"></param>
        /// <param name="drawingBuffer"></param>
        private void DrawObject(int scanline, WorldObject obj, Pixel[] drawingBuffer)
        {
            // Remove distortion (counter fishbowl effect)
            Angle distortionRemovalAngle = new Angle(fieldOfView / 2);
            distortionRemovalAngle.Turn(-angleBetweenRays.Value * (double)scanline);
            obj.Distance = obj.Distance * Math.Cos(distortionRemovalAngle.ToRadians());

            // Use distance to determine the size of the wall slice
            int wallHeight = (int)Math.Floor(wallSize / obj.Distance * distanceToViewPort);

            // The visible scale of a wall compared to its original size
            double wallScale = (double)wallHeight / wallSize;

            // If a wall slice is larger than the viewport height, we only need the visible section
            // skipPixels indicates how many pixels from top and bottom towards the center can be skipped during rendering
            int skipPixels = wallHeight > viewPort.Height ? (wallHeight - viewPort.Height) / 2 : 0;
            int scanlineStartY = centerOfViewPort.Y - (wallHeight - skipPixels * 2) / 2;

            // Draw wall slice (untextured)
            // GLDraw.DrawLine(new Point(scanline, centerOfViewPort.Y - (wallHeight - skipPixels * 2) / 2), new Point(scanline, centerOfViewPort.Y + (wallHeight - skipPixels * 2) / 2), wall.IntersectsWith == IntersectionAxe.Xaxe ? Color.DarkGray : Color.Gray);

            // Determine start indexes for texture read and viewport write buffers
            int drawingBufferStart = obj.TextureX * (int)wallSize;
            int viewPortBufferIndex = scanline * viewPort.Height;

            // Generate a scanline with a textured wall slice
            for (int y = 0 + skipPixels; y < wallHeight - skipPixels; y++)
            {
                // Determine index of texture pixel while taking zoom into account
                int textureCurrentPixelIndex = (int)(y / wallScale);

                // Current scanline pixel index
                int spxIndex = y - skipPixels;

                // Current viewPortBuffer index
                int drawingBufferIndex = viewPortBufferIndex + spxIndex;

                // Create a pixel in the scanline draw buffer
                Color pixelColor = textures[obj.TextureIndex][drawingBufferStart + textureCurrentPixelIndex].Color;
                if (pixelColor.ToArgb() != Color.Black.ToArgb())
                {
                    drawingBuffer[drawingBufferIndex] = new Pixel();
                    drawingBuffer[drawingBufferIndex].X = scanline;
                    drawingBuffer[drawingBufferIndex].Y = scanlineStartY + spxIndex;
                    drawingBuffer[drawingBufferIndex].Color = pixelColor;

                    // Pixels on dark sides of walls need to be half the color value
                    if (obj.IntersectsWith == IntersectionAxe.Xaxe)
                    {
                        Color color = drawingBuffer[drawingBufferIndex].Color;
                        drawingBuffer[drawingBufferIndex].Color = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2);
                    }

                    // Make walls in the distance darker
                    if (obj.Distance > 250)
                    {
                        double colorDivider = obj.Distance / 250;
                        colorDivider = (colorDivider > 3) ? 3 : colorDivider;

                        Color color = drawingBuffer[drawingBufferIndex].Color;
                        drawingBuffer[drawingBufferIndex].Color = Color.FromArgb((int)(color.R / colorDivider), (int)(color.G / colorDivider), (int)(color.B / colorDivider));
                    }
                }
            }
        }

        /// <summary>
        /// Draw top view of the game world
        /// </summary>
        private void DrawMap(Point drawLocation, int blockWidth = 10)
        {
            // Draw a rectangle for each wall block found in the map
            Color blockColor = Color.White;
            for (int y = 0; y < wallsGrid.Length; y++)
            {
                for (int x = 0; x < wallsGrid[y].Length; x++)
                {
                    if (wallsGrid[y][x] != 0)
                    {
                        int xStart = x * blockWidth + drawLocation.X;
                        int yStart = y * blockWidth + drawLocation.Y;

                        GLDraw.DrawRectangle(new Rectangle(xStart, yStart, blockWidth, blockWidth), Color.DarkRed);
                    }
                }
            }

            // Draw player on the map
            Point mapPlayer = new Point((int)Math.Floor(player.Position.X / wallSize * blockWidth) + drawLocation.X,
                                        (int)Math.Floor(player.Position.Y / wallSize * blockWidth) + drawLocation.Y);
            GLDraw.DrawCircle(mapPlayer, blockWidth, Color.Red);


            // Draw all the rays that are being casted when looking for walls
            Angle angle = new Angle(player.Angle.Value + fieldOfView / 2);

            for (int wallSlice = 0; wallSlice < viewPort.Width; wallSlice++)
            {
                // Determine the distance to the current wall slice
                WorldObject wall = FindObject(angle, wallsGrid);
                Point rayEndPoint = new Point((int)Math.Floor(wall.X / wallSize * blockWidth) + drawLocation.X,
                                              (int)Math.Floor(wall.Y / wallSize * blockWidth) + drawLocation.Y);

                // Draw the ray that was casted to find the wall
                GLDraw.DrawLine(mapPlayer, rayEndPoint, Color.Yellow);

                // Turn towards next wall slice
                angle.Turn(-angleBetweenRays.Value);
            }


            // Draw players field of view on the map (with fixed length)
            angle = new Angle(player.Angle.Value + fieldOfView / 2);
            for (int wallSlice = 0; wallSlice < viewPort.Width; wallSlice++)
            {
                int lineLength = 20;
                int deltaX = (int)(Math.Cos(angle.ToRadians()) * lineLength);
                int deltaY = (int)(Math.Sin(angle.ToRadians()) * lineLength);

                GLDraw.DrawLine(mapPlayer, new Point(mapPlayer.X + deltaX, mapPlayer.Y - deltaY), Color.Green);
                
                angle.Turn(-angleBetweenRays.Value);
            }
        }

        /// <summary>
        /// Draw information used during development
        /// </summary>
        private void DrawDebugInfo()
        {
            string info = String.Format("FPS: {0}\r\n", (int)fps);
            info += String.Format("Angle: {0}\r\n", player.Angle.Value);
            info += String.Format("Calculation duration: {0} ms\r\n", durationRenderCalculation);
            info += String.Format("Drawing duration: {0} ms\r\n", durationRenderDrawing);

            textPrinter.Begin();
            textPrinter.Print(info, debuggerFont, Color.White, new RectangleF(10, 500, 780, 180));
            textPrinter.End();
        }
        #endregion

        #region Calculation
        /// <summary>
        /// Use raycasting to determine the distance to the nearest wall at specified angle
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        /// <param name="objectsGrid">the grid array in which the objects are defined</param>
        /// <returns>location of the first object found</returns>
        private WorldObject FindObject(Angle angle, int[][] objectsGrid)
        {
            // Look for the nearest intersecting wall
            Intersection intersection = FindIntersection(angle, objectsGrid);

            if (intersection == null)
            {
                return null;
            }

            WorldObject obj = new WorldObject(intersection);

            // Calculate TextureX property which determines which part of the texture to map to this object
            Point gridLocation = CoordsToGrid(obj);

            // Set texture for the object
            obj.TextureIndex = objectsGrid[gridLocation.Y][gridLocation.X];

            // Determine on which X-coordinate of the object the intersection was found
            obj.TextureX = obj.IntersectsWith == IntersectionAxe.Xaxe
                            ? (int)(obj.X % wallSize)
                            : (int)(obj.Y % wallSize);

            return obj;
        }

        #region Raycasting methods
        /// <summary>
        /// Use raycasting to find the nearest intersection with an object on the grid at a specified angle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private Intersection FindIntersection(Angle angle, int[][] grid)
        {
            bool foundX = false;
            bool foundY = false;

            // Distance from current point to next intersection for X-axis and Y-axis
            double angleTan = Math.Tan(angle.ToRadians());
            double deltaX = wallSize / angleTan;
            double deltaY = wallSize * angleTan;

            // Correct the deltaX/deltaY values
            if (angle.FacingSouth())
            {
                deltaX = -deltaX;
            }

            if (angle.FacingEast())
            {
                deltaY = -deltaY;
            }

            // Location of horizontal (X) and vertical (Y) intersections
            Intersection intersectionX = null;
            Intersection intersectionY = null;

            // Distance to next horizontal intersection
            if (angle.FacingNorth() || angle.FacingSouth())
            {
                while (intersectionX == null || (!foundX && !IsOutOfBounds(intersectionX, grid)))
                {
                    if (intersectionX == null)
                    {
                        intersectionX = new Intersection();
                        intersectionX.IntersectsWith = IntersectionAxe.Xaxe;
                        intersectionX.Y = Math.Floor(player.Position.Y / wallSize) * wallSize;
                        intersectionX.Y += angle.FacingNorth() ? -0.001 : wallSize;
                        intersectionX.X = player.Position.X + (player.Position.Y - intersectionX.Y) / angleTan;
                    }
                    else
                    {
                        // Target the next horizontal intersection
                        intersectionX.X = intersectionX.X + deltaX;
                        intersectionX.Y += angle.FacingNorth() ? -wallSize : wallSize;
                    }

                    foundX = ContainsItem(intersectionX, grid);
                }
            }

            // Distance to next vertical intersection
            if (angle.FacingWest() || angle.FacingEast())
            {
                while (intersectionY == null || (!foundY && !IsOutOfBounds(intersectionY, grid)))
                {
                    if (intersectionY == null)
                    {
                        intersectionY = new Intersection();
                        intersectionY.IntersectsWith = IntersectionAxe.Yaxe;
                        intersectionY.X = Math.Floor(player.Position.X / wallSize) * wallSize;
                        intersectionY.X += angle.FacingWest() ? -0.001 : wallSize;
                        intersectionY.Y = player.Position.Y + (player.Position.X - intersectionY.X) * angleTan;
                    }
                    else
                    {
                        // Target the next vertical intersection
                        intersectionY.X += angle.FacingWest() ? -wallSize : wallSize;
                        intersectionY.Y = intersectionY.Y + deltaY;
                    }

                    foundY = ContainsItem(intersectionY, grid);
                }
            }

            // Determine which wall is nearest to our starting point
            if (!(IsOutOfBounds(intersectionX, grid) && IsOutOfBounds(intersectionY, grid)))
            {
                Intersection intersection = ChooseNearest(angle, intersectionX, intersectionY);

                // Calculate distance from player to the intersection
                intersection.SetDistance(player.Position, angle);

                return intersection;
            }

            return null;
        }

        /// <summary>
        /// Returns the intersection that is nearest to the player
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="intersectionX">Horizontal intersection point</param>
        /// <param name="intersectionY">Vertical intersection point</param>
        /// <returns></returns>
        private Intersection ChooseNearest(Angle angle, Intersection intersectionX, Intersection intersectionY)
        {
            // If either horizontal or vertical intersection was left empty we can simply return the other one
            if (intersectionX == null)
            {
                return intersectionY;
            }
            else if (intersectionY == null)
            {
                return intersectionX;
            }
            else
            {
                // Determine which intersection is nearest
                double distanceX = Math.Abs(Math.Abs(player.Position.X - intersectionX.X) / Math.Cos(angle.ToRadians()));
                double distanceY = Math.Abs(Math.Abs(player.Position.X - intersectionY.X) / Math.Cos(angle.ToRadians()));

                if (distanceX < distanceY)
                {
                    return intersectionX;
                }
                else
                {
                    return intersectionY;
                }
            }
        }
        #endregion

        #region Grid utility methods
        /// <summary>
        /// Converts world coordinates to grid coordinates (eg. 10,10 = 0,0)
        /// </summary>
        /// <param name="coordinates">world coordinates</param>
        /// <returns>grid coordinates</returns>
        private Point CoordsToGrid(Intersection coordinates)
        {
            return new Point((int)(coordinates.X / wallSize), (int)(coordinates.Y / wallSize));
        }

        /// <summary>
        /// Checks if certain coordinates on the specified grid contain an item
        /// </summary>
        /// <param name="coordinates">coordinates on map (not array indexes)</param>
        /// <param name="grid">the grid to search in</param>
        /// <returns></returns>
        private bool ContainsItem(Intersection coordinates, int[][] grid)
        {
            Point gridCell = CoordsToGrid(coordinates);

            return (gridCell.Y >= 0 && gridCell.Y < grid.Length)
                   && (gridCell.X >= 0 && gridCell.X < grid[0].Length)
                   && grid[gridCell.Y][gridCell.X] != 0;
        }

        /// <summary>
        /// Checks if certain coordinates are outside of the level boundaries
        /// </summary>
        /// <param name="coordinates">coordinates on grid (not array indexes)</param>
        /// <param name="grid">the grid to search in</param>
        /// <returns></returns>
        private bool IsOutOfBounds(Intersection coordinates, int[][] grid)
        {
            Point gridCell = CoordsToGrid(coordinates);

            return gridCell.Y < 0 || gridCell.Y >= grid.Length
                   || gridCell.X < 0 || gridCell.X >= grid[0].Length;
        }
        #endregion
        #endregion

        #region Application entry point
        /// <summary>
        /// Entry point of this application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (Game window = new Game(new Size(1024, 600)))
            {
                // Get the title and category  of this example using reflection.
                window.Title = "Raycaster";
                window.Run();
            }
        }
        #endregion
    }
}
