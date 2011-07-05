using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Aviad.WPF.Controls
{
    public static class ListBoxItemBehavior
    {
        public static bool GetSelectOnMouseOver(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectOnMouseOverProperty);
        }

        public static void SetSelectOnMouseOver(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectOnMouseOverProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectOnMouseOverProperty =
            DependencyProperty.RegisterAttached("SelectOnMouseOver", typeof(bool), typeof(ListBoxItemBehavior), new UIPropertyMetadata(false, OnSelectOnMouseOverChanged));

        static void OnSelectOnMouseOverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBoxItem lbi = d as ListBoxItem;
            if (lbi == null) return;
            bool bNew = (bool)e.NewValue, bOld = (bool)e.OldValue;
            if (bNew == bOld) return;
            if (bNew)
                lbi.MouseEnter += new MouseEventHandler(lbi_MouseEnter);
            else
                lbi.MouseEnter -= lbi_MouseEnter;
        }

        static void lbi_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ListBoxItem lbi = (ListBoxItem)sender;
            lbi.IsSelected = true;
            var listBox = ListBox.ItemsControlFromItemContainer(lbi);
            FrameworkElement focusedElement = (FrameworkElement)FocusManager.GetFocusedElement(FocusManager.GetFocusScope(listBox));
            if (focusedElement != null && focusedElement.IsDescendantOf(listBox))
                lbi.Focus();
        }
    }
}
