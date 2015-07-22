// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
#if SCALAR
using UnoptimizedVectors;
#else
using System.Numerics;
#endif

namespace RayTracer.Objects
{
    public class Polygon
    {
        private Vector3[] points;

        public Vector3 NormalDirection { get; private set; }

        public Polygon(Vector3 position, IEnumerable<Vector3> points)
        {
            var pointsArray = points.ToArray();
            this.points = pointsArray;

            var rightDirection = pointsArray[0] - pointsArray[1];
            NormalDirection = Vector3.Cross(Util.UpVector, rightDirection);
        }
    }
}