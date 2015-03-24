using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using WP.TiTanCam.Model;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Microsoft.Phone.Tasks;

namespace WP.TiTanCam.Views
{
    public partial class CameraRoll : PhoneApplicationPage
    {
        private ObservableCollection<MediaImage> listImage = new ObservableCollection<MediaImage>();
        private PhotoChooserTask photoTask = new PhotoChooserTask();
        private BitmapImage selectedImage;

        public CameraRoll()
        {
            InitializeComponent();
        }

        void photoTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);

                var instance = Resource.Instance;
                instance.ImageFile = image;
                NavigationService.Navigate(new Uri("/Views/CameraRollItem.xaml", UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //GetImage();
            photoTask.Completed += photoTask_Completed;
            photoTask.Show();
            ListThumbnail.ItemsSource = listImage;
        }
        public void GetImage()
        {
            MediaSource mediaSource = MediaSource.GetAvailableMediaSources().First(source => source.MediaSourceType == MediaSourceType.LocalDevice);
            using (MediaLibrary mediaLibrary = new MediaLibrary(mediaSource))
            {
                PictureAlbum cameraRollAlbum = mediaLibrary.RootPictureAlbum.Albums.First((album) => album.Name == "Camera Roll"); //Get albulm Cameraroll
                PictureCollection pictures = cameraRollAlbum.Pictures;
                //MessageBox.Show(pictures.Count().ToString());
                try
                {
                    if (pictures != null)
                    {
                        foreach (var item in pictures)
                        {
                            //binding to listbox
                            BitmapImage bitImage = new BitmapImage();
                            bitImage.SetSource(item.GetThumbnail());
                            MediaImage mediaImage = new MediaImage();
                            mediaImage.ImageFile = bitImage;
                            listImage.Add(mediaImage);
                        }
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Camera roll null");
                }

            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
            //NavigationService.GoBack();
            //MessageBox.Show("dasdas");

        }
        private void ListThumbnail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = sender as Image;
            if (item == null)
            {
                MessageBox.Show("Listbox null");
            }
            selectedImage = item.Source as BitmapImage;

            //try
            //{
            //    if (selectedImage != null)
            //    {
            //        //var instance = Resource.Instance;
            //        //instance.ImageFile = selectedImage;
            //        //NavigationService.Navigate(new Uri("/Views/CameraRollItem.xaml", UriKind.Relative));
            //    }
            //}
            //catch (Exception)
            //{

            //    MessageBox.Show("Not Image Selected");
            //}
            //MediaImage a = new MediaImage();

            //PhoneApplicationService.Current.State["selectedItem"] = image;

            //NavigationService.Navigate(new Uri("/Views/CameraRollItem.xaml", UriKind.Relative));
            //NavigationService.Navigate(new Uri(String.Format("/Views/CameraRollItem.xaml?msg={0}&number = {1}", image, 1), UriKind.Relative));

        }
    }
}