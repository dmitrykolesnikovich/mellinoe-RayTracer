// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer
{
    /// <summary>
    /// Represents a color, with components Red, Green, Blue, and Alpha between 0 (min) and 1 (max).
    /// </summary>
    public struct Color
    {
        /// <summary> The color's red component, between 0.0 and 1.0 </summary>
        public float R { get { return backingVector.X; } }

        /// <summary> The color's green component, between 0.0 and 1.0 </summary>
        public float G { get { return backingVector.Y; } }

        /// <summary> The color's blue component, between 0.0 and 1.0 </summary>
        public float B { get { return backingVector.Z; } }

        /// <summary> The color's alpha component, between 0.0 and 1.0 </summary>
        public float A { get { return backingVector.W; } }

        private readonly Vector4 backingVector;

        /// <summary>
        /// Constructs a color from the given component values.
        /// </summary>
        /// <param name="r">The color's red value</param>
        /// <param name="g">The color's green value</param>
        /// <param name="b">The color's blue value</param>
        /// <param name="a">The color's alpha value</param>
        public Color(float r, float g, float b, float a)
        {
            this.backingVector = new Vector4(r, g, b, a);
        }

        private Color(Vector4 vec)
        {
            this.backingVector = vec;
        }

        public static readonly Color Red = new Color(1, 0, 0, 1);
        public static readonly Color Green = new Color(0, 1, 0, 1);
        public static readonly Color Blue = new Color(0, 0, 1, 1);
        public static readonly Color Purple = new Color(1, 0, 1, 1);
        public static readonly Color White = new Color(1, 1, 1, 1);
        public static readonly Color Black = new Color(0, 0, 0, 1);
        public static readonly Color Yellow = new Color(1, 1, 0, 1);
        public static readonly Color Grey = new Color(.6f, .6f, .6f, 1);
        public static readonly Color DarkGrey = new Color(.23f, .23f, .18f, 1);
        public static readonly Color Clear = new Color(1, 1, 1, 0);
        public static readonly Color Sky = new Color(102f / 255f, 152f / 255f, 1f, 1f);
        public static readonly Color Zero = new Color(0f, 0f, 0f, 0f);
        public static readonly Color Silver = new Color(0.2f, 0.2f, 0.28f, 1f);

        public override string ToString()
        {
            return string.Format("Color: [{0}, {1}, {2}, {3}]", this.R, this.G, this.B, this.A);
        }

        /// <summary>
        /// Returns a new color whose components are the average of the components of first and second
        /// </summary>
        public static Color Average(Color first, Color second)
        {
            return new Color((first.backingVector + second.backingVector) * .5f);
        }

        /// <summary>
        /// Linearly interpolates from one color to another based on t.
        /// </summary>
        /// <param name="from">The first color value</param>
        /// <param name="to">The second color value</param>
        /// <param name="t">The weight value. At t = 0, "from" is returned, at t = 1, "to" is returned.</param>
        /// <returns></returns>
        public static Color Lerp(Color from, Color to, float t)
        {
            t = Util.Clamp(t, 0f, 1f);

            return from * (1 - t) + to * t;
        }

        public static Color operator *(Color color, float factor)
        {
            return new Color(color.backingVector * factor);
        }
        public static Color operator *(float factor, Color color)
        {
            return new Color(color.backingVector * factor);
        }
        public static Color operator *(Color left, Color right)
        {
            return new Color(left.backingVector * right.backingVector);
        }

        /// <summary>
        /// Returns this color with the component values clamped from 0 to 1.
        /// </summary>
        public Color Limited
        {
            get
            {
                var r = Util.Clamp(R, 0, 1);
                var g = Util.Clamp(G, 0, 1);
                var b = Util.Clamp(B, 0, 1);
                var a = Util.Clamp(A, 0, 1);
                return new Color(r, g, b, a);
            }
        }

        public static Color operator +(Color left, Color right)
        {
            return new Color(left.backingVector + right.backingVector);
        }

        public static Color operator -(Color left, Color right)
        {
            return new Color(left.backingVector - right.backingVector);
        }

        /// <summary>
        /// Returns a BGRA32 integer representation of the color
        /// </summary>
        /// <param name="color">The color object to convert</param>
        /// <returns>An integer value whose 4 bytes each represent a single BGRA component value from 0-255</returns>
        public static int ToBGRA32(Color color)
        {
            byte r = (byte)(255 * color.R);
            byte g = (byte)(255 * color.G);
            byte b = (byte)(255 * color.B);
            byte a = (byte)(255 * color.A);

            return (r << 16) | (g << 8) | (b << 0) | (a << 24);
        }
    }

    public struct Color32Argb
    {
        public byte A, R, G, B;

        public Color32Argb(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            B = b;
            G = g;
        }

        public Color32Argb(Color color)
        {
            color = color.Limited;
            A = (byte)(255 * color.A);
            R = (byte)(255 * color.R);
            G = (byte)(255 * color.G);
            B = (byte)(255 * color.B);
        }
    }
}
