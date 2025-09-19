using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace weatherapp.utils.UI
{
    public class ElementUtil
    {

        public static TextBlock CreateTextBlock(string text, Brush foreground, double fontSize, FontWeight fontWeight, HorizontalAlignment horizontal, Thickness margin)
        {
            return new TextBlock
            {
                Text = text,
                Foreground = foreground,
                FontSize = fontSize,
                FontWeight = fontWeight,
                HorizontalAlignment = horizontal,
                Margin = margin
            };
        }

        public static StackPanel CreateStackPanel(Orientation Orientation, double Width, Thickness margin)
        {
            return new StackPanel
            {
                Orientation = Orientation,
                Width = Width,
                Margin = margin
            };
        }

        public static Image CreateImage(string iconpath, double Width, double Height, HorizontalAlignment horizontal, Thickness margin)
        {
            return new Image
            {
                Source = new BitmapImage(new Uri(iconpath, UriKind.Relative)),
                Width = Width,
                Height = Height,
                HorizontalAlignment = horizontal,
                Margin = margin
            };
        }

        // KEEP system.windows.Shapes.Rectangle prevents name conflicts
        public static System.Windows.Shapes.Rectangle CreateRectangle(double Width, double Height, SolidColorBrush Rgba, VerticalAlignment vertical, Thickness margin)
        {
            return new System.Windows.Shapes.Rectangle
            {
                Width = Width,
                Height = Height,
                Fill = Rgba,
                VerticalAlignment = vertical,
                Margin = margin
            };
        }
    }
}
