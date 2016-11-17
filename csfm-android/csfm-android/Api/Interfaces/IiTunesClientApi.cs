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
using csfm_android.Utils.iTunes;
using System.Threading.Tasks;

namespace csfm_android.Api.Interfaces
{
    /// <summary>
    /// Interface to the iTunes API
    /// </summary>
    public interface IiTunesClientApi
    {
        /// <summary>
        /// HTTP GET iTunes API Request to get track information
        /// </summary>
        /// <param name="keywords">The keywords to search for. Every keyword should be separated with a '+'. No space is allowed</param>
        /// <returns>A list of iTunes items</returns>
        [Get("/search?term={keywords}")]
        Task<ITunesResponse> Search(string keywords);
    }
}