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

namespace csfm_android.Utils.MaterialDesignSearchView
{
    public class MaterialSearchView : FrameLayout, Filter.IFilterListener
    {
        public const int REQUEST_VOICE = 9999;

        private IMenuItem mMenuItem;
        private bool mIsSearchOpen = false;
        private int mAnimationDuration;
        private bool mClearingFocus;

        //Views
        private View mSearchLayout;
        public View mTintView { get; private set; }
        private ListView mSuggestionsListView;
        public EditText mSearchSrcTextView { get; private set; }
        public ImageButton mBackBtn { get; private set; }
        public ImageButton mVoiceBtn { get; private set; }
        public ImageButton mEmptyBtn { get; private set; }
        public RelativeLayout mSearchTopBar { get; private set; }

        public string mOldQueryText { get; set; }
        public string mUserQuery { get; set; }

        public IOnQueryTextListener mOnQueryChangeListener { get; private set; }
        public ISearchViewListener mSearchViewListener { get; private set; }

        private IListAdapter mAdapter;

        private MaterialSearchViewSavedState mSavedState;
        public bool submit = false;

        private bool ellipsize = false;

        private bool allowVoiceSearch;
        private Drawable suggestionIcon;

        public Context mContext { get; private set; }

        private MaterialSearchViewOnClickListener mOnClickListener;

        public MaterialSearchView(Context context) : this(context, null)
        {
        }

        public MaterialSearchView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public MaterialSearchView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs)
        {
            mContext = context;
            mOnClickListener = new MaterialSearchViewOnClickListener(this);
            initiateView();
            initStyle(attrs, defStyleAttr);
        }

        private void initStyle(IAttributeSet attrs, int defStyleAttr)
        {
            TypedArray a = mContext.ObtainStyledAttributes(attrs, Resource.Styleable.MaterialSearchView, defStyleAttr, 0);

            if (a != null)
            {
                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchBackground))
                {
                    Background = a.GetDrawable(Resource.Styleable.MaterialSearchView_searchBackground);
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_android_textColor))
                {
                    SetTextColor(a.GetColor(Resource.Styleable.MaterialSearchView_android_textColor, 0));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_android_textColorHint))
                {
                    SetHintTextColor(a.GetColor(Resource.Styleable.MaterialSearchView_android_textColorHint, 0));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_android_hint))
                {
                    SetHint(a.GetString(Resource.Styleable.MaterialSearchView_android_hint));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchVoiceIcon))
                {
                    SetVoiceIcon(a.GetDrawable(Resource.Styleable.MaterialSearchView_searchVoiceIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchCloseIcon))
                {
                    SetCloseIcon(a.GetDrawable(Resource.Styleable.MaterialSearchView_searchCloseIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchBackIcon))
                {
                    SetBackIcon(a.GetDrawable(Resource.Styleable.MaterialSearchView_searchBackIcon));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchSuggestionBackground))
                {
                    SetSuggestionBackground(a.GetDrawable(Resource.Styleable.MaterialSearchView_searchSuggestionBackground));
                }

                if (a.HasValue(Resource.Styleable.MaterialSearchView_searchSuggestionIcon))
                {
                    SetSuggestionIcon(a.GetDrawable(Resource.Styleable.MaterialSearchView_searchSuggestionIcon));
                }

                a.Recycle();
            }
        }

        private void initiateView()
        {
            LayoutInflater.From(mContext).Inflate(Resource.Layout.material_search_view, this, true);
            mSearchLayout = FindViewById<View>(Resource.Id.search_layout);

            mSearchTopBar = mSearchLayout.FindViewById<RelativeLayout>(Resource.Id.search_top_bar);
            mSuggestionsListView = mSearchLayout.FindViewById<ListView>(Resource.Id.suggestion_list);
            mSearchSrcTextView = mSearchLayout.FindViewById<EditText>(Resource.Id.searchTextView);
            mBackBtn = mSearchLayout.FindViewById<ImageButton>(Resource.Id.action_up_btn);
            mVoiceBtn = mSearchLayout.FindViewById<ImageButton>(Resource.Id.action_voice_btn);
            mEmptyBtn = mSearchLayout.FindViewById<ImageButton>(Resource.Id.action_empty_btn);
            mTintView = mSearchLayout.FindViewById(Resource.Id.transparent_view);

            mSearchSrcTextView.SetOnClickListener(mOnClickListener);
            mBackBtn.SetOnClickListener(mOnClickListener);
            mVoiceBtn.SetOnClickListener(mOnClickListener);
            mEmptyBtn.SetOnClickListener(mOnClickListener);
            mTintView.SetOnClickListener(mOnClickListener);

            allowVoiceSearch = false;

            ShowVoice(true);

            InitSearchView();

            mSearchLayout.Visibility = ViewStates.Gone;
            mSuggestionsListView.Visibility = ViewStates.Gone;
            SetAnimationDuration(AnimationUtil.ANIMATION_DURATION_MEDIUM);
        }

        private void InitSearchView()
        {
            mSearchSrcTextView.SetOnEditorActionListener(new MaterialSearchViewOnEditorActionListener(this));
            mSearchSrcTextView.AddTextChangedListener(new MaterialSearchViewTextChangedListener(this));
            mSearchSrcTextView.OnFocusChangeListener = new MaterialSearchViewOnFocusChangeListener(this);
        }

        public void StartFilter(string s)
        {
            if (mAdapter != null && mAdapter is IFilterable)
            {
                ((IFilterable)mAdapter).Filter.InvokeFilter(s, this);
            }
        }

        public void OnVoiceClicked()
        {
            Intent intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelWebSearch);
            if (mContext is Activity)
            {
                ((Activity)mContext).StartActivityForResult(intent, MaterialSearchView.REQUEST_VOICE);
            }
        }

        public void OnTextChanged(string newText)
        {
            string text = mSearchSrcTextView.Text;
            mUserQuery = text;
            bool hasText = !string.IsNullOrWhiteSpace(text);
            if (hasText)
            {
                mEmptyBtn.Visibility = ViewStates.Visible;
                ShowVoice(false);
            }
            else
            {
                mEmptyBtn.Visibility = ViewStates.Gone;
                ShowVoice(true);
            }

            if (mOnQueryChangeListener != null && newText != mOldQueryText)
            {
                mOnQueryChangeListener.OnQueryTextChange(newText);
            }
            mOldQueryText = newText;
        }

        public void OnSubmitQuery()
        {
            string query = mSearchSrcTextView.Text;
            if (!string.IsNullOrWhiteSpace(query))
            {
                if (mOnQueryChangeListener == null || !mOnQueryChangeListener.OnQueryTextSubmit(query))
                {
                    CloseSearch();
                    mSearchSrcTextView.Text = null;
                }
            }
        }

        private bool IsVoiceAvailable()
        {
            if (IsInEditMode)
            {
                return true;
            }
            PackageManager pm = mContext.PackageManager;
            IList<ResolveInfo> activities = pm.QueryIntentActivities(new Intent(RecognizerIntent.ActionRecognizeSpeech), 0);
            return activities.Count == 0;
        }

        public void HideKeyboard(View view)
        {
            InputMethodManager imm = view.Context.GetSystemService(Context.InputMethodService).JavaCast<InputMethodManager>();
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
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

        public void SetBackground(Drawable background)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                mSearchTopBar.Background = background;
            }
            else
            {
                mSearchTopBar.SetBackgroundDrawable(background);
            }
        }

        public void SetTextColor(Android.Graphics.Color color)
        {
            mSearchSrcTextView.SetTextColor(color);
        }

        public void SetHintTextColor(Android.Graphics.Color color)
        {
            mSearchSrcTextView.SetHintTextColor(color);
        }

        public void SetHint(string hint)
        {
            mSearchSrcTextView.Hint = hint;
        }

        public void SetVoiceIcon(Drawable drawable)
        {
            mVoiceBtn.SetImageDrawable(drawable);
        }

        public void SetCloseIcon(Drawable drawable)
        {
            mEmptyBtn.SetImageDrawable(drawable);
        }

        public void SetBackIcon(Drawable drawable)
        {
            mBackBtn.SetImageDrawable(drawable);
        }

        public void SetSuggestionIcon(Drawable drawable)
        {
            suggestionIcon = drawable;
        }

        public void SetSuggestionBackground(Drawable background)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                mSuggestionsListView.Background = background;
            }
            else
            {
                mSuggestionsListView.SetBackgroundDrawable(background);
            }
        }

        public void SetCursorDrawable(int drawable)
        {
            try
            {
                var prop = typeof(TextView).GetField("mCursorDrawableRes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop.SetValue(mSearchSrcTextView, drawable);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("MaterialSearchView", ex.ToString());
                //Ignored
            }
        }

        public void SetVoiceSearch(bool voiceSearch)
        {
            allowVoiceSearch = voiceSearch;
        }

        public void ShowSuggestions()
        {
            if (mAdapter != null && mAdapter.Count > 0 && mSuggestionsListView.Visibility == ViewStates.Gone)
            {
                mSuggestionsListView.Visibility = ViewStates.Visible;
            }
        }

        public void SetSubmitOnClick(bool submit)
        {
            this.submit = submit;
        }

        public void SetOnItemClickListener(AdapterView.IOnItemClickListener listener)
        {
            mSuggestionsListView.OnItemClickListener = listener;
        }

        public void SetAdapter(IListAdapter adapter)
        {
            mAdapter = adapter;
            mSuggestionsListView.Adapter = adapter;
            StartFilter(mSearchSrcTextView.Text);
        }

        public void SetSuggestions(string[] suggestions)
        {
            if (suggestions != null && suggestions.Length > 0)
            {
                mTintView.Visibility = ViewStates.Visible;
                SearchAdapter adapter = new SearchAdapter(mContext, suggestions, suggestionIcon, ellipsize);
                SetAdapter(adapter);

                SetOnItemClickListener(new MaterialSearchViewAdapterViewOnItemClickListener(this, adapter));
            }
            else
            {
                mTintView.Visibility = ViewStates.Gone;
            }
        }

        public void DismissSuggestions()
        {
            if (mSuggestionsListView.Visibility == ViewStates.Visible)
            {
                mSuggestionsListView.Visibility = ViewStates.Gone;
            }
        }

        public void SetQuery(string query, bool submit)
        {
            mSearchSrcTextView.Text = query;
            if (query != null)
            {
                mSearchSrcTextView.SetSelection(mSearchSrcTextView.Length());
                mUserQuery = query;
            }
            if (submit && !string.IsNullOrWhiteSpace(query))
            {
                OnSubmitQuery();
            }
        }

        public void ShowVoice(bool show)
        {
            if (show && IsVoiceAvailable() && allowVoiceSearch)
            {
                mVoiceBtn.Visibility = ViewStates.Visible;
            }
            else
            {
                mVoiceBtn.Visibility = ViewStates.Gone;
            }
        }

        public void SetMenuItem(IMenuItem item)
        {
            this.mMenuItem = item;
            mMenuItem.SetOnMenuItemClickListener(new MaterialSearchViewOnMenuItemClickListener(this));
        }

        public bool IsSearchOpen()
        {
            return mIsSearchOpen;
        }

        public void SetAnimationDuration(int duration)
        {
            mAnimationDuration = duration;
        }

        public void ShowSearch(bool animate = true)
        {
            if (IsSearchOpen())
            {
                return;
            }

            //Request Focus
            mSearchSrcTextView.Text = null;
            mSearchSrcTextView.RequestFocus();

            //if (animate)
            //{
            //    SetVisibleWithAnimation();
            //}
            //else
            //{
                mSearchLayout.Visibility = ViewStates.Visible;
                if (mSearchViewListener != null)
                {
                    mSearchViewListener.OnSearchViewShown();
                }
            //}
            mIsSearchOpen = true;
        }

        private void SetVisibleWithAnimation()
        {
            AnimationUtil.AnimationListener animationListener = new MaterialSearchViewAnimationListener(this);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                mSearchLayout.Visibility = ViewStates.Visible;
                AnimationUtil.reveal(mSearchTopBar, animationListener);
            }
            else
            {
                AnimationUtil.fadeInView(mSearchLayout, mAnimationDuration, animationListener);
            }
        }

        public void CloseSearch()
        {
            if (!IsSearchOpen())
            {
                return;
            }

            mSearchSrcTextView.Text = null;
            DismissSuggestions();
            ClearFocus();

            mSearchLayout.Visibility = ViewStates.Gone;
            if (mSearchViewListener != null)
            {
                mSearchViewListener.OnSearchViewClosed();
            }
            mIsSearchOpen = false;

        }

        public void SetOnQueryTextListener(IOnQueryTextListener listener)
        {
            mOnQueryChangeListener = listener;
        }

        public void SetOnSearchViewListener(ISearchViewListener listener)
        {
            mSearchViewListener = listener;
        }

        public void SetEllipsize(bool ellipsize)
        {
            this.ellipsize = ellipsize;
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
            if (mClearingFocus) return false;
            // Check if SearchView is focusable.
            if (!Focusable) return false;
            return mSearchSrcTextView.RequestFocus(direction, previouslyFocusedRect);
        }

        public override void ClearFocus()
        {
            mClearingFocus = true;
            HideKeyboard(this);
            base.ClearFocus();
            mSearchSrcTextView.ClearFocus();
            mClearingFocus = false;
        }

        protected override IParcelable OnSaveInstanceState()
        {
            IParcelable superState = base.OnSaveInstanceState();
            mSavedState = new MaterialSearchViewSavedState(superState);

            mSavedState.Query = mUserQuery;
            mSavedState.IsSearchOpen = this.mIsSearchOpen;

            return base.OnSaveInstanceState();
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (!(state is MaterialSearchViewSavedState))
            {
                base.OnRestoreInstanceState(state);
                return;
            }

            mSavedState = state as MaterialSearchViewSavedState;
            if (mSavedState.IsSearchOpen)
            {
                ShowSearch(false);
                SetQuery(mSavedState.Query, false);
            }

            base.OnRestoreInstanceState(mSavedState.SuperState);
        }


    }



    public interface ISearchViewListener
    {
        void OnSearchViewShown();
        void OnSearchViewClosed();
    }

    public class MaterialSearchViewOnClickListener : Java.Lang.Object, IOnClickListener
    {
        private MaterialSearchView materialSearchView;

        public MaterialSearchViewOnClickListener(MaterialSearchView materialSearchView)
        {
            this.materialSearchView = materialSearchView;
        }

        public void OnClick(View v)
        {
            if (v == materialSearchView.mBackBtn) materialSearchView.CloseSearch();
            else if (v == materialSearchView.mVoiceBtn) materialSearchView.OnVoiceClicked();
            else if (v == materialSearchView.mEmptyBtn) materialSearchView.mSearchSrcTextView.Text = null;
            else if (v == materialSearchView.mSearchSrcTextView) materialSearchView.ShowSuggestions();
            else if (v == materialSearchView.mTintView) materialSearchView.CloseSearch();
        }
    }

    public class MaterialSearchViewOnEditorActionListener : Java.Lang.Object, TextView.IOnEditorActionListener
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

    public class MaterialSearchViewTextChangedListener : Java.Lang.Object, ITextWatcher
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
            m.mUserQuery = s_;
            m.StartFilter(s_);
            m.OnTextChanged(s_);
        }
    }

    public class MaterialSearchViewOnFocusChangeListener : Java.Lang.Object, IOnFocusChangeListener
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
                m.ShowKeyboard(m.mSearchSrcTextView);
                m.ShowSuggestions();
            }
        }
    }

    public class MaterialSearchViewAdapterViewOnItemClickListener : Java.Lang.Object, AdapterView.IOnItemClickListener
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

    public class MaterialSearchViewOnMenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
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

    public class MaterialSearchViewAnimationListener : Java.Lang.Object, AnimationUtil.AnimationListener
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
            if (m.mSearchViewListener != null)
            {
                m.mSearchViewListener.OnSearchViewShown();
            }
            return false;
        }

        public bool onAnimationStart(View view)
        {
            return false;
        }
    }

    public class MaterialSearchViewSavedState : BaseSavedState
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
            throw new NotImplementedException();
        }
    }


}