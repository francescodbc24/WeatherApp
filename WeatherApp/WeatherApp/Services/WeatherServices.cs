using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Configuration;
using WeatherApp.Models;


namespace WeatherApp.Services
{
    public class WeatherServices
    {
        string Url = Settings.UrlApi + "weather?q=$ID&APPID=" + Settings.ApiKey; 
        string PathImg = "https://openweathermap.org/img/wn/";
        public async Task<Weather> GetWeatherByName(string name = "Bologna")
        {
            try
            {

                string url = Url.Replace("$ID", name);
                using (var http = new HttpClient())
                {
                    var response = await http.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var weather = JsonConvert.DeserializeObject<Weather>(content);
                        weather.Main.Temp = ConvertKelvinToCelcius(weather.Main.Temp);
                        weather.Main.TempMax = ConvertKelvinToCelcius(weather.Main.TempMax);
                        weather.Main.TempMin = ConvertKelvinToCelcius(weather.Main.TempMin);
                        foreach (var item in weather.WeatherWeather)
                        {
                            item.Icon = PathImg + item.Icon + "@4x.png";
                            weather.Image = item.Icon;
                        }
                        return weather;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }
   
        
        public double ConvertKelvinToCelcius(double temp)
        {
            return temp - 273.15;

        }
        public double ConvertKelvinToFahrenheit(double temp)
        {
            return temp + 273.15;

        }
        

    }
}
