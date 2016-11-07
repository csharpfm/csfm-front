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
using Android.Support.V4.View;
using Android.Annotation;
using Android.Util;
using Android.Animation;

namespace csfm_android.Utils.MaterialDesignSearchView
{
    public class AnimationUtil
    {

        public static int ANIMATION_DURATION_SHORT = 150;
        public static int ANIMATION_DURATION_MEDIUM = 400;
        public static int ANIMATION_DURATION_LONG = 800;

        public interface AnimationListener
        {
            /**
             * @return true to override parent. Else execute Parent method
             */
            bool onAnimationStart(View view);

            bool onAnimationEnd(View view);

            bool onAnimationCancel(View view);
        }

        public static void crossFadeViews(View showView, View hideView)
        {
            crossFadeViews(showView, hideView, ANIMATION_DURATION_SHORT);
        }

        public static void crossFadeViews(View showView, View hideView, int duration)
        {
            fadeInView(showView, duration);
            fadeOutView(hideView, duration);
        }

        public static void fadeInView(View view)
        {
            fadeInView(view, ANIMATION_DURATION_SHORT);
        }

        public static void fadeInView(View view, int duration)
        {
            fadeInView(view, duration, null);
        }

        public static void fadeInView(View view, int duration, AnimationListener listener)
        {
            view.Visibility = ViewStates.Visible;
            view.Alpha = 0f;
            IViewPropertyAnimatorListener vpListener = null;

            if (listener != null)
            {
                vpListener = new ViewPropertyAnimatorListener(listener);
            }
            ViewCompat.Animate(view).Alpha(1f).SetDuration(duration).SetListener(vpListener);
        }

        public class ViewPropertyAnimatorListener : Java.Lang.Object, IViewPropertyAnimatorListener
        {
            private AnimationListener listener;

            public ViewPropertyAnimatorListener(AnimationListener listener)
            {
                this.listener = listener;
            }

            public void OnAnimationStart(View view)
            {
                if (!listener.onAnimationStart(view))
                {
                    view.DrawingCacheEnabled = true;
                }
            }

            public void OnAnimationEnd(View view)
            {
                if (!listener.onAnimationEnd(view))
                {
                    view.DrawingCacheEnabled = false;
                }
            }

            public void OnAnimationCancel(View view)
            {

            }
        }

        [TargetApi(Value = 21)] //Lollipop
        public static void reveal(View view, AnimationListener listener)
        {
            int cx = view.Width - (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 24, view.Resources.DisplayMetrics);
            int cy = view.Height / 2;
            int Radius = Math.Max(view.Width, view.Height);

            Animator anim = ViewAnimationUtils.CreateCircularReveal(view, cx, cy, 0, Radius);
            view.Visibility = ViewStates.Visible;
            anim.AddListener(new CustomAnimatorListenerAdapter(listener, view));
            anim.Start();

        }

        public class CustomAnimatorListenerAdapter : Android.Animation.AnimatorListenerAdapter
        {
            private AnimationListener listener;
            private View view;

            public CustomAnimatorListenerAdapter(AnimationListener listener, View view)
            {
                this.listener = listener;
                this.view = view;
            }

            public override void OnAnimationCancel(Animator animation)
            {
                listener.onAnimationCancel(view);
            }

            public override void OnAnimationEnd(Animator animation)
            {
                listener.onAnimationEnd(view);
            }

            public override void OnAnimationRepeat(Animator animation)
            {
            }

            public override void OnAnimationStart(Animator animation)
            {
                listener.onAnimationStart(view);
            }
        }

        public static void fadeOutView(View view)
        {
            fadeOutView(view, ANIMATION_DURATION_SHORT);
        }

        public static void fadeOutView(View view, int duration)
        {
            fadeOutView(view, duration, null);
        }

        public static void fadeOutView(View view, int duration, AnimationListener listener)
        {
            ViewCompat.Animate(view).Alpha(0f).SetDuration(duration).SetListener(new FadeOutViewPropertyAnimatorListener(view, duration, listener));
        }

        public class FadeOutViewPropertyAnimatorListener : Java.Lang.Object, IViewPropertyAnimatorListener
        {
            private int duration;
            private AnimationListener listener;
            private View view;

            public FadeOutViewPropertyAnimatorListener(View view, int duration, AnimationListener listener)
            {
                this.view = view;
                this.duration = duration;
                this.listener = listener;
            }

            public void OnAnimationCancel(View view)
            {
                
            }

            public void OnAnimationEnd(View view)
            {
                if (listener == null || !listener.onAnimationEnd(view))
                {
                    view.Visibility = ViewStates.Gone;
                    view.DrawingCacheEnabled = false;
                }
            }

            public void OnAnimationStart(View view)
            {
                if (listener == null || !listener.onAnimationStart(view))
                {
                    view.DrawingCacheEnabled = true;
                }
            }
        }
    }
}
/*            @Override
            public void onAnimationStart(View view)
{
    if (listener == null || !listener.onAnimationStart(view))
    {
        view.setDrawingCacheEnabled(true);
    }
}

@Override
            public void onAnimationEnd(View view)
{
    if (listener == null || !listener.onAnimationEnd(view))
    {
        view.setVisibility(View.GONE);
        view.setDrawingCacheEnabled(false);
    }
}

@Override
            public void onAnimationCancel(View view)
{
}
        });
    }
}
}*/