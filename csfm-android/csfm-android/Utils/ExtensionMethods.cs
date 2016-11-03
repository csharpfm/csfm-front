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

namespace csfm_android.Utils
{
    public static class ExtensionMethods
    {
        public static bool IsStringEmpty(this String text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static string DefaultStringIfEmpty(this String text, string defaultText)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return defaultText;
            }
            return text;
        }



    }
}