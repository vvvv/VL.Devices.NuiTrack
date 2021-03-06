﻿using System;
using System.Runtime.CompilerServices;
using nuitrack;
using VL.Lib.Collections;

namespace VL.Devices.Nuitrack
{
    public static class PointCloud
    {
        public static SpreadBuilder<ColoredPoint> AddPoints(
            SpreadBuilder<ColoredPoint> builder, 
            DepthSensor sensor, 
            ColorFrame colorFrame, 
            DepthFrame depthFrame, 
            int minZ = 100,
            int maxZ = ushort.MaxValue, 
            int decimation = 1)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (sensor is null)
                throw new ArgumentNullException(nameof(sensor));
            if (colorFrame is null)
                throw new ArgumentNullException(nameof(colorFrame));
            if (depthFrame is null)
                throw new ArgumentNullException(nameof(depthFrame));
            if (colorFrame.Cols < depthFrame.Cols || colorFrame.Rows < depthFrame.Rows)
                throw new ArgumentException("The color image must have at least the same size as the depth image.");

            var width = depthFrame.Cols;
            var height = depthFrame.Rows;
            var strideX = colorFrame.Cols / depthFrame.Cols;
            var strideY = colorFrame.Rows / depthFrame.Rows;
            var step = Math.Max(1, decimation);

            for (var y = 0; y < height; y += step)
            {
                for (var x = 0; x < width; x += step)
                {
                    var z = depthFrame[y, x];
                    if (z >= minZ && z <= maxZ)
                    {
                        var c = colorFrame[y * strideY, x * strideX];
                        var p = sensor.ConvertProjToRealCoords(x, y, z);
                        builder.Add(new ColoredPoint(AsVector3(ref p), ToColor(in c)));
                    }
                }
            }

            return builder;
        }

        static Stride.Core.Mathematics.Vector3 AsVector3(ref Vector3 vector)
        {
            return Unsafe.As<Vector3, Stride.Core.Mathematics.Vector3>(ref vector);
        }

        static Stride.Core.Mathematics.Color ToColor(in Color3 color)
        {
            return new Stride.Core.Mathematics.Color(color.Red, color.Green, color.Blue);
        }
    }
}
