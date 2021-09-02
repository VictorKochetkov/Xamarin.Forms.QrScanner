#if ANDROID

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using static Android.Gms.Vision.Detector;
using Point = Android.Graphics.Point;
using RelativeLayout = Android.Widget.RelativeLayout;

namespace Xamarin.Forms.Controls
{
    /// <summary>
    /// Custom camera source preview with fixed scale and aspect ratio
    /// https://github.com/xamarin/GooglePlayServicesComponents/tree/main/samples/com.google.android.gms/play-services-vision/VisionSample
    /// </summary>
    public class CameraSourcePreview : ViewGroup
    {
        private SurfaceView view;
        private CameraSource cameraSource;
        private bool started = false;
        private bool surfaceAvailable = false;

        /// <summary>
        /// Is surface available.
        /// </summary>
        protected bool SurfaceAvailable
        {
            get => surfaceAvailable;
            set
            {
                surfaceAvailable = value;

                if (surfaceAvailable)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraSourcePreview"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attributes.</param>
        public CameraSourcePreview(CameraSource cameraSource, Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.cameraSource = cameraSource;
            view = new SurfaceView(context);
            view.Holder.AddCallback(new SurfaceCallback(this));
            AddView(view);
        }

        /// <summary>
        /// Stop camera preview.
        /// </summary>
        public void Stop()
        {
            cameraSource.Stop();
            started = false;
        }

        /// <summary>
        /// Start camera preview.
        /// </summary>
        private void Start()
        {
            if (SurfaceAvailable && !started)
            {
                cameraSource.Start(view.Holder);
                started = true;
            }
        }

        /// <inheritdoc/>
        private class SurfaceCallback : Java.Lang.Object, ISurfaceHolderCallback
        {
            private readonly CameraSourcePreview parent;

            public SurfaceCallback(CameraSourcePreview parent)
            {
                this.parent = parent;
            }

            /// <inheritdoc/>
            public void SurfaceCreated(ISurfaceHolder surface)
            {
                parent.SurfaceAvailable = true;
            }

            /// <inheritdoc/>
            public void SurfaceDestroyed(ISurfaceHolder surface)
            {
                parent.SurfaceAvailable = false;
            }

            /// <inheritdoc/>
            public void SurfaceChanged(ISurfaceHolder holder, global::Android.Graphics.Format format, int width, int height)
            {
            }
        }

        // https://stackoverflow.com/questions/51959266/linearlayout-camerasource-dont-fill-screen

        /// <inheritdoc/>
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            int previewWidth = 320;
            int previewHeight = 240;

            if (cameraSource != null)
            {
                var size = cameraSource.PreviewSize;
                if (size != null)
                {
                    previewWidth = size.Width;
                    previewHeight = size.Height;
                }
            }

            // Swap width and height sizes when in portrait, since it will be rotated 90 degrees
            if (IsPortraitMode())
            {
                int tmp = previewWidth;
                previewWidth = previewHeight;
                previewHeight = tmp;
            }

            int viewWidth = right - left;
            int viewHeight = bottom - top;

            int childWidth;
            int childHeight;
            int childXOffset = 0;
            int childYOffset = 0;
            float widthRatio = (float)viewWidth / (float)previewWidth;
            float heightRatio = (float)viewHeight / (float)previewHeight;

            // To fill the view with the camera preview, while also preserving the correct aspect ratio,
            // it is usually necessary to slightly oversize the child and to crop off portions along one
            // of the dimensions.  We scale up based on the dimension requiring the most correction, and
            // compute a crop offset for the other dimension.
            if (widthRatio > heightRatio)
            {
                childWidth = viewWidth;
                childHeight = (int)((float)previewHeight * widthRatio);
                childYOffset = (childHeight - viewHeight) / 2;
            }
            else
            {
                childWidth = (int)((float)previewWidth * heightRatio);
                childHeight = viewHeight;
                childXOffset = (childWidth - viewWidth) / 2;
            }

            for (int i = 0; i < ChildCount; ++i)
            {
                // One dimension will be cropped.  We shift child over or up by this offset and adjust
                // the size to maintain the proper aspect ratio.
                GetChildAt(i).Layout(
                        -1 * childXOffset, -1 * childYOffset,
                        childWidth - childXOffset, childHeight - childYOffset);
            }
        }

        private bool IsPortraitMode()
        {
            var orientation = Context.Resources.Configuration.Orientation;

            if (orientation == global::Android.Content.Res.Orientation.Landscape)
            {
                return false;
            }

            if (orientation == global::Android.Content.Res.Orientation.Portrait)
            {
                return true;
            }

            return false;
        }
    }
}

#endif