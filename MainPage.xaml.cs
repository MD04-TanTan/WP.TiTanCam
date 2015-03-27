using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP.TiTanCam.Resources;

using Windows.Foundation.Collections;
using Lumia.Imaging;
using System.Threading.Tasks;
using Windows.Phone.Media.Capture;
using System.Windows.Media.Imaging;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using Windows.Storage;
using System.Diagnostics;
using Microsoft.Phone.Tasks;
using WP.TiTanCam.Model;
using WP.TiTanCam.ViewModel;

namespace WP.TiTanCam
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoCaptureDevice captureDevice;
        MediaLibrary library = new MediaLibrary();
        private PhotoChooserTask photoTask = new PhotoChooserTask();
        private int saveCounter = 0;


        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        protected async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(ResolutionHelper.CurrentResolution.ToString());
            CheckResolution();
            //Check available back cam & front cam
            if (PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Back) || PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Back))
            {
                if (PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Back)) //check available back cam
                {
                    IReadOnlyList<Windows.Foundation.Size> supportedResolutions = PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Back);
                    Windows.Foundation.Size res = supportedResolutions[0];
                    captureDevice = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Back, res); // init object Photo Capture Device
                }
                else // check front cam
                {
                    IReadOnlyList<Windows.Foundation.Size> supportedResolutions = PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Front);
                    Windows.Foundation.Size res = supportedResolutions[0];
                    captureDevice = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Front, res);
                }

                captureDevice.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation, captureDevice.SensorLocation == CameraSensorLocation.Back
                    ? captureDevice.SensorRotationInDegrees : -captureDevice.SensorRotationInDegrees);
                ViewFinderPanel.SetSource(captureDevice); //set source VideoBrush
                ViewFinderPanel.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 }; //set transform portrait 

            }
        }

        private void CheckResolution()
        {
            var screen = ResolutionHelper.CurrentResolution;
            switch (screen)
            {
                case Resolution.HD:
                    LayoutRoot.Width = 480;
                    LayoutRoot.Height = 853;
                    break;
                case Resolution.WVGA:
                    LayoutRoot.Width = 480;
                    LayoutRoot.Height = 800;
                    break;

            }
        }

        private void SetLayoutLandscapeR()
        {
            ViewFinderCanvas.Width = 640;
            ViewFinderCanvas.Height = 480;

            Grid.SetRow(ViewFinderCanvas, 0);
            Grid.SetColumn(ViewFinderCanvas, 1);
            Grid.SetRow(ListButtonGrid, 0);
            Grid.SetColumn(ListButtonGrid, 0);

            Grid.SetRow(CameraRollButton, 0);
            Grid.SetColumn(CameraRollButton, 0);

            Grid.SetRow(CaptureButton, 1);
            Grid.SetColumn(CaptureButton, 0);

            Grid.SetRow(FilterButton, 2);
            Grid.SetColumn(FilterButton, 0);

            ViewFinderPanel.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 180 };
        }
        private void SetLayoutLandscapeL()
        {
            ViewFinderCanvas.Width = 853;
            ViewFinderCanvas.Height = 480;

            //LayoutRoot.RowDefinitions.Clear();
            //LayoutRoot.ColumnDefinitions.Clear();
            //ColumnDefinition column1 = new ColumnDefinition();
            //ColumnDefinition column2 = new ColumnDefinition();
            //LayoutRoot.ColumnDefinitions.Add(column1);
            //LayoutRoot.ColumnDefinitions.Add(column2);

            //Grid.SetRow(ViewFinderCanvas, 0);
            //Grid.SetColumn(ViewFinderCanvas, 0);
            //Grid.SetRow(ListButtonGrid, 0);
            //Grid.SetColumn(ListButtonGrid, 0);

            //Grid.SetRow(CameraRollButton, 0);
            //Grid.SetColumn(CameraRollButton, 0);

            //Grid.SetRow(CaptureButton, 1);
            //Grid.SetColumn(CaptureButton, 0);

            //Grid.SetRow(FilterButton, 2);
            //Grid.SetColumn(FilterButton, 0);

            ViewFinderPanel.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 0 };
        }
        private void SetLayoutPortrait()
        {
            ViewFinderCanvas.Width = 480;
            ViewFinderCanvas.Height = 853;

            Grid.SetRow(ListButtonGrid, 0);
            Grid.SetColumn(ListButtonGrid, 0);

            Grid.SetRow(CameraRollButton, 0);
            Grid.SetColumn(CameraRollButton, 0);

            Grid.SetRow(CaptureButton, 0);
            Grid.SetColumn(CaptureButton, 1);

            Grid.SetRow(FilterButton, 0);
            Grid.SetColumn(FilterButton, 2);

            ViewFinderPanel.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
        }
        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);

            if (e.Orientation == PageOrientation.LandscapeRight) //status bar on the right
            {

                //SetLayoutLandscapeR();
                SetOrientation(PageOrientation.LandscapeRight, ViewFinderCanvas);
            }
            else if (e.Orientation == PageOrientation.LandscapeLeft) //status bar on the left
            {

                SetLayoutLandscapeL();
                SetOrientation(PageOrientation.LandscapeLeft, ViewFinderCanvas);
            }
            else //portrait
            {
                SetLayoutPortrait();
                SetOrientation(PageOrientation.PortraitUp, ViewFinderCanvas);
            }


        }

        public void SetOrientation(PageOrientation pageOrientation, Canvas viewFinderCanvas)
        //public void SetOrientation(PageOrientation pageOrientation)
        {
            int degree = 0;
            viewFinderCanvas.Opacity = 0;
            var videoBrush = viewFinderCanvas.Background as VideoBrush;
            if (captureDevice.SensorLocation == CameraSensorLocation.Front)
            {
                switch (pageOrientation)
                {
                    case PageOrientation.LandscapeLeft:
                        degree = 0;
                        //ChangeLayout(false, viewFinderPanel);
                        break;
                    case PageOrientation.PortraitUp:
                        degree = 270;
                        //ChangeLayout(true, viewFinderPanel);
                        break;
                    case PageOrientation.LandscapeRight:
                        degree = 180;
                        //ChangeLayout(false, viewFinderPanel);
                        break;
                    case PageOrientation.PortraitDown:
                        degree = 90;
                        //ChangeLayout(true, viewFinderPanel);
                        break;
                }
            }
            else
            {
                switch (pageOrientation)
                {
                    case PageOrientation.LandscapeLeft:
                        degree = 0;
                        //ChangeLayout(false, viewFinderPanel);
                        break;
                    case PageOrientation.PortraitUp:
                        degree = 90;
                        //ChangeLayout(true, viewFinderPanel);
                        break;
                    case PageOrientation.LandscapeRight:
                        degree = 180;
                        //ChangeLayout(false, viewFinderPanel);
                        break;
                    case PageOrientation.PortraitDown:
                        degree = 270;
                        //ChangeLayout(true, viewFinderPanel);
                        break;
                }
            }
            try
            {
                if (captureDevice != null)
                {
                    captureDevice.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation, degree);
                }
                CompositeTransform compositeTransform = new CompositeTransform()
                {
                    CenterX = 0.5,
                    CenterY = 0.5,
                    Rotation = degree
                };
                videoBrush.RelativeTransform = compositeTransform;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Orientation exception: {0}", ex.Message));
            }
            viewFinderCanvas.Opacity = 1;
        }

        //void myCamera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        //{

        //    string fileName = saveCounter + ".jpg";
        //    try
        //    {
        //        library.SavePictureToCameraRoll(fileName, e.ImageStream);
        //        e.ImageStream.Seek(0, SeekOrigin.Begin);

        //        using (IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
        //        {
        //            using (IsolatedStorageFileStream targetStream = isoStorage.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.Write))
        //            {
        //                // Initialize the buffer for 4KB disk pages.
        //                byte[] readBuffer = new byte[4096];
        //                int bytesRead = -1;

        //                // Copy the image to the local folder. 
        //                while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
        //                {
        //                    targetStream.Write(readBuffer, 0, bytesRead);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        MessageBox.Show("Problem occured in Capture Image Available");
        //    }
        //    finally
        //    {
        //        e.ImageStream.Close();
        //    }
        //}

        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (captureDevice != null)
            {
                CapturePhoto();
            }
        }

        private async void CapturePhoto()
        {
            try
            {
                CameraCaptureSequence sequence = captureDevice.CreateCaptureSequence(1);
                captureDevice.SetProperty(KnownCameraGeneralProperties.AutoFocusRange, AutoFocusRange.Infinity);
                captureDevice.SetProperty(KnownCameraGeneralProperties.PlayShutterSoundOnCapture, true);
                captureDevice.SetProperty(KnownCameraPhotoProperties.FlashMode, FlashState.Auto);

                MemoryStream captureStream1 = new MemoryStream();
                sequence.Frames[0].CaptureStream = captureStream1.AsOutputStream();

                await captureDevice.PrepareCaptureSequenceAsync(sequence);

                await sequence.StartCaptureAsync();
                captureStream1.Seek(0, SeekOrigin.Begin);

                MediaLibrary library = new MediaLibrary();
                string filename = "IMG_" + saveCounter;
                Picture pic1 = library.SavePictureToCameraRoll(filename, captureStream1); //Save picture in cameraroll

                //Update Background Cameraroll Button
                BitmapImage bitImage = new BitmapImage();
                bitImage.SetSource(pic1.GetThumbnail());
                ImageBrush imgButton = new ImageBrush();
                imgButton.ImageSource = bitImage;
                CameraRollButton.Background = imgButton;

                saveCounter++;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ViewFinderCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point uiTapPoint = e.GetPosition(ViewFinderCanvas);
            Windows.Foundation.Point tapPoint = new Windows.Foundation.Point(uiTapPoint.X, uiTapPoint.Y);

        }

        private void CameraRollButton_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/CameraRoll.xaml", UriKind.RelativeOrAbsolute));
            photoTask.Completed += photoTask_Completed;
            photoTask.Show();
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

    }
}