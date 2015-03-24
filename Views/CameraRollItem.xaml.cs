using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

using WP.TiTanCam.Model;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media;

namespace WP.TiTanCam.Views
{
    public partial class CameraRollItem : PhoneApplicationPage
    {

        BitmapImage selectedImage = new BitmapImage();
        public CameraRollItem()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //GetImage(pathImage);
            //NavigationContext.QueryString.TryGetValue("pathIMG", out pathImage);
            //MessageBox.Show(pathImage);
            var instance = Resource.Instance;
            selectedImage = instance.ImageFile;
            string h = selectedImage.PixelHeight.ToString();
            
            MainImage.Source = selectedImage;
            
            //MainImage.RenderTransform = new RotateTransform() { CenterX = 0.5, CenterY = 0.5, Angle = 90 };
            //MessageBox.Show(selectedImage.PixelWidth.ToString() + " "+selectedImage.PixelHeight.ToString());
            //RotateImage();
            //MainImage.Source = selectedImage;
        }

        private void RotateImage()
        {
            
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            NavigationService.GoBack();
        }
        public void GetImage(string path)
        {
            MediaSource mediaSource = MediaSource.GetAvailableMediaSources().First(source => source.MediaSourceType == MediaSourceType.LocalDevice);
            using (MediaLibrary mediaLibrary = new MediaLibrary(mediaSource))
            {
                PictureAlbum cameraRollAlbum = mediaLibrary.RootPictureAlbum.Albums.First((album) => album.Name == "Camera Roll"); //Get albulm Cameraroll
                PictureCollection pictures = cameraRollAlbum.Pictures;
                try
                {
                    if (pictures != null)
                    {
                        Picture picture = pictures.FirstOrDefault(p => p.Name == path);
                        var stream = picture.GetImage();
                        selectedImage.SetSource(stream);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Camera roll null");
                }

            }
            //return selectedImage;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}