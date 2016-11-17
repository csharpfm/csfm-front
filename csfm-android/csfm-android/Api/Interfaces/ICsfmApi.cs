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
    //Interface to the MatchFM API
    public interface ICsfmApi
    {
        #region Authentication API

        /// <summary>
        /// HTTP POST API Request to sign in
        /// </summary>
        /// <param name="data">Dictionary containing keys "grant_type", "username", and "password"</param>
        /// <returns>JSON Object</returns>
        [Post("/oauth/token")]
        Task<string> SignIn([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        /// <summary>
        /// HTTP POST API Request to create a new account
        /// </summary>
        /// <param name="data">Dictionary containing keys "Username", "Email" and "Password"</param>
        /// <returns>Empty string on success</returns>
        [Post("/api/Account/Register")]
        Task<string> SignUp([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        #endregion

        #region Users API

        /// <summary>
        /// API Request to get the user information
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Get("/api/Users/{username}")]
        Task<User> GetUser(string username, [Header("Authorization")] string accessToken);

        /// <summary>
        /// HTTP GET API Request to get the user history. This request doesn't need an access token.
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns></returns>
        [Get("/api/Users/{username}/History")]
        Task<List<History>> GetUserHistory(string username);

        /// <summary>
        /// HTTP POST API Request to add a new song to the user's history (scrobble)
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="history">Dictionary containing the keys "Artist", "Album" and "Title"</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Post("/api/Users/{username}/History")]
        Task PostUserHistory(string username, [Body(BodySerializationMethod.Json)]  Dictionary<string, object> history, [Header("Authorization")] string accessToken);

        /// <summary>
        /// HTTP PUT API Request to update user's location server-side.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="location">The location</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Put("/api/Users/{username}/Location")]
        Task PutUserLocation(string username, [Body(BodySerializationMethod.Json)]  Dictionary<string, double> location, [Header("Authorization")] string accessToken);

        /// <summary>
        /// HTTP GET API Request to get the favorite 10 artists of a user
        /// </summary>
        /// <param name="username">The username to get the top artists from</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Get("/api/Users/{username}/TopArtists")]
        Task<List<Artist>> GetUserTopArtists(string username, [Header("Authorization")] string accessToken);

        #endregion

        #region Match

        /// <summary>
        /// HTTP GET API Request to get the list of matched users with the specified username.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Get("/api/Users/{username}/Match")]
        Task<List<User>> GetUserMatch(string username, [Header("Authorization")] string accessToken);

        /// <summary>
        /// HTTP PUT API Request to match or dismatch with a user
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="data">Dictionary containing the keys "ProfilId" (user id to match with) and "isMatch" (value : either to accept or decline the recommendation)</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Put("/api/Users/{username}/Match")]
        Task PutUserMatch(string username, [Body(BodySerializationMethod.Json)]  Dictionary<string, object> data, [Header("Authorization")] string accessToken);
        #endregion

        #region Recommendations

        /// <summary>
        /// HTTP GET API Request to get a list of recommended users the user could potentially want to match with.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [Get("/api/Users/{username}/Recommendations")]
        Task<List<User>> GetUserRecommendations(string username, [Header("Authorization")] string accessToken);

        #endregion

        #region Last.fm

        /// <summary>
        /// Import your last.fm account into MatchFM
        /// </summary>
        /// <param name="username">Last.fm username</param>
        /// <param name="accessToken">The access token (retrieved on login)</param>
        /// <returns></returns>
        [Post("/api/Users/{username}/LastFMImport")]
        Task LinkLastFMAccount(string username, [Header("Authorization")] string accessToken);

        #endregion
    }
}