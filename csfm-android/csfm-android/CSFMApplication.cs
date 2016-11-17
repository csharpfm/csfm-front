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
    /// <summary>
    /// Class defining the application
    /// </summary>
    [Application]
    public class CSFMApplication : Application
    {
        static CSFMApplication _instance;

        static readonly bool isDebug = false;

        static readonly string bearerToken = "token";

        static readonly string username = "csfm_username";

        static readonly string isScrobbling = "is_scrobbling";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="transfer"></param>
        public CSFMApplication(IntPtr handle, JniHandleOwnership transfer): base(handle, transfer)
        {
            _instance = this;
        }

        /// <summary>
        /// On application creation
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
        }

        /// <summary>
        /// CSFMApplication instance
        /// </summary>
        public static CSFMApplication Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Whether the application is in Debug Mode
        /// </summary>
        public static bool IsDebug
        {
            get { return isDebug;  }
        }

        /// <summary>
        /// BearerToken key used in CSFMPrefs
        /// </summary>
        public static string BearerToken
        {
            get { return bearerToken; }
        }

        /// <summary>
        ///  Username key used in CSFMPrefs
        /// </summary>
        public static string Username
        {
            get { return username; }
        }

        /// <summary>
        /// IsScrobbling key used in CSFMPrefs
        /// </summary>
        public static string IsScrobbling
        {
            get { return isScrobbling; }
        }
    }
}