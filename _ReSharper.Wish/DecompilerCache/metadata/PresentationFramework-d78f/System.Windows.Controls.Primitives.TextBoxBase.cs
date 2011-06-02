// Type: System.Windows.Controls.Primitives.TextBoxBase
// Assembly: PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\PresentationFramework.dll

using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof (FrameworkElement))]
    [Localizability(LocalizationCategory.Text)]
    public abstract class TextBoxBase : Control
    {
        public static readonly DependencyProperty IsReadOnlyProperty;
        public static readonly DependencyProperty IsReadOnlyCaretVisibleProperty;
        public static readonly DependencyProperty AcceptsReturnProperty;
        public static readonly DependencyProperty AcceptsTabProperty;
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty;
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty;
        public static readonly DependencyProperty IsUndoEnabledProperty;
        public static readonly DependencyProperty UndoLimitProperty;
        public static readonly DependencyProperty AutoWordSelectionProperty;
        public static readonly DependencyProperty SelectionBrushProperty;
        public static readonly DependencyProperty SelectionOpacityProperty;
        public static readonly DependencyProperty CaretBrushProperty;
        public static readonly RoutedEvent TextChangedEvent;
        public static readonly RoutedEvent SelectionChangedEvent;
        public bool IsReadOnly { get; set; }
        public bool IsReadOnlyCaretVisible { get; set; }
        public bool AcceptsReturn { get; set; }
        public bool AcceptsTab { get; set; }
        public SpellCheck SpellCheck { get; }
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }
        public double ExtentWidth { get; }
        public double ExtentHeight { get; }
        public double ViewportWidth { get; }
        public double ViewportHeight { get; }
        public double HorizontalOffset { get; }
        public double VerticalOffset { get; }
        public bool CanUndo { get; }
        public bool CanRedo { get; }
        public bool IsUndoEnabled { get; set; }
        public int UndoLimit { get; set; }
        public bool AutoWordSelection { get; set; }
        public Brush SelectionBrush { get; set; }
        public double SelectionOpacity { get; set; }
        public Brush CaretBrush { get; set; }
        public void AppendText(string textData);
        public override void OnApplyTemplate();

        [SecurityCritical]
        public void Copy();

        [SecurityCritical]
        public void Cut();

        public void Paste();
        public void SelectAll();
        public void LineLeft();
        public void LineRight();
        public void PageLeft();
        public void PageRight();
        public void LineUp();
        public void LineDown();
        public void PageUp();
        public void PageDown();
        public void ScrollToHome();
        public void ScrollToEnd();
        public void ScrollToHorizontalOffset(double offset);
        public void ScrollToVerticalOffset(double offset);
        public bool Undo();
        public bool Redo();
        public void LockCurrentUndoUnit();
        public void BeginChange();
        public void EndChange();
        public IDisposable DeclareChangeBlock();
        protected virtual void OnTextChanged(TextChangedEventArgs e);
        protected virtual void OnSelectionChanged(RoutedEventArgs e);
        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate);
        protected override void OnMouseWheel(MouseWheelEventArgs e);
        protected override void OnPreviewKeyDown(KeyEventArgs e);
        protected override void OnKeyDown(KeyEventArgs e);
        protected override void OnKeyUp(KeyEventArgs e);
        protected override void OnTextInput(TextCompositionEventArgs e);
        protected override void OnMouseDown(MouseButtonEventArgs e);
        protected override void OnMouseMove(MouseEventArgs e);
        protected override void OnMouseUp(MouseButtonEventArgs e);
        protected override void OnQueryCursor(QueryCursorEventArgs e);
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e);
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e);
        protected override void OnDragEnter(DragEventArgs e);
        protected override void OnDragOver(DragEventArgs e);
        protected override void OnDragLeave(DragEventArgs e);
        protected override void OnDrop(DragEventArgs e);
        protected override void OnContextMenuOpening(ContextMenuEventArgs e);
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e);
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e);
        protected override void OnLostFocus(RoutedEventArgs e);
        public event TextChangedEventHandler TextChanged;
        public event RoutedEventHandler SelectionChanged;
    }
}
