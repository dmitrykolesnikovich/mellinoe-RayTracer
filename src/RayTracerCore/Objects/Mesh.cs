// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RayTracer.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer.Objects
{
    /// <summary>
    /// Represents a renderable poly-mesh object containing vertices and UV-mapping information.

    /// </summary>
    public class Mesh : DrawableSceneObject
    {
        private Polygon[] polygons;

        public Mesh(Vector3 position, Material material, IEnumerable<Polygon> polygons)
            : base(position, material)
        {
            this.polygons = polygons.ToArray();
        }

        public override bool TryCalculateIntersection(Ray ray, out Intersection intersection)
        {
            for (int g = 0; g < polygons.Length; g++)
            {
                if (IntersectsPolygon(polygons[g], ray, out intersection))
                {
                    return true;
                }
            }

            intersection = new Intersection();
            return false;
        }

        private bool IntersectsPolygon(Polygon polygon, Ray ray, out Intersection intersection)
        {
            intersection = new Intersection();

            Vector3 vecDirection = ray.Direction;
            Vector3 rayToPlaneDirection = ray.Origin - this.Position;

            float D = Vector3.Dot(polygon.NormalDirection, vecDirection);
            float N = -Vector3.Dot(polygon.NormalDirection, rayToPlaneDirection);

            if (Math.Abs(D) <= .0005f)
            {
                return false;
            }

            float sI = N / D;
            if (sI < 0 || sI > ray.Distance) // Behind or out of range
            {
                return false;
            }

            var intersectionPoint = ray.Origin + (new Vector3(sI) * vecDirection);
            var uv = this.GetUVCoordinate(intersectionPoint);

            return true;
        }

        public override Materials.UVCoordinate GetUVCoordinate(Vector3 position)
        {
            // no clue
            return new Materials.UVCoordinate(0, 0);
        }
    }
}
