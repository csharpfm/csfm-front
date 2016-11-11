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

namespace csfm_android.Ui.Adapters
{
    public abstract class AbstractAdapter<T> : RecyclerView.Adapter
    {
        private List<T> data;

        /// <summary>
        /// Updates the view
        /// </summary>
        public List<T> Data
        {
            set
            {
                SetData(value, () => NotifyDataSetChanged());
            }
            
            get
            {
                return data;
            }
        }

        public override int ItemCount
        {
            get
            {
                return data != null ? data.Count : 0;
            }
        }

        public AbstractAdapter(Context context, List<T> data)
        {
            this.data = data;
        }

        protected void SetData(List<T> data, Action callback)
        {
            this.data = data;
            callback();
        }

    }

}