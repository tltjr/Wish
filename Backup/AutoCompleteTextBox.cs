using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Diagnostics;

namespace Aviad.WPF.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ACTB"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ACTB;assembly=ACTB"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:AutoCompleteTextBox/>
    ///
    /// </summary>
    public class AutoCompleteTextBox : TextBox
    {
        Popup popup;
        ListBox listBox;
        Func<object, string, bool> filter;
        string textCache = "";
        bool suppressEvent = false;
        // Binding hack - not really necessary.
        //DependencyObject dummy = new DependencyObject();
        FrameworkElement dummy = new FrameworkElement();

        public Func<object, string, bool> Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (filter != value)
                {
                    filter = value;
                    if (listBox != null)
                    {
                        if (filter != null)
                            listBox.Items.Filter = FilterFunc;
                        else
                            listBox.Items.Filter = null;
                    }
                }
            }
        }

        #region ItemsSource Dependency Property

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as IEnumerable);
        }

        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (listBox == null) return;
            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                listBox.ItemsSource = new LimitedListCollectionView((IList)((ListCollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions };
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                listBox.ItemsSource = new LimitedListCollectionView(((CollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions };
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                listBox.ItemsSource = new LimitedListCollectionView((IList)itemsSource) { Limit = MaxCompletions };
                Debug.Print("Was IList");
            }
            else
            {
                listBox.ItemsSource = new LimitedCollectionView(itemsSource) { Limit = MaxCompletions };
                Debug.Print("Was IEnumerable");
            }
            if (listBox.Items.Count == 0) InternalClosePopup();
        }

        #endregion

        #region Binding Dependency Property

        public string Binding
        {
            get { return (string)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Binding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding", typeof(string), typeof(AutoCompleteTextBox), new UIPropertyMetadata(null));

        #endregion

        #region ItemTemplate Dependency Property

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            ItemsControl.ItemTemplateProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemTemplateChanged));

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateChanged(e.NewValue as DataTemplate);
        }

        private void OnItemTemplateChanged(DataTemplate p)
        {
            if (listBox == null) return;
            listBox.ItemTemplate = p;
        }

        #endregion

        #region ItemContainerStyle Dependency Property

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemContainerStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemContainerStyleProperty =
            ItemsControl.ItemContainerStyleProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemContainerStyleChanged));

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemContainerStyleChanged(e.NewValue as Style);
        }

        private void OnItemContainerStyleChanged(Style p)
        {
            if (listBox == null) return;
            listBox.ItemContainerStyle = p;
        }

        #endregion

        #region MaxCompletions Dependency Property

        public int MaxCompletions
        {
            get { return (int)GetValue(MaxCompletionsProperty); }
            set { SetValue(MaxCompletionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxCompletions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCompletionsProperty =
            DependencyProperty.Register("MaxCompletions", typeof(int), typeof(AutoCompleteTextBox), new UIPropertyMetadata(int.MaxValue));

        #endregion

        #region ItemTemplateSelector Dependency Property

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            ItemsControl.ItemTemplateSelectorProperty.AddOwner(typeof(AutoCompleteTextBox), new UIPropertyMetadata(null, OnItemTemplateSelectorChanged));

        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnItemTemplateSelectorChanged(DataTemplateSelector p)
        {
            if (listBox == null) return;
            listBox.ItemTemplateSelector = p;
        }

        #endregion

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        private void InternalClosePopup()
        {
            if (popup != null)
                popup.IsOpen = false;
        }
        private void InternalOpenPopup()
        {
            popup.IsOpen = true;
            if (listBox != null) listBox.SelectedIndex = -1;
        }
        public void ShowPopup()
        {
            if (listBox == null || popup == null) InternalClosePopup();
            else if (listBox.Items.Count == 0) InternalClosePopup();
            else InternalOpenPopup();
        }
        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (popup != null)
            {
                InternalClosePopup();
                Dispatcher.Invoke(new Action(() =>
                {
                    Focus();
                    if (moveFocus)
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }), System.Windows.Threading.DispatcherPriority.Background);
            }

            // Retrieve the Binding object from the control.
            var originalBinding = BindingOperations.GetBinding(this, BindingProperty);
            if (originalBinding == null) return;

            // Binding hack - not really necessary.
            //Binding newBinding = new Binding()
            //{
            //    Path = new PropertyPath(originalBinding.Path.Path, originalBinding.Path.PathParameters),
            //    XPath = originalBinding.XPath,
            //    Converter = originalBinding.Converter,
            //    ConverterParameter = originalBinding.ConverterParameter,
            //    ConverterCulture = originalBinding.ConverterCulture,
            //    StringFormat = originalBinding.StringFormat,
            //    TargetNullValue = originalBinding.TargetNullValue,
            //    FallbackValue = originalBinding.FallbackValue
            //};
            //newBinding.Source = obj;
            //BindingOperations.SetBinding(dummy, TextProperty, newBinding);

            // Set the dummy's DataContext to our selected object.
            dummy.DataContext = obj;

            // Apply the binding to the dummy FrameworkElement.
            BindingOperations.SetBinding(dummy, TextProperty, originalBinding);
            suppressEvent = true;

            // Get the binding's resulting value.
            Text = dummy.GetValue(TextProperty).ToString();
            suppressEvent = false;
            listBox.SelectedIndex = -1;
            SelectAll();
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (suppressEvent) return;
            textCache = Text ?? "";
            Debug.Print("Text: " + textCache);
            if (popup != null && textCache == "")
            {
                InternalClosePopup();
            }
            else if (listBox != null)
            {
                if (filter != null)
                    listBox.Items.Filter = FilterFunc;

                if (popup != null)
                {
                    if (listBox.Items.Count == 0)
                        InternalClosePopup();
                    else
                        InternalOpenPopup();
                }
            }
        }

        private bool FilterFunc(object obj)
        {
            return filter(obj, textCache);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            popup = Template.FindName("PART_Popup", this) as Popup;
            listBox = Template.FindName("PART_ListBox", this) as ListBox;
            if (listBox != null)
            {
                listBox.PreviewMouseDown += new MouseButtonEventHandler(listBox_MouseUp);
                listBox.KeyDown += new KeyEventHandler(listBox_KeyDown);
                OnItemsSourceChanged(ItemsSource);
                OnItemTemplateChanged(ItemTemplate);
                OnItemContainerStyleChanged(ItemContainerStyle);
                OnItemTemplateSelectorChanged(ItemTemplateSelector);
                if (filter != null)
                    listBox.Items.Filter = FilterFunc;
            }
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (suppressEvent) return;
            if (popup != null)
            {
                InternalClosePopup();
            }
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var fs = FocusManager.GetFocusScope(this);
            var o = FocusManager.GetFocusedElement(fs);
            if (e.Key == Key.Escape)
            {
                InternalClosePopup();
                Focus();
            }
            else if (e.Key == Key.Down)
            {
                if (listBox != null && o == this)
                {
                    suppressEvent = true;
                    listBox.Focus();
                    suppressEvent = false;
                }
            }
        }

        void listBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;
            var item = listBox.ItemContainerGenerator.ItemFromContainer(dep);
            if (item == null) return;
            SetTextValueBySelection(item, false);
        }
        void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                SetTextValueBySelection(listBox.SelectedItem, false);
            else if (e.Key == Key.Tab)
                SetTextValueBySelection(listBox.SelectedItem, true);
        }
    }
}
