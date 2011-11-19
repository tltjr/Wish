using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Wish
{
    public class CompletionData : ICompletionData
    {
        private readonly string _baseText;
        private readonly string _completion;

        public CompletionData(string baseText, string completion)
        {
            _baseText = baseText;
            _completion = completion;
        }

        public ImageSource Image
        {
            get { return null; }
        }

        public string Text
        {
            get { return _completion; }
        }

        public object Content
        {
            get { return _completion; }
        }

        public object Description
        {
            get { return _completion; }
        }

        public double Priority
        {
            get { return 0; }
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var offset = completionSegment.Offset - _baseText.Length;
            var length = _baseText.Length;
            textArea.Document.Replace(offset, length, _completion);
        }
    }
}
