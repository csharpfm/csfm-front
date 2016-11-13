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
    public interface IiTunesClientApi
    {
        [Get("/search?term={keywords}")]
        Task<ITunesResponse> Search(string keywords);
    }
}