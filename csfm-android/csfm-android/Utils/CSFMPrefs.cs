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
using Android.Preferences;

namespace csfm_android.Utils
{
    public class CSFMPrefs
    {
        public static ISharedPreferences Prefs
        {
            get
            {
                return PreferenceManager.GetDefaultSharedPreferences(CSFMApplication.Instance.ApplicationContext);
            }
        }

        public static ISharedPreferencesEditor Editor
        {
            get
            {
                return Prefs.Edit();
            }
        }
    }
}