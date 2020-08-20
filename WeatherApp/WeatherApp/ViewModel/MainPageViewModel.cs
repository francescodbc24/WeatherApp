using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using Xamarin.Forms;
using Xamarin.Essentials;
using WeatherApp.Messaging;

namespace WeatherApp.ViewModel
{
    public class MainPageViewModel :INotifyPropertyChanged
    {
        private WeatherServices WeatherService = new WeatherServices();

        private Weather _actualweather;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GetWeatherCommand { get; set; }

        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(City))); }
        }
        public Weather ActualWeather
        {
            get { return _actualweather; }
            set { _actualweather = value; PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ActualWeather))); }
        }
        public MainPageViewModel()
        {
            LoadWeather();
            GetWeatherCommand = new Command(async () => await ExecuteGetWeather(null));
            this.PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == nameof(City))
                {
                    GetWeatherCommand.CanExecute(null);
                }
            };
        }
        private async Task ExecuteGetWeather(object obj)
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    MessagingCenter.Send(this, nameof(SendMessage), new SendMessage() { Title = "Error", Msg = "Ops looks like you're not connected to the internet" });
                ActualWeather = !string.IsNullOrEmpty(_city) 
                ?  await WeatherService.GetWeatherByName(_city) 
                : await WeatherService.GetWeatherByName();
            if(ActualWeather==null)
                 MessagingCenter.Send(this, nameof(SendMessage), new SendMessage() { Title = "Info", Msg = "City not found." });
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, nameof(SendMessage),new SendMessage() { Title="Error",Msg=ex.Message });
            }
                        
        }
       
        public async void LoadWeather()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    MessagingCenter.Send(this, nameof(SendMessage), new SendMessage() { Title = "Error", Msg = "Ops looks like you're not connected to the internet" });
                ActualWeather =await WeatherService.GetWeatherByName();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, nameof(SendMessage), new SendMessage() { Title = "Error", Msg = ex.Message });
            }
        }
    }
}
