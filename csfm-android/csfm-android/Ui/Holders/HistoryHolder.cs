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
using Android.Views.Animations;
using csfm_android.Api.Model;
using Square.Picasso;
using csfm_android.Ui.Utils;
using Java.IO;
using csfm_android.Api;

namespace csfm_android.Ui.Holders
{
    /// <summary>
    /// History view holder
    /// </summary>
    public class HistoryHolder : RecyclerView.ViewHolder
    {
        public const int LAYOUT = Resource.Layout.history_item;

        private History history_;

        public TextView SongName { get; private set; }

        public TextView SongArtist { get; private set; }

        public ImageView AlbumCover { get; private set; }
        public TextView Date { get; private set; }

        /// <summary>
        /// Loads the image url into the AlbumCover ImageView, or the placeholder on null
        /// </summary>
        public string AlbumCoverUrl
        {
            set
            {
                if (value != null)
                    Picasso.With(Application.Context)
                           .Load(value)
                           .Transform(new CircleTransform())
                           .Placeholder(Resource.Drawable.ic_music_circle_grey600_24dp)
                           .Into(AlbumCover);
                else
                    Picasso.With(Application.Context)
                        .Load(Resource.Drawable.ic_music_circle_grey600_24dp)
                        .Transform(new CircleTransform())
                        .Into(AlbumCover);
            }
        }

        /// <summary>
        /// Loads the image file into the AlbumCover ImageView, or the placeholder on null
        /// </summary>
        public string AlbumCoverFile
        {
            set
            {
                try
                {
                    Picasso.With(Application.Context)
                        .Load(new File(value))
                        .Transform(new CircleTransform())
                        .Into(AlbumCover);
                }
                catch
                {
                    //Placeholder
                    Picasso.With(Application.Context)
                        .Load(Resource.Drawable.ic_music_circle_grey600_24dp)
                        .Transform(new CircleTransform())
                        .Into(AlbumCover);
                }

            }
        }

        /// <summary>
        /// Rotates the image view on true
        /// </summary>
        public bool Animation
        {
            set
            {
                if (value)
                {
                    AlbumCover.StartAnimation(AnimationUtils.LoadAnimation(Application.Context, Resource.Animation.rotate));
                }
                else
                {
                    AlbumCover.ClearAnimation();
                }
            }
        }

        /// <summary>
        /// Constructor (on view creation)
        /// </summary>
        /// <param name="itemView"></param>
        public HistoryHolder(View itemView) : base (itemView)
        {
            SongName = itemView.FindViewById<TextView>(Resource.Id.history_song_name);
            SongArtist = itemView.FindViewById<TextView>(Resource.Id.history_artist_name);
            AlbumCover = itemView.FindViewById<ImageView>(Resource.Id.history_image);
            Date = itemView.FindViewById<TextView>(Resource.Id.history_date);
        }

        /// <summary>
        /// Bind an item to the view 
        /// </summary>
        /// <param name="history"></param>
        public void Bind(History history)
        {
            this.history_ = history;
            SongName.Text = history?.Track?.Name;
            SongArtist.Text = history?.Track?.Artist_Album_Format;

            if (!history.IsScrobbling)
            {
                Date.Text = history?.ListenDateFormat;
                string url = history?.Image;
                AlbumCoverUrl = url;
                if (url == null)
                {
                    //Get an album art from iTunes on null
                    new ApiClient().GetAlbumArtUrl(history, s =>
                    {
                        if (this.history_ == history && s != null)
                        {
                            AlbumCoverUrl = s;
                        }
                        history.Track.Album.Image = s;
                    });
                }
            }
            else
            {
                Date.Text = "";
                AlbumCoverFile = history?.Track?.Album?.Image;
            }
            Animation = history.IsScrobbling;


        }

        /// <summary>
        /// Bind a dummy element with "No Result" label
        /// </summary>
        public void BindNoResult()
        {
            SongName.Text = "";
            SongArtist.Text = Application.Context.Resources.GetString(Resource.String.no_data_available);
            Date.Text = "";
            AlbumCoverUrl = null;
            Animation = false;
        }
    }
}