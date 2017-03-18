using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFInteraction
{
    public class ListViewColumnsAutoSizeHelper
    {
        #region bool .Enabled

        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for Enable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(ListViewColumnsAutoSizeHelper), new PropertyMetadata(
                false, new PropertyChangedCallback(OnEnabledPropertyChangedCallback)
            ));

        #endregion

        static void OnEnabledPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lview = d as ListView;
            if (d != null)
            {
                var newValue = (bool)e.NewValue;
                var oldValue = (bool)e.OldValue;

                if (newValue && !oldValue)
                {
                    lview.SizeChanged += lview_SizeChanged;
                }
                else if (!newValue && oldValue)
                {
                    lview.SizeChanged -= lview_SizeChanged;
                }
            }
        }

        private static void lview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var lview = sender as ListView;
            var view = lview.View as GridView;

            if (e.WidthChanged && view != null)
            {
                var dw = (e.NewSize.Width - e.PreviousSize.Width) / view.Columns.Count;

                foreach (var col in view.Columns)
                    col.Width = Math.Max(col.ActualWidth + dw, 5);
            }
        }
    }
}
