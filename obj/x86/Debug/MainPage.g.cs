﻿#pragma checksum "D:\VTCA\MD04\Windows Phone\Lab\Windows Phone App\WP.TiTanCam\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "665EEA967D7A6EDB06E77DA225BA7B56"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WP.TiTanCam {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Canvas ViewFinderCanvas;
        
        internal System.Windows.Shapes.Rectangle FocusIndicator;
        
        internal System.Windows.Media.VideoBrush ViewFinderBrush;
        
        internal System.Windows.Controls.Grid ListButtonGrid;
        
        internal System.Windows.Controls.Button CameraRollButton;
        
        internal System.Windows.Controls.Button CaptureButton;
        
        internal System.Windows.Controls.Button FilterButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WP.TiTanCam;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ViewFinderCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ViewFinderCanvas")));
            this.FocusIndicator = ((System.Windows.Shapes.Rectangle)(this.FindName("FocusIndicator")));
            this.ViewFinderBrush = ((System.Windows.Media.VideoBrush)(this.FindName("ViewFinderBrush")));
            this.ListButtonGrid = ((System.Windows.Controls.Grid)(this.FindName("ListButtonGrid")));
            this.CameraRollButton = ((System.Windows.Controls.Button)(this.FindName("CameraRollButton")));
            this.CaptureButton = ((System.Windows.Controls.Button)(this.FindName("CaptureButton")));
            this.FilterButton = ((System.Windows.Controls.Button)(this.FindName("FilterButton")));
        }
    }
}
