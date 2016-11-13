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

        private const string ITUNES_URL = "https://itunes.apple.com";

        /// <summary>
        /// The instance
        /// </summary>
        private static readonly ICsfmApi instance = RestService.For<ICsfmApi>(SERVER_URL);

        private static readonly IiTunesClientApi iTunes = RestService.For<IiTunesClientApi>(ITUNES_URL);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        public ApiClient()
        {
        }


        /// <summary>
        /// Retrieves the bearer.
        /// </summary>
        /// <returns>System.String.</returns>
        public string RetrieveBearer()
        {
            return CSFMPrefs.Prefs.GetString(CSFMApplication.BearerToken, "");
        }

        /// <summary>
        /// Retrieves the username.
        /// </summary>
        /// <returns>System.String.</returns>
        public string RetrieveUsername()
        {
            return CSFMPrefs.Prefs.GetString(CSFMApplication.Username, "");
        }

        /// <summary>
        /// Provides the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Provide(string key, string value)
        {
            CSFMPrefs.Editor.PutString(key, value).Commit();
        }

        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> LogIn(string username, string password)
        {
            Dictionary<String, object> informations = new Dictionary<String, object>();
            informations.Add("grant_type", "password");
            informations.Add("username", username);
            informations.Add("password", password);

            try
            {
                var response = await instance.SignIn(informations);
                var token = JObject.Parse(response)["access_token"].ToString();
                Provide(CSFMApplication.BearerToken, token);
                Provide(CSFMApplication.Username, username);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> SignUp(string email, string username, string password)
        {
            Dictionary<String, object> informations = new Dictionary<String, object>();
            informations.Add("Username", username);
            informations.Add("Email", email);
            informations.Add("Password", password);

            try
            {
                var status = await instance.SignUp(informations);

                if (String.IsNullOrEmpty(status))
                {
                    return true;
                }
            }
            catch (Refit.ApiException e)
            {
                return false;
            }

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
        /// Posts the history.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="history">The history.</param>
        public async void PostHistory(string username, History history, Action callback = null)
        {
            PostHistory(username, history.Track.Album.Artist.Name, history.Track.Album.Name, history.Track.Name, callback);
        }

        /// <summary>
        /// Posts the history
        /// </summary>
        /// <param name="username"></param>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="title"></param>
        public async void PostHistory(string username, string artist, string album, string title, Action callback = null)
        {
            try
            {
                var data = new Dictionary<string, object>
                {
                    { "Artist", artist },
                    { "Album", album },
                    { "Title", title }
                };

                await instance.PostUserHistory(username, data, "Bearer " + this.RetrieveBearer());

                if (callback != null)
                    callback();
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
        /// <returns>Task&lt;User&gt;.</returns>
        public async Task<User> GetUser(string username)
        {
            try
            {
                return await instance.GetUser(username, "Bearer " + this.RetrieveBearer());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Puts the user location.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> PutUserLocation(string username, double latitude, double longitude)
        {
            try
            {
                var data = new Dictionary<String, double>
                {
                    { "latitude", latitude },
                    { "longitude", longitude },
                };

                await instance.PutUserLocation(username, data, "Bearer " + this.RetrieveBearer());

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Imports the last fm.
        /// </summary>
        /// <param name="lastfmUsername">The lastfm username.</param>
        public async void ImportLastFm(string lastfmUsername)
        {
            await instance.LinkLastFMAccount(lastfmUsername, "Bearer " + this.RetrieveBearer());
        }


        public async Task<List<User>> GetUserMatch(string username)
        {
            try
            {
                return await instance.GetUserMatch(username, "Bearer " + this.RetrieveBearer());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> PutUserMatch(string username, string profileId, bool isMatch)
        {
            try
            {
                var data = new Dictionary<String, object>
                {
                    { "ProfilId", profileId },
                    { "Match", isMatch },
                };

                await instance.PutUserMatch(username, data, "Bearer " + this.RetrieveBearer());

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Key : {artist}+{album} (replace " " with "+")
        private static Dictionary<string, string> images = new Dictionary<string, string>();

        public async Task<string> GetAlbumArtUrl(History history, Action<string> callback)
        {
            string keywords = history.Track.Album.Name.Replace(' ', '+');
            keywords += "+" + history.Track.Album.Artist.Name.Replace(' ', '+');

            string url = null;
            if (images.ContainsKey(keywords))
            {
                url = images[keywords];
            }
            else
            {
                ITunesResponse response = await iTunes.Search(keywords);
                url = response.Items.FirstOrDefault(i => i.AlbumArtUrl != null)?.AlbumArtUrl;
            }
            callback(url);
            return url;
        }
    }

    
}
 
 