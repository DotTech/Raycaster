using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Raycaster.Entities;

namespace Raycaster.Utils
{
    /// <summary>
    /// Helper class with basic shape drawing methods for OpenGL
    /// </summary>
    public static class GLDraw
    {
        /// <summary>
        /// Draws a single pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void DrawPixel(int x, int y, Color color)
        {
            GL.Begin(BeginMode.Points);

            GL.Color3(color);

            GL.Vertex2(x, y);

            GL.End();
        }

        public static void DrawPixels(IEnumerable<Pixel> pixels)
        {
            GL.Begin(BeginMode.Points);
            Color current = Color.Empty;
            
            foreach (Pixel p in pixels)
            {
                if (p != null)
                {
                    // Set the color for this pixel
                    if (p.Color != current)
                    {
                        current = p.Color;
                        GL.Color3(current);
                    }

                    // Set coordinates for this pixel
                    GL.Vertex2(p.X, p.Y);
                }
            }

            GL.End();
        }

        public static int[] DrawToMemory(IEnumerable<Pixel> pixels)
        {
            int[] result = new int[pixels.Count()];
            int index = 0;

            foreach (Pixel p in pixels)
            {
                result[index] = (p != null) ? p.Color.ToArgb() : Color.Black.ToArgb();
                index++;
            }

            return result;
            
            /*
            int[] data = GLDraw.DrawToMemory(buffer);

            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(viewPort.Width, viewPort.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            bmp.Save("d:\\Data\\Projects\\Raycaster\\test.bmp");
            */

            /*
            var a = GLDraw.DrawToMemory(buffer);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            //var b = new System.Drawing.Imaging.BitmapData();
            //var b = new Bitmap(

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, viewPort.Width, viewPort.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Int, a);
            */
            /*
            int[] data = GLDraw.DrawToMemory(buffer);

            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(viewPort.Width, viewPort.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            bmp.Save("d:\\Data\\Projects\\Raycaster\\test.bmp");
            */

            /*
            var a = GLDraw.DrawToMemory(buffer);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            //var b = new System.Drawing.Imaging.BitmapData();
            //var b = new Bitmap(

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, viewPort.Width, viewPort.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Int, a);
            */
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public static void DrawRectangle(Rectangle rectangle, Color color)
        {
            GL.Begin(BeginMode.Quads);

            GL.Color3(color);

            GL.Vertex2(rectangle.X, rectangle.Y);
            GL.Vertex2(rectangle.X + rectangle.Width, rectangle.Y);
            GL.Vertex2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            GL.Vertex2(rectangle.X, rectangle.Y + rectangle.Height);

            GL.End();
        }

        /// <summary>
        /// Draws a filled circle
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public static void DrawCircle(Point location, int size, Color color)
        {
            GL.Begin(BeginMode.TriangleFan);

            GL.Color3(color);

            GL.Vertex2(location.X, location.Y);

            for (int angle = 0; angle <= 360; angle++)
            {
                double radianAngle = ((double)angle).ToRadians();
                GL.Vertex2(location.X + Math.Sin(radianAngle) * size / 2, location.Y + Math.Cos(radianAngle) * size / 2);
            }

            GL.End();
        }

        /// <summary>
        /// Draws a line from point start to point end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        public static void DrawLine(Point start, Point end, Color color)
        {
            GL.Begin(BeginMode.Lines);

            GL.Color3(color);
            GL.Vertex2(start.X, start.Y);
            GL.Vertex2(end.X, end.Y);

            GL.End();
        }
    }
}
