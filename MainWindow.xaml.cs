using Microsoft.ProjectOxford.Face;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CharacterLikeness.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MemoryStream Stream1 = null;
        private MemoryStream Stream2 = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectButton1_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Imágenes *.jpg|*.jpg";
            var result = ofd.ShowDialog();
            if (result == true)
            {
                var stream = ofd.OpenFile();
                Stream1 = new MemoryStream();
                stream.CopyTo(Stream1);
                Stream1.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = Stream1;
                bitmapImage.EndInit();
                Image1.Source = bitmapImage;
            }
        }

        private void SelectButton2_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Imágenes *.jpg|*.jpg";
            var result = ofd.ShowDialog();
            if (result == true)
            {
                var stream = ofd.OpenFile();
                Stream2 = new MemoryStream();
                stream.CopyTo(Stream2);
                Stream2.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = Stream2;
                bitmapImage.EndInit();
                Image2.Source = bitmapImage;
            }
        }

        private async void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            Stream1.Seek(0, SeekOrigin.Begin);
            Stream2.Seek(0, SeekOrigin.Begin);

            var service = new FaceServiceClient("e5571b6797284cdf90164ed342d6d628", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");
            var face1 = await service.DetectAsync(Stream1);
            var face2 = await service.DetectAsync(Stream2);
            var verify = await service.VerifyAsync(face1[0].FaceId, face2[0].FaceId);
            CompareValue.Text = string.Format("{0:0.00}%", verify.Confidence * 100);
            CompareResult.Visibility = Visibility.Visible;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            Image1.Source = null;
            Image2.Source = null;
            Stream1 = null;
            Stream2 = null;
            CompareResult.Visibility = Visibility.Collapsed;
        }
    }
}