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
using Android.Graphics.Drawables;
using Java.Lang;
using Android.Text;

namespace csfm_android.Utils.MaterialDesignSearchView
{
    public class SearchAdapter : BaseAdapter<string>, IFilterable
    {
        private List<string> data;
        private string[] suggestions;
        private Drawable suggestionIcon;
        private LayoutInflater inflater;
        private bool ellipsize;

        public Filter Filter
        {
            get
            {
                return new SearchAdapterFilter(this);
            }
        }

        public override int Count
        {
            get
            {
                return data.Count;
            }
        }

        public override string this[int position]
        {
            get
            {
                return data[position];
            }
        }

        public SearchAdapter(Context context, string[] suggestions)
        {
            inflater = LayoutInflater.From(context);
            data = new List<string>();
            this.suggestions = suggestions;
        }

        public SearchAdapter(Context context, string[] suggestions, Drawable suggestionIcon, bool ellipsize)
        {
            inflater = LayoutInflater.From(context);
            data = new List<string>();
            this.suggestions = suggestions;
            this.suggestionIcon = suggestionIcon;
            this.ellipsize = ellipsize;
        }

        public class SearchAdapterFilter : Filter
        {
            private SearchAdapter adapter;

            public SearchAdapterFilter(SearchAdapter adapter)
            {
                this.adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                FilterResults filterResults = new FilterResults();
                if (!string.IsNullOrWhiteSpace(constraint.ToString()))
                {

                    // Retrieve the autocomplete results.
                    string searchWord = constraint.ToString().Trim().ToLower();

                    LinkedList<string> searchData = new LinkedList<string>();
                    foreach(var str in adapter.suggestions)
                    {
                        string suggestion = str?.Trim()?.ToLower();
                        if (suggestion.Contains(searchWord) && suggestion != searchWord)
                        {
                            searchData.AddLast(suggestion);
                        }
                    }
                    Java.Lang.Object[] result = searchData.Select(r => r.ToJavaObject()).ToArray();
                    // Assign the data to the FilterResults
                    filterResults.Values = result;
                    filterResults.Count = result.Length;
                }

                constraint.Dispose();

                return filterResults;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                if (results.Values != null)
                {
                    using (var values = results.Values)
                    {
                        adapter.data = values.ToArray<Java.Lang.Object>().Select(r => r.ToNetObject<string>()).ToList();
                        adapter.NotifyDataSetChanged();
                    }
                }

                results.Dispose();
                constraint.Dispose();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return data[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            SuggestionsViewHolder viewHolder;

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.material_search_view_suggest_item, parent, false);
                viewHolder = new SuggestionsViewHolder(convertView, suggestionIcon);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = convertView.Tag as SuggestionsViewHolder;
            }

            string currentListData = (string)GetItem(position);

            viewHolder.textView.Text = currentListData;
            if (ellipsize)
            {
                viewHolder.textView.SetSingleLine();
                viewHolder.textView.Ellipsize = TextUtils.TruncateAt.End;
            }

            return convertView;
        }

        private class SuggestionsViewHolder : Java.Lang.Object
        {
            public TextView textView;
            public ImageView imageView;

            public SuggestionsViewHolder(View convertView, Drawable suggestionIcon)
            {
                textView = convertView.FindViewById<TextView>(Resource.Id.material_search_view_suggestion_text);
                if (suggestionIcon != null)
                {
                    imageView = convertView.FindViewById<ImageView>(Resource.Id.material_search_view_suggestion_icon);
                    imageView.SetImageDrawable(suggestionIcon);
                }
            }
        }
    }
}