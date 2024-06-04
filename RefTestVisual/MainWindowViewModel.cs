using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RefTestVisual
{
    public class MainWindowViewModel : BindableBase
    {
        private WriteableBitmap _img;
        private Size _rect;

        public WriteableBitmap Img {
            get => _img;
            set => SetProperty(ref _img, value);
        }

        public Size Rect {
            get => _rect;
            set
            {
                SetProperty(ref _rect, value);
                Img.Resize((int)value.Width, (int)value.Height, WriteableBitmapExtensions.Interpolation.Bilinear);
            }
        }
        public MainWindowViewModel()
        {
            Img = BitmapFactory.New(1, 1);
            Img.DrawLine(0, 0, 100, 100, Color.FromRgb(255, 0, 0));

        }
    }
}
