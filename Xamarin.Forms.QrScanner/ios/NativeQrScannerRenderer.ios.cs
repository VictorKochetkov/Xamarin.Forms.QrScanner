#if IOS

using System;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;
using CoreFoundation;
using Foundation;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(QrScannerView), typeof(NativeQrScannerRenderer))]

namespace Xamarin.Forms.Controls
{
    /// <summary>
    /// Native implementation of QR scanner.
    /// http://www.renaudpradenc.com/?p=453
    /// https://www.hackingwithswift.com/example-code/media/how-to-scan-a-qr-code
    /// </summary>
    public class NativeQrScannerRenderer : ViewRenderer<QrScannerView, UIView>, IAVCaptureMetadataOutputObjectsDelegate
    {
        private AVCaptureSession session;
        private AVCaptureVideoPreviewLayer preview;

        /// <summary>
        /// Implementation of <see cref="IAVCaptureMetadataOutputObjectsDelegate"/>.
        /// </summary>
        /// <param name="captureOutput">Output.</param>
        /// <param name="metadataObjects">Scan results.</param>
        /// <param name="connection">Capture connection.</param>
        [Export("captureOutput:didOutputMetadataObjects:fromConnection:")]
        public void DidOutputMetadataObjects(AVCaptureMetadataOutput captureOutput, AVMetadataObject[] metadataObjects, AVCaptureConnection connection)
        {
            foreach (var m in metadataObjects)
            {
                if (m is AVMetadataMachineReadableCodeObject result && result.Type == AVMetadataObjectType.QRCode)
                {
                    if (Element?.IsScanning == true)
                        Element?.Result?.Execute(result.StringValue);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(QrScannerView.Width):
                case nameof(QrScannerView.Height):
                    UpdateViewSize();
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnElementChanged(ElementChangedEventArgs<QrScannerView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                if (DeviceInfo.DeviceType == DeviceType.Virtual)
                {
                    Console.WriteLine("Can't start QR scanner on simulator");
                    return;
                }

                var permission = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

                if (permission != AVAuthorizationStatus.Authorized)
                {
                    throw new SecurityException("Camera permission not granted");
                }

                var view = new UIView();

                var deviceDiscoverySession = AVCaptureDeviceDiscoverySession.Create(new AVCaptureDeviceType[] { AVCaptureDeviceType.BuiltInWideAngleCamera }, AVMediaType.Video, AVCaptureDevicePosition.Back);

                if (deviceDiscoverySession == null)
                {
                    throw new Exception("Failed to get the camera device");
                }

                var captureDevice = deviceDiscoverySession.Devices.FirstOrDefault();

                if (captureDevice == null)
                {
                    throw new Exception("Failed to get the camera device");
                }

                NSError error;
                var input = new AVCaptureDeviceInput(captureDevice, out error);

                if (error != null)
                {
                    throw new Exception(error.ToString());
                }

                var output = new AVCaptureMetadataOutput();

                session = new AVCaptureSession();

                session.AddInput(input);
                session.AddOutput(output);

                output.SetDelegate(this, DispatchQueue.MainQueue);
                output.MetadataObjectTypes = AVMetadataObjectType.QRCode;

                preview = new AVCaptureVideoPreviewLayer(session);
                preview.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;

                view.Layer.InsertSublayer(preview, 0);

                SetNativeControl(view);

                session.StartRunning();

                UpdateViewSize();
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            session?.StopRunning();
            session = null;

            base.Dispose(disposing);
        }

        private void UpdateViewSize()
        {
            if (preview != null && Element != null)
            {
                preview.Frame = new CoreGraphics.CGRect(0, 0, Element.Width, Element.Height);
            }
        }
    }
}

#endif