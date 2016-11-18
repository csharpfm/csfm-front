// ***********************************************************************
// Assembly         : csfm-android
// Author           : Pierre Defache
// Created          : 11-07-2016
//
// Last Modified By : Pierre Defache
// Last Modified On : 11-11-2016
// ***********************************************************************
// <copyright file="ApiClient.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using csfm_android.Api.Interfaces;
using csfm_android.Utils;
using Newtonsoft.Json.Linq;
using csfm_android.Api.Model;
using System.Threading.Tasks;
using Java.Util;
using Newtonsoft.Json;
using csfm_android.Ui.Holders;
using csfm_android.Utils.iTunes;
using System.Text.RegularExpressions;

namespace csfm_android.Api
{
    /// <summary>
    /// Class ApiClient.
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// The server URL
        /// </summary>
        private static readonly string SERVER_URL = "http://matchfm.westeurope.cloudapp.azure.com";

        /// <summary>
        /// The iTunes URL
        /// </summary>
        private const string ITUNES_URL = "https://itunes.apple.com";

        /// <summary>
        /// The MatchFM API Client
        /// </summary>
        private static readonly ICsfmApi instance = RestService.For<ICsfmApi>(SERVER_URL);

        /// <summary>
        /// The iTunes API Client
        /// </summary>
        private static readonly IiTunesClientApi iTunes = RestService.For<IiTunesClientApi>(ITUNES_URL);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        public ApiClient()
        {
        }

        #region MatchFM

        public string BearerFormat
        {
            get
            {
                return "Bearer " + RetrieveBearer();
            }
        }

        /// <summary>
        /// Retrieves the bearer.
        /// </summary>
        /// <returns>System.String.</returns>
        public string RetrieveBearer()
        {
            return CSFMPrefs.Bearer;
        }

        /// <summary>
        /// Retrieves the username.
        /// </summary>
        /// <returns>System.String.</returns>
        public string RetrieveUsername()
        {
            return CSFMPrefs.Username;
        }
        /// <summary>
        /// Log into MatchFM account
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="successCallback">Callback to invoke in case of success</param>
        /// <param name="errorCallback">Callback to invoke in case of error</param>
        /// <returns>True on login successful</returns>
        public async Task<bool> LogIn(string username, string password, Action successCallback, Action errorCallback)
        {
            Dictionary<string, object> informations = new Dictionary<string, object>()
            {
                { "grant_type", "password" },
                { "username", username },
                { "password", password }
            };

            try
            {
                string response = await instance.SignIn(informations);
                string token = JObject.Parse(response)["access_token"].ToString();
                CSFMPrefs.Bearer = token;
                CSFMPrefs.Username = username;
                successCallback?.Invoke();
                return true;
            }
            catch (Refit.ApiException e)
            {
                //Error
            }
            errorCallback?.Invoke();
            return false;
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="successCallback">Callback to invoke in case of success</param>
        /// <param name="errorCallback">Callback to invoke in case of error</param>
        /// <returns>True on account creation success</returns>
        public async Task<bool> SignUp(string email, string username, string password, Action successCallback, Action errorCallback)
        {
            Dictionary<string, object> informations = new Dictionary<string, object>()
            {
                {"Username", username },
                {"Email", email },
                {"Password", password }
            };

            try
            {
                string status = await instance.SignUp(informations);
                if (string.IsNullOrEmpty(status))
                {
                    successCallback?.Invoke();
                    return true;
                }
            }
            catch (Refit.ApiException e)
            {
                //Error
            }

            errorCallback?.Invoke();
            return false;
        }

        /// <summary>
        /// Gets the history.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Task&lt;List&lt;History&gt;&gt;.</returns>
        public async Task<List<History>> GetHistory(string username)
        {
            try
            {
                return await instance.GetUserHistory(username);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Post a new history item (new scrobble)
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="history">The history item.</param>
        public async void PostHistory(string username, History history, Action callback = null)
        {
            PostHistory(username, history.Track.Album.Artist.Name, history.Track.Album.Name, history.Track.Name, callback);
        }

        /// <summary>
        /// Posts a new history item
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="artist">The artist</param>
        /// <param name="album">The album</param>
        /// <param name="title">The title</param>
        /// <param name="successCallback">Callback to invoke in case of success</param>
        public async void PostHistory(string username, string artist, string album, string title, Action successCallback = null)
        {
            try
            {
                var data = new Dictionary<string, object>
                {
                    { "Artist", artist },
                    { "Album", album },
                    { "Title", title }
                };

                await instance.PostUserHistory(username, data, this.BearerFormat);
                successCallback?.Invoke();
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>User instance</returns>
        public async Task<User> GetUser(string username)
        {
            try
            {
                return await instance.GetUser(username, this.BearerFormat);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates the user location on server
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>True on success</returns>
        public async Task<bool> PutUserLocation(string username, double latitude, double longitude)
        {
            try
            {
                var data = new Dictionary<String, double>
                {
                    { "latitude", latitude },
                    { "longitude", longitude },
                };

                await instance.PutUserLocation(username, data, this.BearerFormat);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Imports the last fm account
        /// </summary>
        /// <param name="lastfmUsername">The lastfm username.</param>
        public async void ImportLastFm(string lastfmUsername)
        {
            await instance.LinkLastFMAccount(lastfmUsername, this.BearerFormat);
        }

        /// <summary>
        /// Get the list of matched users
        /// </summary>
        /// <param name="username">User to get the matched users from</param>
        /// <returns></returns>
        public async Task<List<User>> GetUserMatch(string username)
        {
            try
            {
                return await instance.GetUserMatch(username, this.BearerFormat);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Match or dismatch a new user
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="profileId">Matched or dismatched user id</param>
        /// <param name="isMatch">True to match the user, false to dismatch</param>
        /// <returns>true on success</returns>
        public async Task<bool> PutUserMatch(string username, string profileId, bool isMatch)
        {
            try
            {
                var data = new Dictionary<String, object>
                {
                    { "ProfilId", profileId },
                    { "Match", isMatch },
                };

                await instance.PutUserMatch(username, data, this.BearerFormat);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the recommended users (to potentially match with)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUserRecommendations(string username)
        {
            try
            {
                return await instance.GetUserRecommendations(username, this.BearerFormat);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a user top 10 artists
        /// </summary>
        /// <param name="username">The username to get the top artists froms</param>
        /// <returns></returns>
        public async Task<List<Artist>> GetUserTopArtists(string username)
        {
            try
            {
                return await instance.GetUserTopArtists(username, this.BearerFormat);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion MatchFM

        #region iTunes

        //Key : {artist}+{album} (replace " " with "+")
        private static Dictionary<string, string> images = new Dictionary<string, string>();

        /// <summary>
        /// Get the album art url of a certain track
        /// </summary>
        /// <param name="history">The history item (containing album and artist names) to get the album cover from</param>
        /// <param name="callback">On success, invoke this callback with the album url as parameter</param>
        /// <returns></returns>
        public async Task<string> GetAlbumArtUrl(History history, Action<string> callback)
        {
            string keywords = history.Track.Album.Name + "+" + history.Track.Album.Artist.Name;
            keywords = new Regex("[ \\+][ \\+]*").Replace(keywords, "+");

            string url = null;
            if (images.ContainsKey(keywords))
            {
                url = images[keywords];
            }
            else
            {
                ITunesResponse response = await iTunes.Search(keywords);
                url = response.Items.FirstOrDefault(i => i.AlbumArtUrl != null)?.AlbumArtUrl;
                images[keywords] = url;
            }
            callback(url);
            return url;
        }


        #endregion iTunes
    }

    
}
 
 