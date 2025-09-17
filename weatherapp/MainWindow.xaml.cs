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

namespace weatherapp
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += main;
        }


        public async void main(object sender, RoutedEventArgs e)
        {
            setlocation();
            settemp(true);
        }

        private void ImageScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                e.Handled = true;
            }
        }

        public async void setlocation()
        {
            var location = await LocationApi.GetLocationAsync();
            string city = location.City;
            Location.Text = city;
        }

        public async void settemp(bool Celsius)
        {
            var weatherinfo = await WeatherApi.GetWeatherInfo();
            if (Celsius)
            {
                Temperature.Text = weatherinfo.temp_c;
            } else
            {
                Temperature.Text = weatherinfo.temp_f;
            }
        }
    }
}