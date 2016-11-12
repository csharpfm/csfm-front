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
using System.IO;

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
        [Get("/api/users/{username}")]
        Task<string> GetUser(string username, [Header("Authorization")] string accessToken);

        [Get("/api/users/{username}/history")]
        Task<string> GetUserHistory(string username, [Header("Authorization")] string accessToken);

        [Delete("/api/users/{username}/history/{id}")]
        Task<string> DeleteUserHistory(string username, int id, [Header("Authorization")] string accessToken);

        [Get("/api/users/modify/password/{password}")]
        Task<string> ModifyPassword(string password, [Header("Authorization")] string accessToken);

        
        [Post("/api/Users/{username}/Photo")]
        [Multipart]
        [Headers("Content-Type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW")]
        Task<string> UploadPhoto(string username, [AttachmentName("Cat_158")] byte[] aFile, [Header("Authorization")] string accessToken);

        /* Match */   
    }
}

