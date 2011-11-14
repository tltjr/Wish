using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
        private readonly FrameworkElement _dummy = new FrameworkElement();
        private Func<object, string, bool> _filter;
        public ListBox ListBox;
        private Popup _popup;
        private bool _suppressEvent;
        private string _textCache = "";
        // Binding hack - not really necessary.
        //DependencyObject dummy = new DependencyObject();

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (AutoCompleteTextBox),
                                                     new FrameworkPropertyMetadata(typeof (AutoCompleteTextBox)));
        }

        public Func<object, string, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (_filter == value) return;
                _filter = value;
                if (ListBox == null) return;
                if (_filter != null)
                {
                    ListBox.Items.Filter = FilterFunc;
                }
                else
                {
                    ListBox.Items.Filter = null;
                }
            }
        }

        #region ItemsSource Dependency Property

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof (AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as IEnumerable);
        }

        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (ListBox == null || itemsSource == null) return;
            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                ListBox.ItemsSource = new LimitedListCollectionView(((ListCollectionView) itemsSource).SourceCollection)
                                          {Limit = MaxCompletions};
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                ListBox.ItemsSource = new LimitedListCollectionView(((CollectionView) itemsSource).SourceCollection)
                                          {Limit = MaxCompletions};
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                ListBox.ItemsSource = new LimitedListCollectionView(itemsSource) {Limit = MaxCompletions};
                Debug.Print("Was IList");
            }
            else
            {
                ListBox.ItemsSource = new LimitedCollectionView(itemsSource) {Limit = MaxCompletions};
                Debug.Print("Was IEnumerable");
            }
            if (ListBox.Items.Count == 0) InternalClosePopup();
        }

        #endregion

        #region Binding Dependency Property

        // Using a DependencyProperty as the backing store for Binding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding", typeof (string), typeof (AutoCompleteTextBox),
                                        new UIPropertyMetadata(null));

        public string Binding
        {
            get { return (string) GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        #endregion

        #region ItemTemplate Dependency Property

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            ItemsControl.ItemTemplateProperty.AddOwner(
                typeof (AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemTemplateChanged));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateChanged(e.NewValue as DataTemplate);
        }

        private void OnItemTemplateChanged(DataTemplate p)
        {
            if (ListBox == null) return;
            ListBox.ItemTemplate = p;
        }

        #endregion

        #region ItemContainerStyle Dependency Property

        // Using a DependencyProperty as the backing store for ItemContainerStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemContainerStyleProperty =
            ItemsControl.ItemContainerStyleProperty.AddOwner(
                typeof (AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemContainerStyleChanged));

        public Style ItemContainerStyle
        {
            get { return (Style) GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemContainerStyleChanged(e.NewValue as Style);
        }

        private void OnItemContainerStyleChanged(Style p)
        {
            if (ListBox == null) return;
            ListBox.ItemContainerStyle = p;
        }

        #endregion

        #region MaxCompletions Dependency Property

        // Using a DependencyProperty as the backing store for MaxCompletions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCompletionsProperty =
            DependencyProperty.Register("MaxCompletions", typeof (int), typeof (AutoCompleteTextBox),
                                        new UIPropertyMetadata(int.MaxValue));

        public int MaxCompletions
        {
            get { return (int) GetValue(MaxCompletionsProperty); }
            set { SetValue(MaxCompletionsProperty, value); }
        }

        #endregion

        #region ItemTemplateSelector Dependency Property

        // Using a DependencyProperty as the backing store for ItemTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            ItemsControl.ItemTemplateSelectorProperty.AddOwner(typeof (AutoCompleteTextBox),
                                                               new UIPropertyMetadata(null,
                                                                                      OnItemTemplateSelectorChanged));

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnItemTemplateSelectorChanged(DataTemplateSelector p)
        {
            if (ListBox == null) return;
            ListBox.ItemTemplateSelector = p;
        }

        #endregion

        private void InternalClosePopup()
        {
            if (_popup != null)
                _popup.IsOpen = false;
        }

        private void InternalOpenPopup()
        {
            _popup.IsOpen = true;
            if (ListBox != null) ListBox.SelectedIndex = -1;
        }

        public void ShowPopup()
        {
            if (ListBox == null || _popup == null) InternalClosePopup();
            else if (ListBox.Items.Count == 0) InternalClosePopup();
            else InternalOpenPopup();
        }

        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (_popup != null)
            {
                InternalClosePopup();
                Dispatcher.Invoke(new Action(() =>
                                                 {
                                                     Focus();
                                                     if (moveFocus)
                                                         MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                                                 }), DispatcherPriority.Background);
            }

            // Retrieve the Binding object from the control.
            Binding originalBinding = BindingOperations.GetBinding(this, BindingProperty);
            if (originalBinding == null) return;

            // Set the dummy's DataContext to our selected object.
            _dummy.DataContext = obj;

            // Apply the binding to the dummy FrameworkElement.
            BindingOperations.SetBinding(_dummy, TextProperty, originalBinding);
            _suppressEvent = true;

            // Get the binding's resulting value.
            Text = _dummy.GetValue(TextProperty).ToString();
            _suppressEvent = false;
            ListBox.SelectedIndex = -1;
            SelectAll();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (_suppressEvent) return;
            _textCache = Text ?? "";
            Debug.Print("Text: " + _textCache);
            if (_popup != null && _textCache == "")
            {
                InternalClosePopup();
            }
            else if (ListBox != null)
            {
                if (_filter != null)
                    ListBox.Items.Filter = FilterFunc;

                if (_popup != null)
                {
                    if (ListBox.Items.Count == 0)
                        InternalClosePopup();
                    else
                        InternalOpenPopup();
                }
            }
        }

        private bool FilterFunc(object obj)
        {
            return _filter(obj, _textCache);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = Template.FindName("PART_Popup", this) as Popup;
            ListBox = Template.FindName("PART_ListBox", this) as ListBox;
            if (ListBox != null)
            {
                ListBox.PreviewMouseDown += listBox_MouseUp;
                ListBox.KeyDown += listBox_KeyDown;
                OnItemsSourceChanged(ItemsSource);
                OnItemTemplateChanged(ItemTemplate);
                OnItemContainerStyleChanged(ItemContainerStyle);
                OnItemTemplateSelectorChanged(ItemTemplateSelector);
                if (_filter != null)
                    ListBox.Items.Filter = FilterFunc;
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (_suppressEvent) return;
            if (_popup != null)
            {
                InternalClosePopup();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            DependencyObject fs = FocusManager.GetFocusScope(this);
            IInputElement o = FocusManager.GetFocusedElement(fs);
            if (e.Key == Key.Escape)
            {
                InternalClosePopup();
                Focus();
            }
            else if (e.Key == Key.Down)
            {
                if (ListBox != null && o == this)
                {
                    _suppressEvent = true;
                    ListBox.Focus();
                    _suppressEvent = false;
                }
            }
        }

        private void listBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var dep = (DependencyObject) e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;
            object item = ListBox.ItemContainerGenerator.ItemFromContainer(dep);
            if (item == null) return;
            SetTextValueBySelection(item, false);
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                SetTextValueBySelection(ListBox.SelectedItem, false);
            else if (e.Key == Key.Tab)
                SetTextValueBySelection(ListBox.SelectedItem, true);
        }
    }
}