using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using weatherapp.utils.Api;
using weatherapp.utils.UI;

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
            await LoadWeatherData();
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

                    var mainIconPath = WeatherUi.GetWeatherIconPath(weatherInfo.condition, weatherInfo.iconUrl);
                    MainWeatherIcon.Source = new BitmapImage(new Uri(mainIconPath, UriKind.Relative));

                    if (useCelsius)
                    {
                        HighLow.Text = $"H:{weatherInfo.maxtemp_c:F0}°  L:{weatherInfo.mintemp_c:F0}°";
                    }
                    else
                    {
                        HighLow.Text = $"H:{weatherInfo.maxtemp_f:F0}°  L:{weatherInfo.mintemp_f:F0}°";
                    }

                    Summary.Text = WeatherUi.GetWeatherSummary(weatherInfo.condition);
                    var bgimage = WeatherUi.setWeatherBg(weatherInfo.condition);
                    BackgroundImage.Source = new BitmapImage(new Uri(bgimage, UriKind.Relative));

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
        if (Celsius)
            {
                Temperature.Text = weatherInfo.temp_c + "°";
            }
            else
            {
                Temperature.Text = weatherInfo.temp_f + "°";
            }
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
                        var iconPath = WeatherUi.GetWeatherIconPath(condition, hour.iconUrl);

                        System.Diagnostics.Debug.WriteLine($"Adding hour: {time}, Temp: {temp}, Condition: {condition}");

                        var hourlyItem = ElementUtil.CreateStackPanel(Orientation.Vertical, 50, new Thickness(4, 0, 4, 0));
                        var timeText = ElementUtil.CreateTextBlock(time, Brushes.Black, 12, FontWeights.Medium, HorizontalAlignment.Center, new Thickness(0, 0, 0, 6));

                        var weatherIcon = ElementUtil.CreateImage(iconPath, 28, 28, HorizontalAlignment.Center, new Thickness(0,0,0,6));

                        var tempText = ElementUtil.CreateTextBlock(temp, Brushes.Black, 14, FontWeights.SemiBold, HorizontalAlignment.Center, new Thickness(0));

                        hourlyItem.Children.Add(timeText);
                        hourlyItem.Children.Add(weatherIcon);
                        hourlyItem.Children.Add(tempText);

                        hourlyForecastPanel.Children.Add(hourlyItem);

                        var itemIndex = hourlyForecastPanel.Children.Count - 1;
                        if (itemIndex < (weatherInfo.hourlyForecast.Take(6).Count() * 2) - 2)
                        {
                            var separator = ElementUtil.CreateRectangle(0.5, 60, new SolidColorBrush(Color.FromArgb(64, 0, 0, 0)), VerticalAlignment.Center, new Thickness(8, 0, 0, 8));

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