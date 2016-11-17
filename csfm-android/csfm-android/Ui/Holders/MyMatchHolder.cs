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
using Android.Support.V7.Widget;

namespace csfm_android.Ui.Holders
{
    /// <summary>
    /// MyMatch RecyclerView Item holder
    /// </summary>
    class MyMatchHolder : RecyclerView.ViewHolder
    {
        public TextView Username { get; private set; }

        public TextView Email { get; private set; }

        public ImageView UserPicture { get; private set; }

   
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemView"></param>
        public MyMatchHolder(View itemView) : base (itemView)
        {
            Username = itemView.FindViewById<TextView>(Resource.Id.my_match_name);
            Email = itemView.FindViewById<TextView>(Resource.Id.my_match_email);
            UserPicture = itemView.FindViewById<ImageView>(Resource.Id.my_match_image);
        }
    }
}