// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RayTracer.Materials;
#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer.Objects
{
    /// <summary>
    /// A two-dimensional circular plane object. Limited in radius rather than extending infinitely.
    /// </summary>
    public class Disc : Plane
    {
        private float radius;

        public Disc(Vector3 centerPosition, Material material, Vector3 normalDirection, float radius, float cellWidth)
            : base(centerPosition, material, normalDirection, cellWidth)
        {
            this.radius = radius;
        }

        public override bool TryCalculateIntersection(Ray ray, out Intersection intersection)
        {
            if (base.TryCalculateIntersection(ray, out intersection) && WithinArea(intersection.Point))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool WithinArea(Vector3 location)
        {
            var distanceFromCenter = (this.Position - location).Magnitude();
            return distanceFromCenter <= radius;
        }
    }
}
