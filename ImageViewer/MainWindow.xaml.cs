using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Win32;

namespace ImageViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            infoHeader.Height = ActualHeight / 6;
            editHeader.Height = infoHeader.Height;
            rotateHeader.Height = infoHeader.Height;
            filterHeader.Height = infoHeader.Height;
            infoHeader.Height = infoHeader.Height;
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
            "Portable Network Graphic (*.png)|*.png",
            };
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document";
            dlg.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
            if (dlg.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder(); //
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)imgPhoto.Source));
                using (var stream = dlg.OpenFile())
                {
                    encoder.Save(stream);
                }
            }
        }

        private void RotateClick(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            tb.BeginInit();
            try
            {
                var tmpImage = (BitmapImage)imgPhoto.Source;
                tb.Source = tmpImage;
            }
            catch
            {
                tb.Source = (TransformedBitmap)imgPhoto.Source;
            }
            RotateTransform transform = new RotateTransform(90);
            tb.Transform = transform;
            tb.EndInit();
            imgPhoto.Source = tb;
        }

        private void GreyscaleClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            Filter filter = new Filter();
            imageBmp = filter.Grayscale(imageBmp);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void SepiaClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            Filter filter = new Filter();
            imageBmp = filter.Sepia(imageBmp);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void PinkClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            Filter filter = new Filter();
            imageBmp = filter.Pink(imageBmp);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void GreenyClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            Filter filter = new Filter();
            imageBmp = filter.Greeny(imageBmp);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void YellowClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            Filter filter = new Filter();
            imageBmp = filter.Yellow(imageBmp);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void VioletClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            Filter filter = new Filter();
            imageBmp = filter.Violet(imageBmp);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void BlurClick(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = (BitmapSource)imgPhoto.Source;
            Bitmap imageBmp = BitmapImage2Bitmap(bitmap);
            GaussianBlur blur = new GaussianBlur(imageBmp);
            imageBmp = blur.Process(5);
            imgPhoto.Source = BitmapToBitmapImage(imageBmp);
        }

        private void InfoClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Width: " + imgPhoto.Source.Width + ", Height: " + imgPhoto.Source.Height);
        }

        private static Bitmap BitmapImage2Bitmap(BitmapSource bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                var bitmap = new Bitmap(outStream);
                return bitmap;
            }
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}