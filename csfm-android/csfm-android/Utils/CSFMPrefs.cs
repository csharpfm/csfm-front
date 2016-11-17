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

        /// <summary>
        /// Provides the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Provide(string key, string value)
        {
            CSFMPrefs.Editor.PutString(key, value).Commit();
        }

        public static string Bearer
        {
            get
            {
                return CSFMPrefs.Prefs.GetString(CSFMApplication.BearerToken, "");
            }

            set
            {
                Provide(CSFMApplication.BearerToken, value);
            }
        }

        public static string Username
        {
            get
            {
                return CSFMPrefs.Prefs.GetString(CSFMApplication.Username, "");
            }

            set
            {
                Provide(CSFMApplication.Username, value);
            }
        }
    }
}