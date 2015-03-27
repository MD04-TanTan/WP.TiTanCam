using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.TiTanCam.ViewModel
{
    public enum Resolution { WVGA, WXGA, HD };

    public class ResolutionHelper
    {
        private static bool IsWvga
        {
            get { return App.Current.Host.Content.ScaleFactor == 100; }
        }

        private static bool IsWxga
        {
            get { return App.Current.Host.Content.ScaleFactor == 160; }
        }

        private static bool IsHD
        {
            get { return App.Current.Host.Content.ScaleFactor == 150; }
        }

        public static Resolution CurrentResolution
        {
            get
            {
                if (IsWvga)return Resolution.WVGA;
                else if (IsWxga) return Resolution.WXGA;
                else if (IsHD) return Resolution.HD;
                else throw new InvalidOperationException("Unknows resolution");
            }
        }
    }
}
