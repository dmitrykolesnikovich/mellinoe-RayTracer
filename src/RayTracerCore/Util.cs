// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer
{
    /// <summary>
    /// Contains various mathematic helper methods for scalars and vectors
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Clamps the given value between min and max
        /// </summary>
        public static float Clamp(float value, float min, float max)
        {
            return value > max ? max : value < min ? min : value;
        }

        /// <summary>
        /// Linearly interpolates between two values, based on t
        /// </summary>
        public static float Lerp(float from, float to, float t)
        {
            return (from * (1 - t)) + (to * t);
        }

        /// <summary>
        /// Returns the maximum of the given set of values
        /// </summary>
        public static float Max(params float[] values)
        {
            float max = values[0];
            for (int g = 1; g < values.Length; g++)
            {
                if (values[g] > max)
                {
                    max = values[g];
                }
            }
            return max;
        }

        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        internal static float DegreesToRadians(float angleInDegrees)
        {
            var radians = (float)((angleInDegrees / 360f) * 2 * Math.PI);
            return radians;
        }

        public static readonly Vector3 RightVector = new Vector3(1, 0, 0);
        public static readonly Vector3 UpVector = new Vector3(0, 1, 0);
        public static readonly Vector3 ForwardVector = new Vector3(0, 0, 1);

        public static float Magnitude(this Vector3 v)
        {
            return v.Length();
        }

        public static Vector3 Normalized(this Vector3 v)
        {
            float lengthSquared = v.LengthSquared();
            if (lengthSquared != 1)
            {
                return Vector3.Normalize(v);
            }
            else
            {
                return v;
            }
        }

        public static Vector3 Projection(Vector3 projectedVector, Vector3 directionVector)
        {
            var mag = Vector3.Dot(projectedVector, directionVector.Normalized());
            return directionVector * new Vector3(mag);
        }
    }
}
