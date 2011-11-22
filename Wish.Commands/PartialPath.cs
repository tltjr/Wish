namespace Wish.Commands
{
    public class PartialPath
    {
        public string Text { get; set; }
        public string CompletionTarget { get; set; }

        public PartialPath(string text)
        {
            CompletionTarget = text;
            Text = text.Replace("'", string.Empty);
        }

        public string Pattern
        {
            get { return Text + "*"; }
        }

        public string Base
        {
            get
            {
                var indexOfLastSlash = Text.LastIndexOf(@"\");
                return Text.Substring(0, indexOfLastSlash + 1);
            }
        }
    }
}
