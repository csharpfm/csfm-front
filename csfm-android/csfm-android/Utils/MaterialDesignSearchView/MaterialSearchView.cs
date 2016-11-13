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
using static Android.Support.V7.Widget.SearchView;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Content.Res;
using static Android.Views.View;
using Android.Speech;
using Android.Content.PM;
using Android.Views.InputMethods;
using Android.Text;
using Java.Lang;
using Java.Lang.Reflect;
using System.Reflection;
using static Android.Support.V7.Widget.ActionMenuView;
using Android.Graphics;
using Android.Support.V7.Widget;
using csfm_android.Api.Model;

namespace csfm_android.Utils.MaterialDesignSearchView
{
    public class MaterialSearchView : FrameLayout, Filter.IFilterListener
    {
        #region const
        public const int REQUEST_VOICE = 9999;
        #endregion const

        #region fields
        //Views
        private IMenuItem menuItem;
        private View searchLayout;
        private View tintView;
        private ListView suggestionsListView;
        private RelativeLayout searchTopBar;
        private EditText searchInput;
        private LinearLayout rightButtonsLayout;

        //Text
        private string oldQueryText;
        private string userQueryText;

        //Adapter
        private IListAdapter mAdapter;

        //Bool
        public bool submit = false;
        private bool ellipsize = false;
        private bool clearingFocus;

        //Saved State
        private MaterialSearchViewSavedState savedState;

        //Context
        private Context context;

        //Listeners
        private MaterialSearchViewOnClickListener clickListener;

        //Buttons
        private ImageButton backButton;
        private ImageButton voiceButton;
        private ImageButton emptyButton;
        private ImageButton searchButton;
        private Drawable suggestionIcon;

        #endregion fields

        #region properties
        /// <summary>
        /// Call this method and pass the menu item so this class can handle click events for the Menu Item.
        /// </summary>
        public IMenuItem MenuItem
        {
            set { this.menuItem = value; menuItem.SetOnMenuItemClickListener(new MaterialSearchViewOnMenuItemClickListener(this)); }
            get { return this.menuItem; }
        }


        public static List<History> History { get; set; }

        /// <summary>
        /// return true if search is open
        /// </summary>
        public bool IsSearchOpen { get; set; }
        public bool IsSetVoiceSearch { get; set; }
        /// <summary>
        /// if show is true, this will enable voice search. If voice is not available on the device, this method call has not effect.
        /// </summary>
        public bool IsShowVoice
        {
            set
            {
                if (value && IsVoiceAvailable && IsSetVoiceSearch)
                {
                    voiceButton.Show();
                }
                else
                {
                    voiceButton.Hide();
                }
            }
        }
        public int AnimationDuration { get; set; }

        private bool IsVoiceAvailable
        {
            get
            {
                if (IsInEditMode)
                {
                    return true;
                }
                PackageManager pm = context.PackageManager;
                IList<ResolveInfo> activities = pm.QueryIntentActivities(new Intent(RecognizerIntent.ActionRecognizeSpeech), 0);
                return activities.Any(); //activites.Count == 0 ?
            }
        }

        /// <summary>
        /// Duration of the open animation
        /// </summary>

        public Drawable SearchTopBarBackground
        {
            set
            {
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
                {
                    searchTopBar.Background = value;
                }
                else
                {
                    searchTopBar.SetBackgroundDrawable(value);
                }
            }
        }

        /// <summary>
        /// Set Adapter for suggestions list. Should implement Filterable.
        /// </summary>
        public IListAdapter Adapter
        {
            set
            {
                mAdapter = value;
                suggestionsListView.Adapter = value;
                StartFilter(searchInput.Text);
            }

            get
            {
                return mAdapter;
            }
        }

        /// <summary>
        /// Set Adapter for suggestions list with the given suggestion array
        /// </summary>
        public List<string> Suggestions
        {
            set
            {
                if (value != null && value.Count > 0)
                {
                    tintView.Show();
                    SearchAdapter adapter = new SearchAdapter(context, value, suggestionIcon, ellipsize);
                    this.Adapter = adapter;
                    this.OnItemClickListener = new MaterialSearchViewAdapterViewOnItemClickListener(this, adapter);
                }
                else
                {
                    tintView.Hide();
                }
            }
        }

        public bool Ellipsize
        {
            get { return ellipsize; }
            set { this.ellipsize = value; }
        }

        /// <summary>
        /// Submit the query as soon as the user clicks the item if true
        /// </summary>
        public bool IsSubmitOnClick
        {
            set
            {
                this.submit = value;
            }
        }

        #endregion

        #region Listener properties

        /// <summary>
        /// Set this listener to listen to Query Change events.
        /// </summary>
        /// <param name="listener"></param>
        public IOnQueryTextListener QueryTextListener { get; set; }
        public ISearchViewListener SearchViewListener { get; set; }

        #endregion Listener properties

        #region Properties Search Source Text View
        public Color TextColor
        {
            set
            {
                searchInput.SetTextColor(value);
            }
        }

        public Color HintTextColor
        {
            set
            {
                searchInput.SetHintTextColor(value);
            }
        }

        public string Hint
        {
            get
            {
                return searchInput.Hint;
            }

            set
            {
                searchInput.Hint = value;
            }
        }

        public int CursorDrawable
        {
            set
            {
                try
                {
                    var prop = typeof(TextView).GetField("mCursorDrawableRes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    prop.SetValue(searchInput, value);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("MaterialSearchView", ex.ToString());
                    //Ignored
                }
            }
        }
        #endregion Search Source Text View

        #region Icons


        public Drawable VoiceIcon { set { voiceButton.SetImageDrawable(value); } }

        public Drawable CloseIcon { set { emptyButton.SetImageDrawable(value); } }

        public Drawable BackIcon { set { backButton.SetImageDrawable(value); } }

        public Drawable SearchIcon { set { searchButton.SetImageDrawable(value); } }

        public Drawable SuggestionIcon { set { suggestionIcon = value; } }

        public Drawable SuggestionBackground
        {
            set
            {
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
                {
                    suggestionsListView.Background = value;
                }
                else
                {
                    suggestionsListView.SetBackgroundDrawable(value);
                }
            }
        }
        #endregion

        #region static

        public static void SetSuggestions(List<History> value)
        {
            SearchAdapter.ClearSuggestions();
            AddSuggestions(value.ToTrackNames());
            AddSuggestions(value.ToAlbumNames());
            AddSuggestions(value.ToArtistNames());
        }

        public static void AddSuggestions(IEnumerable<string> suggestions)
        {
            SearchAdapter.AddSuggestions(suggestions);
        }

        public static void AddSuggestion(string suggestion)
        {
            SearchAdapter.AddSuggestion(suggestion);
        }

        #endregion static

        #region ctors
        public MaterialSearchView(Context context) : this(context, null)
        {
        }

        public MaterialSearchView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public MaterialSearchView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs)
        {
            this.context = context;
            clickListener = new MaterialSearchViewOnClickListener(this);
            InitiateView();
            InitStyle(attrs, defStyleAttr);

            attrs.Dispose();
        }
        #endregion ctors

        #region init
        private void InitStyle(IAttributeSet attrs, int defStyleAttr)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.MaterialSearchView, defStyleAttr, 0);

            if (a != null)
            {
                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchBackground))
                {
                    Background = a.GetDrawable(Resource.Styleable.MaterialSearchView_searchBackground);
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_android_textColor))
                {
                    TextColor = a.GetColor(Resource.Styleable.MaterialSearchView_android_textColor, 0);
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_android_textColorHint))
                {
                    HintTextColor = (a.GetColor(Resource.Styleable.MaterialSearchView_android_textColorHint, 0));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_android_hint))
                {
                    Hint = (a.GetString(Resource.Styleable.MaterialSearchView_android_hint));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchVoiceIcon))
                {
                    VoiceIcon = (a.GetDrawable(Resource.Styleable.MaterialSearchView_searchVoiceIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchCloseIcon))
                {
                    CloseIcon = (a.GetDrawable(Resource.Styleable.MaterialSearchView_searchCloseIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchBackIcon))
                {
                    BackIcon = (a.GetDrawable(Resource.Styleable.MaterialSearchView_searchBackIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchSearchIcon))
                {
                    SearchIcon = (a.GetDrawable(Resource.Styleable.MaterialSearchView_searchSearchIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchSuggestionBackground))
                {
                    SuggestionBackground = (a.GetDrawable(Resource.Styleable.MaterialSearchView_searchSuggestionBackground));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchSuggestionIcon))
                {
                    SuggestionIcon = (a.GetDrawable(Resource.Styleable.MaterialSearchView_searchSuggestionIcon));
                }

                a.Recycle();
                a.Dispose();
            }
        }

        private void InitiateView()
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.material_search_view, this, true);
            searchLayout = FindViewById<View>(Resource.Id.search_layout);

            searchTopBar = searchLayout.FindViewById<RelativeLayout>(Resource.Id.search_top_bar);
            suggestionsListView = searchLayout.FindViewById<ListView>(Resource.Id.suggestion_list);
            searchInput = searchLayout.FindViewById<EditText>(Resource.Id.searchTextView);
            backButton = searchLayout.FindViewById<ImageButton>(Resource.Id.action_up_btn);
            voiceButton = searchLayout.FindViewById<ImageButton>(Resource.Id.action_voice_btn);
            emptyButton = searchLayout.FindViewById<ImageButton>(Resource.Id.action_empty_btn);
            tintView = searchLayout.FindViewById(Resource.Id.transparent_view);
            searchButton = searchLayout.FindViewById<ImageButton>(Resource.Id.action_search_btn);
            rightButtonsLayout = searchLayout.FindViewById<LinearLayout>(Resource.Id.action_empty_btn_layout);

            searchInput.SetOnClickListener(clickListener);
            backButton.SetOnClickListener(clickListener);
            voiceButton.SetOnClickListener(clickListener);
            emptyButton.SetOnClickListener(clickListener);
            searchButton.SetOnClickListener(clickListener);
            tintView.SetOnClickListener(clickListener);

            IsSetVoiceSearch = false;

            IsShowVoice = true;

            InitSearchView();

            searchLayout.Visibility = ViewStates.Gone;
            suggestionsListView.Visibility = ViewStates.Gone;
            AnimationDuration = (AnimationUtil.ANIMATION_DURATION_MEDIUM);
        }

        private void InitSearchView()
        {
            searchInput.SetOnEditorActionListener(new MaterialSearchViewOnEditorActionListener(this));
            searchInput.AddTextChangedListener(new MaterialSearchViewTextChangedListener(this));
            searchInput.OnFocusChangeListener = new MaterialSearchViewOnFocusChangeListener(this);
        }
        #endregion init

        #region private
        private void StartFilter(string s)
        {
            if (mAdapter != null && mAdapter is IFilterable)
            {
                ((IFilterable)mAdapter).Filter.InvokeFilter(s, this);
            }
        }

        private void OnVoiceClicked()
        {
            Intent intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelWebSearch);
            if (this.context is Activity)
            {
                ((Activity)this.context).StartActivityForResult(intent, MaterialSearchView.REQUEST_VOICE);
            }
        }

        private void OnTextChanged(string newText)
        {
            string text = searchInput.Text;
            userQueryText = text;
            bool hasText = !string.IsNullOrWhiteSpace(text);
            if (hasText)
            {
                rightButtonsLayout.Show();
                //emptyButton.Show();
                IsShowVoice = false;
            }
            else
            {
                rightButtonsLayout.Hide();
                //emptyButton.Hide();
                IsShowVoice = true;
            }

            if (QueryTextListener != null && newText != oldQueryText)
            {
                QueryTextListener.OnQueryTextChange(newText);
            }
            oldQueryText = newText;
        }

        private void OnSubmitQuery()
        {
            string query = searchInput.Text;
            if (!string.IsNullOrWhiteSpace(query))
            {
                AddSuggestion(query);
                if (QueryTextListener == null || !QueryTextListener.OnQueryTextSubmit(query))
                {
                    CloseSearch();
                    searchInput.Text = null;
                }
            }
        }


        #endregion private

        #region public
        public void HideKeyboard()
        {
            HideKeyboard(this.searchInput);
        }
        public void HideKeyboard(View view)
        {
            InputMethodManager imm = view.Context.GetSystemService(Context.InputMethodService).JavaCast<InputMethodManager>();
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }

        public void ShowKeyboard()
        {
            ShowKeyboard(this.searchInput);
        }
        public void ShowKeyboard(View view)
        {
            if (Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.GingerbreadMr1 && view.HasFocus)
            {
                view.ClearFocus();
            }
            view.RequestFocus();
            InputMethodManager imm = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
            imm.ShowSoftInput(view, 0);
        }

        /// <summary>
        /// Call this method to show suggestions list. This shows up when adapter is set. Call {@link #setAdapter(ListAdapter)} before calling this.
        /// </summary>
        public void ShowSuggestions()
        {
            if (mAdapter != null && mAdapter.Count > 0 && suggestionsListView.IsHidden())
            {
                suggestionsListView.Show();
            }
        }



        /// <summary>
        /// Set Suggest List OnItemClickListener
        /// </summary>
        public AdapterView.IOnItemClickListener OnItemClickListener
        {
            set
            {
                suggestionsListView.OnItemClickListener = value;
            }
        }



        /// <summary>
        /// Dismiss the suggestions list
        /// </summary>
        public void DismissSuggestions()
        {
            if (suggestionsListView.IsShown())
            {
                suggestionsListView.Hide();
            }
        }

        /// <summary>
        /// Calling this will set the query to search text box. if submit is true, it'll submit the query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="submit"></param>
        public void SetQuery(string query, bool submit)
        {
            searchInput.Text = query;
            if (query != null)
            {
                searchInput.SetSelection(searchInput.Length());
                userQueryText = query;
            }
            if (submit && !string.IsNullOrWhiteSpace(query))
            {
                OnSubmitQuery();
            }
        }



        /// <summary>
        /// Open Search View. This will animate the showing of the view.
        /// </summary>
        /// <param name="animate"></param>
        public void ShowSearch(bool animate = false)
        {
            if (IsSearchOpen)
            {
                return;
            }

            //Request Focus
            searchInput.Text = null;
            searchInput.RequestFocus();

            if (animate)
            {
                SetVisibleWithAnimation();
            }
            else
            {
                searchLayout.Show();
                if (SearchViewListener != null)
                {
                    SearchViewListener.OnSearchViewShown();
                }
            }
            IsSearchOpen = true;
        }

        private void SetVisibleWithAnimation()
        {
            AnimationUtil.AnimationListener animationListener = new MaterialSearchViewAnimationListener(this);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                searchLayout.Show();
                AnimationUtil.reveal(searchTopBar, animationListener);
            }
            else
            {
                AnimationUtil.fadeInView(searchLayout, AnimationDuration, animationListener);
            }
        }

        /// <summary>
        /// Close search view
        /// </summary>
        public void CloseSearch()
        {
            if (!IsSearchOpen)
            {
                return;
            }

            searchInput.Text = null;
            DismissSuggestions();
            ClearFocus();

            searchLayout.Hide();
            if (SearchViewListener != null)
            {
                SearchViewListener.OnSearchViewClosed();
            }
            IsSearchOpen = false;

        }


        public void OnFilterComplete(int count)
        {
            if (count > 0)
            {
                ShowSuggestions();
            }
            else
            {
                DismissSuggestions();
            }
        }

        public override bool RequestFocus(FocusSearchDirection direction, Rect previouslyFocusedRect)
        {
            // Don't accept focus if in the middle of clearing focus
            if (clearingFocus) return false;
            // Check if SearchView is focusable.
            if (!Focusable) return false;
            return searchInput.RequestFocus(direction, previouslyFocusedRect);
        }

        public override void ClearFocus()
        {
            clearingFocus = true;
            HideKeyboard(this);
            base.ClearFocus();
            searchInput.ClearFocus();
            clearingFocus = false;
        }

        #endregion public

        #region protected

        protected override IParcelable OnSaveInstanceState()
        {
            IParcelable superState = base.OnSaveInstanceState();
            savedState = new MaterialSearchViewSavedState(superState);

            savedState.Query = userQueryText;
            savedState.IsSearchOpen = this.IsSearchOpen;

            return base.OnSaveInstanceState();
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (!(state is MaterialSearchViewSavedState))
            {
                base.OnRestoreInstanceState(state);
                return;
            }

            savedState = state as MaterialSearchViewSavedState;
            if (savedState.IsSearchOpen)
            {
                ShowSearch(false);
                SetQuery(savedState.Query, false);
            }

            base.OnRestoreInstanceState(savedState.SuperState);
        }

        #endregion protected

        #region interface



        public interface ISearchViewListener
        {
            void OnSearchViewShown();
            void OnSearchViewClosed();
        }

        #endregion interface


        #region Listener classes

        private class MaterialSearchViewOnClickListener : Java.Lang.Object, IOnClickListener
        {
            private MaterialSearchView materialSearchView;

            public MaterialSearchViewOnClickListener(MaterialSearchView materialSearchView)
            {
                this.materialSearchView = materialSearchView;
            }

            public void OnClick(View v)
            {
                if (v == materialSearchView.backButton) materialSearchView.CloseSearch();
                else if (v == materialSearchView.voiceButton) materialSearchView.OnVoiceClicked();
                else if (v == materialSearchView.searchButton) materialSearchView.OnSubmitQuery();
                else if (v == materialSearchView.emptyButton) materialSearchView.searchInput.Text = null;
                else if (v == materialSearchView.searchInput) materialSearchView.ShowSuggestions();
                else if (v == materialSearchView.tintView) materialSearchView.CloseSearch();
            }
        }

        private class MaterialSearchViewOnEditorActionListener : Java.Lang.Object, TextView.IOnEditorActionListener
        {
            public MaterialSearchView materialSearchView;

            public MaterialSearchViewOnEditorActionListener(MaterialSearchView m)
            {
                this.materialSearchView = m;
            }

            public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
            {
                this.materialSearchView.OnSubmitQuery();
                return true;
            }
        }

        private class MaterialSearchViewTextChangedListener : Java.Lang.Object, ITextWatcher
        {
            private MaterialSearchView m;

            public MaterialSearchViewTextChangedListener(MaterialSearchView m)
            {
                this.m = m;
            }

            public void AfterTextChanged(IEditable s)
            {
            }

            public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
            {
            }

            public void OnTextChanged(ICharSequence s, int start, int before, int count)
            {
                string s_ = s.ToString();
                m.userQueryText = s_;
                m.StartFilter(s_);
                m.OnTextChanged(s_);
            }
        }

        private class MaterialSearchViewOnFocusChangeListener : Java.Lang.Object, IOnFocusChangeListener
        {
            private MaterialSearchView m;

            public MaterialSearchViewOnFocusChangeListener(MaterialSearchView m)
            {
                this.m = m;
            }

            public void OnFocusChange(View v, bool hasFocus)
            {
                if (hasFocus)
                {
                    m.ShowKeyboard(m.searchInput);
                    m.ShowSuggestions();
                }
            }
        }

        private class MaterialSearchViewAdapterViewOnItemClickListener : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private SearchAdapter a;
            private MaterialSearchView m;

            public MaterialSearchViewAdapterViewOnItemClickListener(MaterialSearchView m, SearchAdapter a)
            {
                this.m = m;
                this.a = a;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                m.SetQuery((string)a.GetItem(position), m.submit);
            }
        }

        private class MaterialSearchViewOnMenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            private MaterialSearchView m;

            public MaterialSearchViewOnMenuItemClickListener(MaterialSearchView m)
            {
                this.m = m;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                m.ShowSearch();
                return true;
            }
        }

        private class MaterialSearchViewAnimationListener : Java.Lang.Object, AnimationUtil.AnimationListener
        {
            private MaterialSearchView m;

            public MaterialSearchViewAnimationListener(MaterialSearchView m)
            {
                this.m = m;
            }

            public bool onAnimationCancel(View view)
            {
                return false;
            }

            public bool onAnimationEnd(View view)
            {
                if (m.SearchViewListener != null)
                {
                    m.SearchViewListener.OnSearchViewShown();
                }
                return false;
            }

            public bool onAnimationStart(View view)
            {
                return false;
            }
        }

        private class MaterialSearchViewSavedState : BaseSavedState
        {
            public string Query { get; set; }
            public bool IsSearchOpen { get; set; }

            public MaterialSearchViewSavedState(IParcelable superState) : base(superState)
            {

            }

            private MaterialSearchViewSavedState(Parcel in_) : base(in_)
            {
                this.Query = in_.ReadString();
                this.IsSearchOpen = in_.ReadInt() == 1;
            }

            public override void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
            {
                base.WriteToParcel(dest, flags);
                dest.WriteString(Query);
                dest.WriteInt(IsSearchOpen ? 1 : 0);
            }

            public static SavedStateIParcelableCreator CREATOR = new SavedStateIParcelableCreator();

            public class SavedStateIParcelableCreator : Java.Lang.Object, IParcelableCreator
            {
                public SavedState CreateFromParcel(Parcel in_)
                {
                    return new SavedState(in_);
                }

                public Java.Lang.Object[] NewArray(int size)
                {
                    return new SavedState[size];
                }

                Java.Lang.Object IParcelableCreator.CreateFromParcel(Parcel source)
                {
                    return new SavedState(source);
                }
            }


            public static implicit operator SavedState(MaterialSearchViewSavedState v)
            {
                return v;
            }
        }

        #endregion Listener classes


    }

    public static class MaterialSearchViewExtensionMethods
    {
        public static void Hide(this View view)
        {
            view.Visibility = ViewStates.Gone;
        }

        public static void Show(this View view)
        {
            view.Visibility = ViewStates.Visible;
        }

        public static bool IsShown(this View view)
        {
            return view.Visibility == ViewStates.Visible;
        }

        public static bool IsHidden(this View view)
        {
            return view.Visibility == ViewStates.Gone;
        }


    }
}

