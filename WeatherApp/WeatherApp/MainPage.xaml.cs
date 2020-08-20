using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Messaging;
using WeatherApp.ViewModel;
using Xamarin.Forms;

namespace WeatherApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MainPageViewModel,SendMessage>(this,nameof(SendMessage),async (sender,argument) =>
            {
               await  DisplayAlert(argument.Title, argument.Msg,"Ok");
            });
        }
    }
}
