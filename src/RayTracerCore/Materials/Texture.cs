// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;

namespace RayTracer.Materials
{
    /// <summary>
    /// Represents a 2D Texture used for materials with textures
    /// </summary>
    public class Texture
    {
        private Color[,] pixels;

        /// <summary>
        /// Constructs a texture object from a System.Drawing.Bitmap object
        /// </summary>
        /// <param name="bitmap">The bitmap to use</param>
        public Texture(Bitmap bitmap)
        {
            pixels = new Color[bitmap.Width, bitmap.Height];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    pixels[x, y] = bitmap.GetPixel(x, y);
                }
            }
        }

        /// <summary>
        /// Constructs a Texture from the bitmap image at the given file path.
        /// </summary>
        /// <param name="fileName"></param>
        public Texture(string fileName) : this(new Bitmap(fileName)) { }

        /// <summary>
        /// Returns the color of the texture at the given x, y coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color this[int x, int y]
        {
            get
            {
                return pixels[x, y];
            }
        }

        public Color GetColorAtUVCoordinate(float u, float v)
        {
            return this[(int)(u * this.pixels.GetLength(0)), (int)(v * this.pixels.GetLength(1))];
        }
    }
}
