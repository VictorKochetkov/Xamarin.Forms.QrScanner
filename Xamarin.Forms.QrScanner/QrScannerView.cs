using System;

namespace Xamarin.Forms.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class QrScannerView : View
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty ResultProperty = BindableProperty.Create(
            nameof(Result),
            typeof(Command<string>),
            typeof(QrScannerView),
            null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                (bindable as QrScannerView).Result = (Command<string>)newValue;
            });

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty IsScanningProperty = BindableProperty.Create(
            nameof(IsScanning),
            typeof(bool),
            typeof(QrScannerView),
            false,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                (bindable as QrScannerView).IsScanning = (bool)newValue;
            });

        /// <summary>
        /// 
        /// </summary>
        public Command<string> Result
        {
            get => (Command<string>)GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsScanning
        {
            get => (bool)GetValue(IsScanningProperty);
            set => SetValue(IsScanningProperty, value);
        }
    }
}
