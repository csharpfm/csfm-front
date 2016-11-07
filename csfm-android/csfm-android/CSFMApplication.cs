using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace csfm_android
{
    [Application]
    public class CSFMApplication : Application
    {
        static CSFMApplication _instance;

        public CSFMApplication(IntPtr handle, JniHandleOwnership transfer): base(handle, transfer)
        {
            _instance = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public static CSFMApplication Instance
        {
            get { return _instance; }
        }
    }
}