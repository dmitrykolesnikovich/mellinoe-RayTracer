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
    /// Represents a ray primitive. Used as a basis for intersection calculations.
    /// </summary>
    public struct Ray
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public readonly float Distance;
        public Ray(Vector3 start, Vector3 direction, float distance)
        {
            this.Origin = start;
            this.Direction = direction.Normalized();
            this.Distance = distance;
        }
        public Ray(Vector3 start, Vector3 direction) : this(start, direction, float.PositiveInfinity) { }
    }
}
