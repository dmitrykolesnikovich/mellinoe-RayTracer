// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using ImageProcessorCore;
using System.IO;

namespace RayTracer.Materials
{

    /// <summary>
    /// Represents a 2D Texture used for materials with textures
    /// </summary>
    public class Texture
    {
        private readonly Color[] _pixels;
        private readonly int _width;
        private readonly int _height;

        /// <summary>
        /// Constructs a texture object from a System.Drawing.Bitmap object
        /// </summary>
        /// <param name="bitmap">The bitmap to use</param>
        public Texture(Image image)
        {
            _width = image.Width;
            _height = image.Height;

            _pixels = new Color[_width * _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    int pixelIndex = x + (y * _height);
                    int floatIndex = pixelIndex * 4;
                    Color c = new Color(
                        image.Pixels[floatIndex],
                        image.Pixels[floatIndex + 1],
                        image.Pixels[floatIndex + 2],
                        image.Pixels[floatIndex + 3]);
                    _pixels[pixelIndex] = c;
                }
            }
        }

        /// <summary>
        /// Constructs a Texture from the bitmap image at the given file path.
        /// </summary>
        /// <param name="fileName"></param>
        public Texture(string fileName) : this(LoadImage(fileName)) { }

        private static Image LoadImage(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            {
                return new Image(fs);
            }
        }

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
                return _pixels[x + (y * _width)];
            }
        }

        public Color GetColorAtUVCoordinate(float u, float v)
        {
            int x = (int)(u * _width);
            int y = (int)(v * _height);
            return this[x, y];
        }
    }
}
