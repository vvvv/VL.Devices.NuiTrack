﻿using nuitrack;
using System;
using System.Reactive.Linq;
using VL.Lib.Basics.Imaging;
using VL.Lib.Collections;
using Stride.Core.Mathematics;

namespace VL.Devices.Nuitrack
{
    // Keep all implementing classes internal and just expose them publicly through interface
    public static class FrameExtensions
    {
        public static IObservable<ColorFrameEventArgs> ColorFrameArrived(ColorSensor sensor)
        {
            if (sensor is null)
                return Observable.Empty<ColorFrameEventArgs>();

            return Observable.FromEvent<ColorSensor.OnUpdate, ColorFrameEventArgs>(handler =>
            {
                ColorSensor.OnUpdate cfaHandler = (x) =>
                {
                    handler(new ColorFrameEventArgs(x));
                };

                return cfaHandler;
            },
                    cfaHandler => sensor.OnUpdateEvent += cfaHandler,
                    cfaHandler => sensor.OnUpdateEvent -= cfaHandler);
        }

        public static IObservable<DepthFrameEventArgs> DepthFrameArrived(DepthSensor sensor)
        {
            if (sensor is null)
                return Observable.Empty<DepthFrameEventArgs>();

            return Observable.FromEvent<DepthSensor.OnUpdate, DepthFrameEventArgs>(handler =>
            {
                DepthSensor.OnUpdate cfaHandler = (x) =>
                {
                    handler(new DepthFrameEventArgs(x));
                };

                return cfaHandler;
            },
                    cfaHandler => sensor.OnUpdateEvent += cfaHandler,
                    cfaHandler => sensor.OnUpdateEvent -= cfaHandler);
        }

        public static IObservable<UserFrameEventArgs> UserFrameArrived(UserTracker userTracker)
        {
            if (userTracker is null)
                return Observable.Empty<UserFrameEventArgs>();

            return Observable.FromEvent<UserTracker.OnUpdate, UserFrameEventArgs>(handler =>
            {
                UserTracker.OnUpdate cfaHandler = (x) =>
                {
                    handler(new UserFrameEventArgs(x));
                };

                return cfaHandler;
            },
                    cfaHandler => userTracker.OnUpdateEvent += cfaHandler,
                    cfaHandler => userTracker.OnUpdateEvent -= cfaHandler);
        }

        public static IObservable<SkeletonDataEventArgs> SkeletonDataArrived(SkeletonTracker skeletonTracker)
        {
            if (skeletonTracker is null)
                return Observable.Empty<SkeletonDataEventArgs>();

            return Observable.FromEvent<SkeletonTracker.OnSkeletonUpdate, SkeletonDataEventArgs>(handler =>
            {
                SkeletonTracker.OnSkeletonUpdate cfaHandler = (x) =>
                {
                    handler(new SkeletonDataEventArgs(x));
                };

                return cfaHandler;
            },
                    cfaHandler => skeletonTracker.OnSkeletonUpdateEvent += cfaHandler,
                    cfaHandler => skeletonTracker.OnSkeletonUpdateEvent -= cfaHandler);
        }

        public static IObservable<HandTrackerDataEventArgs> HandDataArrived(HandTracker handTracker)
        {
            if (handTracker is null)
                return Observable.Empty<HandTrackerDataEventArgs>();

            return Observable.FromEvent<HandTracker.OnUpdate, HandTrackerDataEventArgs>(handler =>
            {
                HandTracker.OnUpdate cfaHandler = (x) =>
                {
                    handler(new HandTrackerDataEventArgs(x));
                };

                return cfaHandler;
            },
                    cfaHandler => handTracker.OnUpdateEvent += cfaHandler,
                    cfaHandler => handTracker.OnUpdateEvent -= cfaHandler);
        }

        public static ColorImage ToColorImage(ColorFrame frame)
        {
            return new ColorImage(frame);
        }

        public static DepthImage ToDepthImage(DepthFrame frame)
        {
            return new DepthImage(frame);
        }

        public static IImage ToUserImage(UserFrame frame)
        {
            ImageInfo info = new ImageInfo(frame.Cols, frame.Rows, PixelFormat.R16);
            ArrayImage<byte> image = new ArrayImage<byte>(frame.Data, info, true);
            return image;
        }
    }
}
