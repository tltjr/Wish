using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace GuiHelpers
{
    public class TabCompletionData : ICompletionData
    {
        public TabCompletionData(string text, string textToReplace)
        {
            Text = text;
            Replace = textToReplace;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }
        public string Replace { get; private set; }

        public object Content
        {
            get { return Text; }
        }

        public object Description
        {
            get { return Text; }
        }

        public double Priority { get { return 0; } }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var offset = completionSegment.Offset - Replace.Length;
            var length = Replace.Length;
            textArea.Document.Replace(offset, length, Text);
        }
    }
}
