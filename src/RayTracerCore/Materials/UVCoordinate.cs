// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer.Materials
{
    /// <summary>
    /// Represents a 2-dimensional texture coordinate, in UV space.
    /// </summary>
    public struct UVCoordinate
    {
        private Vector2 backingVector;
        /// <summary>
        /// The U value of the coordinate
        /// </summary>
        public float U { get { return backingVector.X; } }
        /// <summary>
        /// The V value of the coordinate
        /// </summary>
        public float V { get { return backingVector.Y; } }

        /// <summary>
        /// Constructs a UV coordinate from the given valeus.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        public UVCoordinate(float u, float v)
        {
            this.backingVector = new Vector2(u, v);
        }
    }
}