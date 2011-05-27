using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeBoxControl.Decorations;

namespace CodeBoxControl
{
    /// <summary>
    ///  A control to view or edit styled text
    /// </summary>
    public partial class CodeBox : TextBox
    {
        public static DependencyProperty BaseForegroundProperty = DependencyProperty.Register("BaseForeground",
                                                                                              typeof (Brush),
                                                                                              typeof (CodeBox),
                                                                                              new FrameworkPropertyMetadata
                                                                                                  (new SolidColorBrush(
                                                                                                       Colors.Black),
                                                                                                   FrameworkPropertyMetadataOptions
                                                                                                       .AffectsRender));

        private List<Decoration> _mDecorations = new List<Decoration>();

        private bool _mScrollingEventEnabled;

        public CodeBox()
        {
            TextChanged += txtTest_TextChanged;
            Foreground = new SolidColorBrush(Colors.Transparent);
            Background = new SolidColorBrush(Colors.Transparent);
            InitializeComponent();
        }

        public Brush BaseForeground
        {
            get { return (Brush) GetValue(BaseForegroundProperty); }
            set { SetValue(BaseForegroundProperty, value); }
        }


        /// <summary>
        /// List of the Decorative attributes assigned to the text
        /// </summary>
        public List<Decoration> Decorations
        {
            get { return _mDecorations; }
            set { _mDecorations = value; }
        }

        private void txtTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateVisual();
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            if (Text == "") return;
            EnsureScrolling();
            var formattedText = new FormattedText(
                Text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(FontFamily.Source),
                FontSize,
                BaseForeground); //Text that matches the textbox's
            var leftMargin = 4.0 + BorderThickness.Left;
            var topMargin = 2 + BorderThickness.Top;
            formattedText.MaxTextWidth = ViewportWidth; // space for scrollbar
            formattedText.MaxTextHeight = Math.Max(ActualHeight + VerticalOffset, 0); //Adjust for scrolling
            drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));

            //TextColor
            foreach (var dec in _mDecorations)
            {
                if (dec.DecorationType != EDecorationType.TextColor) continue;
                var ranges = dec.Ranges(Text);
                foreach (var p in ranges)
                {
                    formattedText.SetForegroundBrush(dec.Brush, p.Start, p.Length);
                }
            }
            drawingContext.DrawText(formattedText, new Point(leftMargin, topMargin - VerticalOffset));
        }

        private void EnsureScrolling()
        {
            if (_mScrollingEventEnabled) return;
            var dp = VisualTreeHelper.GetChild(this, 0);
            var sv = VisualTreeHelper.GetChild(dp, 0) as ScrollViewer;
            sv.ScrollChanged += ScrollChanged;
            _mScrollingEventEnabled = true;
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            InvalidateVisual();
        }
    }
}