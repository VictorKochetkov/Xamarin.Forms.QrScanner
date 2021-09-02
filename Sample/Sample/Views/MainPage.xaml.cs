using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sample.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly Page page;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// 
        /// </summary>
        public Command StartScanner => new Command(async (result) =>
        {
            await page.Navigation.PushAsync(new QrPage());
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        public MainPageViewModel(Page page)
        {
            this.page = page;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
                await Permissions.RequestAsync<Permissions.Camera>();
        }
    }
}
