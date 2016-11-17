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
using csfm_android.Api.Model;
using csfm_android.Ui.Holders;
using csfm_android.Ui.Utils;
using Square.Picasso;

namespace csfm_android.Ui.Adapters
{
    /// <summary>
    /// MyMatchFragment RecyclerView's adapter
    /// </summary>
    class MyMatchAdapter : RecyclerView.Adapter
    {
        private List<User> users;

        private Context context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="users">Items to display</param>
        public MyMatchAdapter(Context context, List<User> users)
        {
            this.context = context;
            this.users = users;
        }

        /// <summary>
        /// Number of elements
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return users != null ? users.Count : 0;
            }
        }

        /// <summary>
        /// On item to bind to a view
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position < ItemCount)
            {
                User user = users[position];

                if (user != null)
                {
                    MyMatchHolder matchHolder = holder as MyMatchHolder;

                    matchHolder.Username.Text = user.Username;
                    matchHolder.Email.Text = user.Email;

                    Picasso.With(context)
                       .Load(user.Photo)
                       .Transform(new CircleTransform())
                       .Into(matchHolder.UserPicture);
                }
            }
        }

        /// <summary>
        /// On item view creation
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.my_match_item, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            MyMatchHolder holder = new MyMatchHolder(itemView);
            return holder;
        }
    }
}