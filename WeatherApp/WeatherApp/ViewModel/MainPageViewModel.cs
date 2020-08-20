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

                ActualWeather = !string.IsNullOrEmpty(_city) 
                ?  await WeatherService.GetWeatherByName(_city) 
                : await WeatherService.GetWeatherByName();
            if(ActualWeather==null)
                await Application.Current.MainPage.DisplayAlert("Info", "City not found","");
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }
        private bool CanExecuteGetWeather(object obj)
        {
            return !string.IsNullOrEmpty(City);
        }
        public async void LoadWeather()
        {
            ActualWeather=await WeatherService.GetWeatherByName();
            //TODO Convert UTC in LocalTime
            var Sunrise=Utilities.Utilities.UnixTimeStampToDateTime(ActualWeather.Sys.Sunrise);
            var Sunset=Utilities.Utilities.UnixTimeStampToDateTime(ActualWeather.Sys.Sunset);
        }
    }
}
