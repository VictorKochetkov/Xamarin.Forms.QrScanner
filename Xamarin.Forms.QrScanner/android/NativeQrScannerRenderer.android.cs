#if ANDROID

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
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
using Xamarin.Forms.Controls;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using static Android.Gms.Vision.Detector;
using Point = Android.Graphics.Point;
using RelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(QrScannerView), typeof(NativeQrScannerRenderer))]

namespace Xamarin.Forms.Controls
{
    /// <summary>
    /// Native QR scanner renderer.
    /// https://www.c-sharpcorner.com/article/xamarin-android-qr-code-reader-by-mobile-camera/
    /// </summary>
    public class NativeQrScannerRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<QrScannerView, global::Android.Views.View>, IProcessor
    {
        private CameraSource cameraSource;
        private CameraSourcePreview cameraPreview;
        private SemaphoreSlim locker = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeQrScannerRenderer"/> class.
        /// </summary>
        /// <param name="context">Android context.</param>
        public NativeQrScannerRenderer(Context context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        public void ReceiveDetections(Detections detections)
        {
            SparseArray qrcodes = detections.DetectedItems;

            if (qrcodes.Size() != 0)
            {
                Post(() =>
                {
                    string result = ((Barcode)qrcodes.ValueAt(0)).RawValue;

                    if (Element?.IsScanning == true)
                        Element?.Result?.Execute(result);
                });
            }
        }

        /// <inheritdoc/>
        public void Release()
        {
        }

        /// <inheritdoc/>
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<QrScannerView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var detector = new BarcodeDetector.Builder(Context)
                    .SetBarcodeFormats(BarcodeFormat.QrCode)
                    .Build();

                detector.SetProcessor(this);

                var display = (Context as Activity).WindowManager.DefaultDisplay;
                var size = new global::Android.Graphics.Point();
                display.GetSize(size);

                int width = size.X;
                int height = size.Y;

                cameraSource = new CameraSource.Builder(Context, detector)
                    .SetRequestedPreviewSize(width, height)
                    .SetFacing(CameraFacing.Back)
                    .SetAutoFocusEnabled(true)
                    .SetRequestedFps(30.0f)
                    .Build();

                cameraPreview = new CameraSourcePreview(cameraSource, Context, null);
                cameraPreview.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

                SetNativeControl(cameraPreview);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            cameraPreview?.Stop();
            cameraSource?.Release();

            base.Dispose(disposing);
        }
    }
}

#endif