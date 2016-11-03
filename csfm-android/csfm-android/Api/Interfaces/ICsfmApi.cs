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
using Refit;

namespace csfm_android.Api.Interfaces
{
    public interface ICsfmApi
    {
        [Post("/oauth/token")]
        bool SignIn([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/Account/register")]
        bool SignUp([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}