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

namespace csfm_android.Api
{
    public sealed class ApiClient
    {
        private static readonly string SERVER_URL = "";
        private static readonly ApiClient instance = new ApiClient();
        private static readonly Uri endpoint = new Uri(SERVER_URL);

        public static ApiClient Instance
        {
            get
            {
                return instance;
            }
        }

        private ApiClient()
        {
        }

    }
}