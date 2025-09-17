using System.Text;
using System.Windows;
using weatherapp.utils;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Linq;

namespace weatherapp
{
    
    public partial class MainWindow : Window
    {
        private bool useCelsius = true;
        private StackPanel hourlyForecastPanel;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += main;
        }

        public async void main(object sender, RoutedEventArgs e)
        {
            settemp(true);
        }

        private async System.Threading.Tasks.Task LoadWeatherData()
        {
            try
            {
                var weatherInfo = await WeatherApi.GetWeatherInfo();
                if (weatherInfo != null)
                {
                    Location.Text = weatherInfo.location;

                    settemp(useCelsius, weatherInfo);

                    WeatherDesc.Text = weatherInfo.condition;

                    var mainIconPath = GetWeatherIconPath(weatherInfo.condition, weatherInfo.iconUrl);
                    MainWeatherIcon.Source = new BitmapImage(new Uri(mainIconPath, UriKind.Relative));

                    if (useCelsius)
                    {
                        HighLow.Text = $"H:{weatherInfo.maxtemp_c:F0}°  L:{weatherInfo.mintemp_c:F0}°";
                    }
                    else
                    {
                        HighLow.Text = $"H:{weatherInfo.maxtemp_f:F0}°  L:{weatherInfo.mintemp_f:F0}°";
                    }

                    Summary.Text = GetWeatherSummary(weatherInfo.condition);

                    UpdateHourlyForecast(weatherInfo);
                }
            }
            catch (Exception ex)
            {
                Location.Text = "Location Error";
                Temperature.Text = "--°";
                WeatherDesc.Text = "Unable to load weather";
                System.Diagnostics.Debug.WriteLine($"Weather loading error: {ex.Message}");
            }
        }

        private void HourlyScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }

        public void settemp(bool Celsius, WeatherApi.WeatherInfo weatherInfo)
        {
            if (weatherInfo != null)
            {
                if (Celsius)
                {
                    Temperature.Text = weatherInfo.temp_c + "°";
                }
                else
                {
                    Temperature.Text = weatherInfo.temp_f + "°";
                }
            }
        }

        private string GetWeatherIconPath(string condition, string iconUrl = "")
        {
            var lowerCondition = condition?.ToLower() ?? "";
            bool isNight = iconUrl.Contains("/night/");

            return lowerCondition switch
            {
                var c when c.Contains("sunny") || c.Contains("clear") => 
                    isNight ? "/Resources/CloudIcons/clearnight.png" : "/Resources/CloudIcons/Clear.png",
                
                var c when c.Contains("partly cloudy") || c.Contains("partially cloudy") => 
                    isNight ? "/Resources/CloudIcons/partycloudynight.png" : "/Resources/CloudIcons/partlycloudy.png",
                
                var c when c.Contains("cloudy") || c.Contains("overcast") => "/Resources/CloudIcons/Cloudy.png",
                
                var c when c.Contains("drizzle") || c.Contains("light rain") || c.Contains("patchy rain nearby") || c.Contains("light drizzle") => 
                    isNight ? "/Resources/CloudIcons/Drizzlenight.png" : "/Resources/CloudIcons/Drizzle.png",
                
                var c when c.Contains("rain") || c.Contains("shower") || c.Contains("moderate rain") || c.Contains("heavy rain") => 
                    "/Resources/CloudIcons/Rain.png",
                
                var c when c.Contains("heavy snow") || c.Contains("blizzard") => "/Resources/CloudIcons/Heavysnow.png",
                var c when c.Contains("snow") || c.Contains("light snow") || c.Contains("moderate snow") => "/Resources/CloudIcons/Snow.png",
                
                var c when c.Contains("thunderstorm") || c.Contains("thunder") || c.Contains("thundery") => 
                    "/Resources/CloudIcons/Thunderstorm.png",
                
                var c when c.Contains("fog") || c.Contains("mist") => "/Resources/CloudIcons/Fog.png",
                
                var c when c.Contains("haze") => "/Resources/CloudIcons/Haze.png",
                
                var c when c.Contains("freezing") || c.Contains("sleet") || c.Contains("ice pellets") => 
                    "/Resources/CloudIcons/Freezingrain.png",
                
                var c when c.Contains("windy") || c.Contains("wind") => "/Resources/CloudIcons/Windy.png",
                
                _ => isNight ? "/Resources/CloudIcons/clearnight.png" : "/Resources/CloudIcons/Clear.png"
            };
        }

        private string GetWeatherSummary(string condition)
        {
            return condition.ToLower() switch
            {
                var c when c.Contains("sunny") || c.Contains("clear") => "Sunny conditions will continue throughout the day. Perfect weather for outdoor activities.",
                var c when c.Contains("cloudy") || c.Contains("overcast") => "Cloudy skies expected with comfortable temperatures.",
                var c when c.Contains("rain") || c.Contains("shower") => "Rain is expected. Consider bringing an umbrella if going outside.",
                var c when c.Contains("snow") => "Snow conditions expected. Drive carefully and dress warmly.",
                var c when c.Contains("fog") || c.Contains("mist") => "Visibility may be reduced due to foggy conditions.",
                _ => $"{condition} conditions expected for today."
            };
        }

        private void UpdateHourlyForecast(WeatherApi.WeatherInfo weatherInfo)
        {
            if (weatherInfo.hourlyForecast != null && weatherInfo.hourlyForecast.Count > 0)
            {
                var scrollViewer = this.FindName("HourlyScrollViewer") as ScrollViewer;
                if (scrollViewer != null)
                {
                    hourlyForecastPanel = scrollViewer.Content as StackPanel;
                }

                if (hourlyForecastPanel != null)
                {
                    hourlyForecastPanel.Children.Clear();
                    System.Diagnostics.Debug.WriteLine($"Found {weatherInfo.hourlyForecast.Count} hourly forecast items");

                    foreach (var hour in weatherInfo.hourlyForecast.Take(6))
                    {
                        var time = DateTime.Parse(hour.time).ToString("ha").ToLower();
                        var temp = useCelsius ? $"{hour.temp_c:F0}°" : $"{hour.temp_f:F0}°";
                        var condition = hour.condition;
                        var rainChance = hour.chance_of_rain;
                        var iconPath = GetWeatherIconPath(condition, hour.iconUrl);

                        System.Diagnostics.Debug.WriteLine($"Adding hour: {time}, Temp: {temp}, Condition: {condition}");

                        var hourlyItem = new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Width = 50,
                            Margin = new Thickness(4, 0, 4, 0)
                        };

                        var timeText = new TextBlock
                        {
                            Text = time,
                            Foreground = Brushes.Black,
                            FontSize = 12,
                            FontWeight = FontWeights.Medium,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 6)
                        };

                        var weatherIcon = new Image
                        {
                            Source = new BitmapImage(new Uri(iconPath, UriKind.Relative)),
                            Width =28,
                            Height = 28,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 6)
                        };

                        var tempText = new TextBlock
                        {
                            Text = temp,
                            Foreground = Brushes.Black,
                            FontSize = 14,
                            FontWeight = FontWeights.SemiBold,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        hourlyItem.Children.Add(timeText);
                        hourlyItem.Children.Add(weatherIcon);
                        hourlyItem.Children.Add(tempText);

                        hourlyForecastPanel.Children.Add(hourlyItem);

                        var itemIndex = hourlyForecastPanel.Children.Count - 1;
                        if (itemIndex < (weatherInfo.hourlyForecast.Take(6).Count() * 2) - 2)
                        {
                            var separator = new Rectangle
                            {
                                Width = 0.5,
                                Height = 60,
                                Fill = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0)),
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(8, 0, 8, 0)
                            };
                            hourlyForecastPanel.Children.Add(separator);
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Total children added to panel: {hourlyForecastPanel.Children.Count}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("hourlyForecastPanel is null!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"No hourly forecast data available. Count: {weatherInfo?.hourlyForecast?.Count ?? 0}");
            }
        }
        
        private async void ToggleTemperatureUnit()
        {
            useCelsius = !useCelsius;
            await LoadWeatherData();
        }
    }
}