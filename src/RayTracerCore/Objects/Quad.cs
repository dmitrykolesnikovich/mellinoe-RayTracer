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
    /// A quad is a plane that is limited in all four directions rather than extending infinitely.
    /// </summary>
    public class Quad : Plane
    {
        private float width, height;

        public Quad(Vector3 centerPosition, Material material, Vector3 normalDirection, float width, float height, float cellWidth)
            : base(centerPosition, material, normalDirection, cellWidth)
        {
            this.width = width;
            this.height = height;
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
            var differenceFromCenter = this.Position - location;
            var uLength = Util.Projection(differenceFromCenter, uDirection);
            var vLength = Util.Projection(differenceFromCenter, vDirection);

            return uLength.Magnitude() <= width / 2f && vLength.Magnitude() <= height / 2f;
        }
    }
}
