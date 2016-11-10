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
using Square.Picasso;
using csfm_android.Ui.Utils;
using csfm_android.Utils.MaterialDesignSearchView;
using csfm_android.Utils;
using csfm_android.Receivers;

namespace csfm_android.Ui.Adapters
{
    public class HistoryAdapter : RecyclerView.Adapter
    {
        private Context context;

        private History scrobble;
        public History Scrobble
        {
            get { return scrobble; }
            set
            {
                scrobble = value;
                NotifyDataSetChanged();
            }
        }

        private List<History> data;
        public List<History> Data
        {
            get { return data; }
            set
            {
                data = value;
                NotifyDataSetChanged();
            }
        }

        public override int ItemCount
        {
            get
            {
                return Data != null ? Data.Count + (Scrobble != null ? 1 : 0) : 0;
            }
        }

        private History this[int index]
        {
            get
            {
                if (scrobble == null) return Data[index];

                //Scrobble != null
                if (index == 0)
                {
                    return scrobble;
                }
                else
                {
                    return Data[index - 1];
                }

                
            }
        }

        public HistoryAdapter(Context context, List<History> history)
        {
            this.context = context;
            this.data = history;
            LocalMusicPlayingReceiver.Register(context, this);
            MaterialSearchView.AddSuggestions(Data.ToTrackNames());
            MaterialSearchView.AddSuggestions(Data.ToAlbumNames());
            MaterialSearchView.AddSuggestions(Data.ToArtistNames());
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.history_item, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            HistoryHolder holder = new HistoryHolder(itemView);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position < ItemCount)
            {
                History history = this[position];

                if (history != null)
                {
                    (holder as HistoryHolder)?.Bind(history);
                }
            }
        }




        //private History scrobble;
        //public History Scrobble
        //{
        //    get { return scrobble; }
        //    set {
        //        SetData(Data, value);
        //    }
        //}

        //public HistoryAdapter(Context context, List<History> history) : base(context, history)
        //{
        //    LocalMusicPlayingReceiver receiver = new LocalMusicPlayingReceiver(this);
        //    IntentFilter iF = new IntentFilter();
        //    //[IntentFilter(new[] { "com.android.music.metachanged", "com.android.music.playstatechanged", "com.android.music.playbackcomplete", "com.android.music.queuechanged" })]
        //    iF.AddActions("com.android.music.metachanged", "com.android.music.playstatechanged", "com.android.music.playbackcomplete", "com.android.music.queuechanged");
        //    context.RegisterReceiver(receiver, iF);
        //    MaterialSearchView.AddSuggestions(Data.ToTrackNames());
        //    MaterialSearchView.AddSuggestions(Data.ToAlbumNames());
        //    MaterialSearchView.AddSuggestions(Data.ToArtistNames());
        //}

        //public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        //{
        //    if (position < ItemCount)
        //    {
        //        History history = Data[position];

        //        if (history != null)
        //        {
        //            (holder as HistoryHolder)?.Bind(history);
        //        }
        //    }
        //}

        //public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        //{
        //    // Inflate the CardView for the photo:
        //    View itemView = LayoutInflater.From(parent.Context).
        //                Inflate(Resource.Layout.history_item, parent, false);

        //    // Create a ViewHolder to hold view references inside the CardView:
        //    HistoryHolder holder = new HistoryHolder(itemView);
        //    return holder;
        //}

        //public void AddScrobble(History scrobble)
        //{
        //    SetData(Data, scrobble);
        //}

        //public void SetData(IEnumerable<History> history, History scrobble)
        //{
        //    Action action;
        //    if (Data != history)
        //    {
        //        action = () => NotifyDataSetChanged();
        //    }
        //    else
        //    {
        //        if (Data == history && scrobble != null && Data.Any(h => !h.IsScrobbling))
        //        {
        //            action = () => NotifyDataSetChanged();
        //        }
        //        else
        //        {
        //            action = () => NotifyDataSetChanged();
        //        }
        //    }

        //    List<History> data = history.Where(h => !h.IsScrobbling).ToList();
        //    if (scrobble != null)
        //    {
        //        data.Insert(0, scrobble);
        //    }
        //    this.Data = data; //base.SetData(data, action);
        //}

    }
}