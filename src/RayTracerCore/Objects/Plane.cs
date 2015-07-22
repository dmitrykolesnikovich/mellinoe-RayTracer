// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using RayTracer.Materials;
using System;
#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer.Objects
{
    /// <summary>
    /// A plane is, conceptually, a sheet that extends infinitely in all directions.
    /// </summary>
    public class Plane : DrawableSceneObject
    {
        private Vector3 normalDirection;
        protected Vector3 uDirection;
        protected Vector3 vDirection;
        private float cellWidth;

        /// <summary>
        /// Constructs a plane with the properties provided
        /// </summary>
        /// <param name="position">The position of the plane's center</param>
        /// <param name="material">The plane's material</param>
        /// <param name="normalDirection">The normal direction of the plane</param>
        /// <param name="cellWidth">The width of a cell in the plane, used for texture coordinate mapping.</param>
        public Plane(Vector3 position, Material material, Vector3 normalDirection, float cellWidth)
            : base(position, material)
        {
            this.normalDirection = normalDirection.Normalized();
            if (normalDirection == Util.ForwardVector)
            {
                this.uDirection = -Util.RightVector;
            }
            else if (normalDirection == -Util.ForwardVector)
            {
                this.uDirection = Util.RightVector;
            }
            else
            {
                this.uDirection = Vector3.Normalize(Vector3.Cross(normalDirection, Util.ForwardVector));
            }

            this.vDirection = Vector3.Normalize(-Vector3.Cross(normalDirection, uDirection));
            this.cellWidth = cellWidth;
        }

        public override bool TryCalculateIntersection(Ray ray, out Intersection intersection)
        {
            Vector3 vecDirection = ray.Direction;
            Vector3 rayToPlaneDirection = ray.Origin - this.Position;

            float D = Vector3.Dot(this.normalDirection, vecDirection);
            float N = -Vector3.Dot(this.normalDirection, rayToPlaneDirection);

            if (Math.Abs(D) <= .0005f)
            {
                intersection = new Intersection();
                return false;
            }

            float sI = N / D;
            if (sI < 0 || sI > ray.Distance) // Behind or out of range
            {
                intersection = new Intersection();
                return false;
            }

            var intersectionPoint = ray.Origin + (sI * vecDirection);
            var uv = this.GetUVCoordinate(intersectionPoint);

            var color = Material.GetDiffuseColorAtCoordinates(uv);

            intersection = new Intersection(intersectionPoint, this.normalDirection, ray.Direction, this, color, Vector3.Distance(ray.Origin, intersectionPoint));
            return true;
        }

        public override UVCoordinate GetUVCoordinate(Vector3 position)
        {
            var uvPosition = this.Position + position;

            var uMag = Vector3.Dot(uvPosition, uDirection);
            var u = (uMag * uDirection).Length();
            if (uMag < 0)
            {
                u += cellWidth / 2f;
            }
            u = (u % cellWidth) / cellWidth;

            var vMag = Vector3.Dot(uvPosition, vDirection);
            var v = (vMag * vDirection).Length();
            if (vMag < 0)
            {
                v += cellWidth / 2f;
            }
            v = (v % cellWidth) / cellWidth;

            return new UVCoordinate(u, v);
        }
    }
}
