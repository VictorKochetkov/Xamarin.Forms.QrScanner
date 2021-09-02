using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Sample.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class QrPageViewModel : INotifyPropertyChanged
    {
        private readonly Page page;
        private bool isScanning = true;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// 
        /// </summary>
        public bool IsScanning
        {
            get => isScanning;
            set
            {
                isScanning = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsScanning)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Command<string> ResultCommand => new Command<string>(async (result) =>
        {
            if (IsScanning)
            {
                IsScanning = false;

                await page.DisplayAlert("QR scanned", result, "Ok");

                IsScanning = true;
            }
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        public QrPageViewModel(Page page)
        {
            this.page = page;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QrPage : ContentPage
    {
        public QrPage()
        {
            InitializeComponent();
            BindingContext = new QrPageViewModel(this);
        }
    }
}
