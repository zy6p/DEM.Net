﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEM.Net.Lib
{
    public static class HeightMapExtensions
    {
        public static HeightMap CenterOnOrigin(this HeightMap heightMap)
        {
            Logger.Info("CenterOnOrigin...");
            var bbox = heightMap.BoundingBox;

            double xOriginOffset = bbox.xMax - (bbox.xMax - bbox.xMin) / 2d;
            double yOriginOffset = bbox.yMax - (bbox.yMax - bbox.yMin) / 2d;
            heightMap.Coordinates = heightMap.Coordinates.Select(pt => new GeoPoint(pt.Latitude - yOriginOffset, pt.Longitude - xOriginOffset,
                pt.Elevation, pt.XIndex, pt.YIndex));

            heightMap.BoundingBox = new BoundingBox(bbox.xMin - xOriginOffset, bbox.xMax - xOriginOffset
                                                    , bbox.yMin - yOriginOffset, bbox.yMax - yOriginOffset);
            return heightMap;
        }

        public static IEnumerable<GeoPoint> CenterOnOrigin(this IEnumerable<GeoPoint> points)
        {
            Logger.Info("CenterOnOrigin...");
            var bbox = points.GetBoundingBox();

            return points.CenterOnOrigin(bbox);
        }
        public static IEnumerable<GeoPoint> CenterOnOrigin(this IEnumerable<GeoPoint> points, BoundingBox bbox)
        {
            Logger.Info("CenterOnOrigin...");
            double xOriginOffset = bbox.xMax - (bbox.xMax - bbox.xMin) / 2d;
            double yOriginOffset = bbox.yMax - (bbox.yMax - bbox.yMin) / 2d;
            points = points.Select(pt => new GeoPoint(pt.Latitude - yOriginOffset, pt.Longitude - xOriginOffset, pt.Elevation, (int)pt.XIndex, (int)pt.YIndex));

            return points;
        }

        /// <summary>
        /// Helper to get an in memory coordinate list
        /// useful to generate normal maps and let the same height map follow the pipeline (reproj, center, ...)
        /// </summary>
        /// <returns></returns>
        public static HeightMap BakeCoordinates(this HeightMap heightMap)
        {
            heightMap.Coordinates = heightMap.Coordinates.ToList();

            return heightMap;
        }

        public static HeightMap ZScale(this HeightMap heightMap, float zFactor = 1f)
        {
            heightMap.Coordinates = heightMap.Coordinates.ZScale(zFactor);

            return heightMap;
        }
        public static HeightMap Scale(this HeightMap heightMap, float x = 1f, float y = 1f, float z = 1f)
        {
            heightMap.Coordinates = heightMap.Coordinates.Scale(x, y, z);
            heightMap.BoundingBox = heightMap.BoundingBox.Scale(x, y); // z does not affect bbox

            return heightMap;
        }
        public static HeightMap FitIntoMillimeters(this HeightMap heightMap, float maxSize)
        {
            float scale = 1f;
            if (heightMap.BoundingBox.Width > heightMap.BoundingBox.Height)
            {
                scale = (float)(maxSize / heightMap.BoundingBox.Width);
            }
            else
            {
                scale = (float)(maxSize / heightMap.BoundingBox.Height);
            }
            heightMap.Coordinates = heightMap.Coordinates.Scale(scale, scale, scale);
            heightMap.BoundingBox = heightMap.Coordinates.GetBoundingBox();
            return heightMap;
        }
        public static IEnumerable<GeoPoint> ZScale(this IEnumerable<GeoPoint> points, float zFactor = 1f)
        {
            return points.Scale(1, 1, zFactor);
        }
        public static IEnumerable<GeoPoint> Scale(this IEnumerable<GeoPoint> points, float x = 1f, float y = 1f, float z = 1f)
        {
            return points.Select(p =>
            {
                var pout = p.Clone();
                pout.Longitude *= x;
                pout.Latitude *= y;
                pout.Elevation *= z;
                return pout;
            });
        }
        public static HeightMap ZTranslate(this HeightMap heightMap, float distance)
        {
            heightMap.Coordinates = heightMap.Coordinates.ZTranslate(distance);

            return heightMap;
        }
        public static IEnumerable<GeoPoint> ZTranslate(this IEnumerable<GeoPoint> points, float distance)
        {
            return points.Select(p =>
            {
                var pout = p.Clone();
                pout.Elevation += distance;
                return pout;
            });
        }

        public static HeightMap Sort(this HeightMap heightMap)
        {
            heightMap.Coordinates = heightMap.Coordinates.Sort();

            return heightMap;
        }
        public static IEnumerable<GeoPoint> Sort(this IEnumerable<GeoPoint> coords)
        {
            coords = coords.OrderByDescending(pt => pt.Latitude)
                           .ThenBy(pt => pt.Longitude);

            return coords;
        }

        public static HeightMap Downsample(this HeightMap heightMap, int step)
        {
            if (step == 0 || step % 2 != 0)
                throw new ArgumentOutOfRangeException("step", "Step must be a factor of 2");

            HeightMap hMap = new HeightMap(heightMap.Width / step, heightMap.Height / step);
            hMap.Maximum = heightMap.Maximum;
            hMap.Mininum = heightMap.Mininum;
            hMap.BoundingBox = heightMap.BoundingBox;
            hMap.Coordinates = DownsampleCoordinates(heightMap.Coordinates.ToList(), heightMap.Width, heightMap.Height, step).ToList();

            return hMap;
        }

        private static IEnumerable<GeoPoint> DownsampleCoordinates(List<GeoPoint> input, int w, int h, int step)
        {
            for (int lat = 0; lat <= h; lat += step)
            {
                for (int lon = 0; lon <= w; lon += step)
                {
                    yield return input[lon + lat * h];
                }
            }
        }

    }
}
