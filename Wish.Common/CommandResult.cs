using ICSharpCode.AvalonEdit;

namespace Wish.Common
{
    public class CommandResult
    {
        public bool IsExit { get; set; }
        public bool Handled { get; set; }
        public string Text { get; set; }
        public string Error { get; set; }
        public string WorkingDirectory { get; set; }
        public int PromptLength { get; set; }

        public GuiStuff ProcessCommandResult(TextEditor textEditor)
        {
            string title = "";
            if (!Handled) return null;
            textEditor.Text = Text;
            var wdir = WorkingDirectory;
            if (null != wdir)
            {
                title = WorkingDirectory;
            }
            var promptLength = PromptLength;
            textEditor.ScrollToEnd();
            return new GuiStuff{ Title = title, PromptLength = promptLength};
        }

    }

    public class GuiStuff
    {
        public string Title { get; set; }
        public int PromptLength { get; set; }
    }
}
