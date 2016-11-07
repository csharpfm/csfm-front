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

        private static int MAX_ITEMS = 50;
        public static List<string> SUGGESTIONS { get; private set; }
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

        private static void TryInitSuggestions()
        {
            if (SUGGESTIONS == null)
            {
                SUGGESTIONS = new List<string>();
            }
        }

        private static void ReformatSuggestions()
        {
            SUGGESTIONS = SUGGESTIONS.GroupBy(s => s.ToLower()).OrderByDescending(x => x.Count()).Select(x => x.First().ToLower()).Take(MAX_ITEMS).ToList();

        }

        public static void AddSuggestions(IEnumerable<string> suggestions)
        {

            TryInitSuggestions();
            lock (SUGGESTIONS)
            {
                if (suggestions != null && suggestions != SUGGESTIONS)
                {
                    SUGGESTIONS.AddRange(suggestions.Where(s => !string.IsNullOrWhiteSpace(s)));
                }
                ReformatSuggestions();
            }
        }

        public static void AddSuggestion(string suggestion)
        {
            TryInitSuggestions();
            if (!string.IsNullOrWhiteSpace(suggestion))
            {
                SUGGESTIONS.Add(suggestion);
            }
            ReformatSuggestions();
        }

        public SearchAdapter(Context context, IEnumerable<string> suggestions)
        {
            inflater = LayoutInflater.From(context);
            data = new List<string>();

            AddSuggestions(suggestions);
        }

        public SearchAdapter(Context context, IEnumerable<string> suggestions, Drawable suggestionIcon, bool ellipsize)
        {
            inflater = LayoutInflater.From(context);
            data = new List<string>();
            AddSuggestions(suggestions);
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

            private IEnumerable<string> Search(string query)
            {
                // Retrieve the autocomplete results.
                IEnumerable<string> words = query.Trim().ToLower().Split(' ');
                IEnumerable<string> result = SUGGESTIONS.Where(s => words.Any(w => s.Contains(w))).Select(s => s.ToFirstUpperCases());

                return result;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                FilterResults filterResults = new FilterResults();
                if (!string.IsNullOrWhiteSpace(constraint.ToString()))
                {
                    IEnumerable<string> searchData = Search(constraint.ToString());
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