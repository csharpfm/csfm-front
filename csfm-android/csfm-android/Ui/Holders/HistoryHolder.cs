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

namespace csfm_android.Ui.Holders
{
    public class HistoryHolder : RecyclerView.ViewHolder
    {

        public TextView SongName { get; private set; }

        public TextView SongArtist { get; private set; }

        public ImageView AlbumCover { get; private set; }

        public string AlbumCoverUrl
        {
            set
            {
                Picasso.With(Application.Context)
                       .Load(value)
                       .Transform(new CircleTransform())
                       .Placeholder(Resource.Drawable.ic_music_circle_grey600_24dp)
                       .Into(AlbumCover);
            }
        }

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

        public TextView Date { get; private set; }

        public HistoryHolder(View itemView) : base (itemView)
        {
            SongName = itemView.FindViewById<TextView>(Resource.Id.history_song_name);
            SongArtist = itemView.FindViewById<TextView>(Resource.Id.history_artist_name);
            AlbumCover = itemView.FindViewById<ImageView>(Resource.Id.history_image);
            Date = itemView.FindViewById<TextView>(Resource.Id.history_date);
        }

        public void Bind(History history)
        {
            SongName.Text = history.Track.Name;
            SongArtist.Text = history.Track.Album.Artist.Name;

            if (!history.IsScrobbling)
            {
                Date.Text = history.ListenDate.ToString();
                AlbumCoverUrl = history.Track.Album.Image;
            }
            else
            {
                Date.Text = "";
                AlbumCoverFile = history.Track.Album.Image;
            }
            Animation = history.IsScrobbling;

        }
    }
}