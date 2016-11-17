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
    /// <summary>
    /// SharedPreferences Manager : Used to save and load values in the internal memory.
    /// </summary>
    public class CSFMPrefs
    {
        /// <summary>
        /// Get the Default Shared Preferences Manager
        /// </summary>
        public static ISharedPreferences Prefs
        {
            get
            {
                return PreferenceManager.GetDefaultSharedPreferences(CSFMApplication.Instance.ApplicationContext);
            }
        }

        /// <summary>
        /// Get the Preferences Editor
        /// </summary>
        public static ISharedPreferencesEditor Editor
        {
            get
            {
                return Prefs.Edit();
            }
        }

        /// <summary>
        /// Save the (key, value) into the Shared preferences
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Provide(string key, string value)
        {
            CSFMPrefs.Editor.PutString(key, value).Commit();
        }

        /// <summary>
        /// Get or set the Bearer from/in the SharedPreferences
        /// </summary>
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

        /// <summary>
        /// Get or set the Bearer from/in the SharedPreferences
        /// </summary>
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

        /// <summary>
        /// Get or set the IsScrobbling value from/in the SharedPreferences. Specifies whether scrobbling feature is enabled or disabled on the application
        /// </summary>
        public static bool IsScrobbling
        {
            set
            {
                CSFMPrefs.Editor.PutBoolean(CSFMApplication.IsScrobbling, value).Commit();
            }

            get
            {
                return CSFMPrefs.Prefs.GetBoolean(CSFMApplication.IsScrobbling, true);
            }
        }
    }
}