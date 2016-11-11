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
using System.Threading.Tasks;
using csfm_android.Api.Model;

namespace csfm_android.Api.Interfaces
{
    public interface ICsfmApi
    {
        /* Sign in & Sign up */
        [Post("/oauth/token")]
        Task<string> SignIn([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/Account/Register")]
        Task<string> SignUp([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        /* User API */
        [Get("/api/Users/{username}")]
        Task<User> GetUser(string username, [Header("Authorization")] string accessToken);

        [Get("/api/Users/{username}/History")]
        Task<List<History>> GetUserHistory(string username);

        [Put("/api/Users/{username}/Location")]
        Task PutUserLocation(string username, string location, [Header("Authorization")] string accessToken);

        /* Match */


        /* LastFM */
        [Post("/api/Users/{username}/LastFMImport")]
        Task LinkLastFMAccount(string username, [Header("Authorization")] string accessToken);
    }
}