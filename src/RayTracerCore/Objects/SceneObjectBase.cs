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
    /// The base class for all scene objects, which must contain a world-space position in the scene.
    /// </summary>
    public abstract class SceneObjectBase
    {
        /// <summary>
        /// The world-space position of the scene object
        /// </summary>
        public Vector3 Position { get; set; }
        public SceneObjectBase(Vector3 position)
        {
            this.Position = position;
        }
    }
}
