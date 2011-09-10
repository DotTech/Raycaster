using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Raycaster.Entities
{
    public class Textures
    {
        public Dictionary<int, Pixel[]> TextureBuffer { get; set; }

        public Pixel[] this[int index]
        {
            get
            {
                return TextureBuffer[index];
            }
        }

        public Textures()
        {
            TextureBuffer = new Dictionary<int, Pixel[]>();
        }

        public void Add(string filename, int index, int width = 64, int height = 64)
        {
            Bitmap textureBmp = new Bitmap("Resources\\textures\\" + filename);
            Pixel[] texture = new Pixel[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture[x * width + y] = new Pixel();
                    texture[x * width + y].Color = textureBmp.GetPixel(x, y);
                    texture[x * width + y].X = x;
                    texture[x * width + y].Y = y;
                }
            }

            TextureBuffer.Add(index, texture);
        }
    }
}
