using Prism.Mvvm;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RefTestVisual
{
    public class MainWindowViewModel : BindableBase
    {
        private WriteableBitmap _img;

        public WriteableBitmap Img {
            get => _img;
            set => SetProperty(ref _img, value);
        }

        public void Resize(int newWidth, int newHeight)
        {
            Img.Resize(newWidth, newHeight);
            //Img = BitmapFactory.New(newWidth, newHeight);
            Img.Clear();
            Img.DrawLine(0, 0, newWidth, newHeight, Color.FromRgb(255, 0, 0));
        }

        public MainWindowViewModel()
        {
            Img = BitmapFactory.New(100, 100);
            Img.DrawLine(0, 0, 100, 100, Color.FromRgb(255, 0, 0));

        }
    }

    public static class BitmapExtensions
    {
        public static WriteableBitmap Resize(this WriteableBitmap source, int newWidth, int newHeight)
        {
            var sw = Stopwatch.StartNew();
            // Создание нового WriteableBitmap с новыми размерами
            WriteableBitmap resizedBitmap = new WriteableBitmap(newWidth, newHeight, source.DpiX, source.DpiY, source.Format, null);

            // Вычисление соотношения сторон старого и нового изображений
            int xRatio = source.PixelWidth / newWidth;
            int yRatio = source.PixelHeight / newHeight;

            // Доступ к пиксельным данным исходного изображения
            int[] sourcePixels = new int[source.PixelWidth * source.PixelHeight];
            source.CopyPixels(sourcePixels, source.PixelWidth * 4, 0);
           
            var resizedPixels = NearestNeighbor(source.PixelWidth, newWidth, newHeight, xRatio, yRatio, sourcePixels);
            // Копирование данных пикселей в новый WriteableBitmap
            resizedBitmap.WritePixels(new Int32Rect(0, 0, newWidth, newHeight), resizedPixels, newWidth * 4, 0);
            Debug.WriteLine(sw.ElapsedMilliseconds);
            return resizedBitmap;
        }

        private static async Task<int[]> Bilinear(int width, int height, int newWidth, int newHeight, double xRatio, double yRatio, int[] sourcePixels)
        {
           return await Task.Run(() =>
            {
                // Пиксельные данные нового изображения
                int[] resizedPixels = new int[newWidth * newHeight];
                // Основной цикл по пикселям нового изображения
                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        // Определение координат исходного пикселя
                        double gx = x * xRatio;
                        double gy = y * yRatio;
                        int gxi = (int)gx;
                        int gyi = (int)gy;

                        int gxi1 = Math.Min(gxi + 1, width - 1);
                        int gyi1 = Math.Min(gyi + 1, height - 1);

                        // Получение цвета пикселей для интерполяции
                        int c00 = sourcePixels[gxi + gyi * width];
                        int c10 = sourcePixels[gxi1 + gyi * width];
                        int c01 = sourcePixels[gxi + gyi1 * width];
                        int c11 = sourcePixels[gxi1 + gyi1 * width];

                        // Фракционные части
                        double fracX = gx - gxi;
                        double fracY = gy - gyi;
                        double fracX1 = 1.0 - fracX;
                        double fracY1 = 1.0 - fracY;

                        // Билинейная интерполяция
                        int red = (int)(
                            ((c00 >> 16 & 0xFF) * fracX1 * fracY1 +
                             (c10 >> 16 & 0xFF) * fracX * fracY1 +
                             (c01 >> 16 & 0xFF) * fracX1 * fracY +
                             (c11 >> 16 & 0xFF) * fracX * fracY));

                        int green = (int)(
                            ((c00 >> 8 & 0xFF) * fracX1 * fracY1 +
                             (c10 >> 8 & 0xFF) * fracX * fracY1 +
                             (c01 >> 8 & 0xFF) * fracX1 * fracY +
                             (c11 >> 8 & 0xFF) * fracX * fracY));

                        int blue = (int)(
                            ((c00 & 0xFF) * fracX1 * fracY1 +
                             (c10 & 0xFF) * fracX * fracY1 +
                             (c01 & 0xFF) * fracX1 * fracY +
                             (c11 & 0xFF) * fracX * fracY));

                        int alpha = (int)(
                            ((c00 >> 24 & 0xFF) * fracX1 * fracY1 +
                             (c10 >> 24 & 0xFF) * fracX * fracY1 +
                             (c01 >> 24 & 0xFF) * fracX1 * fracY +
                             (c11 >> 24 & 0xFF) * fracX * fracY));

                        // Объединение компонентов цвета и установка пикселя
                        resizedPixels[x + y * newWidth] = (alpha << 24) | (red << 16) | (green << 8) | blue;

                    }

                }
                return resizedPixels;
            });
        }
        private static int[] NearestNeighbor(int width, int newWidth, int newHeight, int xRatio, int yRatio, int[] sourcePixels)
        {
            // Пиксельные данные нового изображения
            int[] resizedPixels = new int[newWidth * newHeight];
            // Основной цикл по пикселям нового изображения
            for (int y = 0; y < newHeight; y++)
            {
                int nearestY = (y * yRatio) * width;
                for (int x = 0; x < newWidth; x++)
                {
                    // Определение координат ближайшего пикселя исходного изображения
                    int nearestX = (x * xRatio);

                    // Извлечение значения пикселя и установка в новое изображение
                    resizedPixels[x + y * newWidth] = sourcePixels[nearestX + nearestY];
                }
            }
            return resizedPixels;
            
        }
    }
}
