using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WP.TiTanCam.Model
{
    public sealed class Resource
    {
        public static object syncRoot = new object();

        private static volatile Resource instance;

        public static Resource Instance
        {
            get {
                if(instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                            instance = new Resource();
                    }
                }
                return instance;
            }
            
        }

        private Resource() { }

        public BitmapImage ImageFile;
        
    }
}
