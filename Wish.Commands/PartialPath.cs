namespace Wish.Commands
{
    public class PartialPath
    {
        public string Text { get; set; }

        public PartialPath(string text)
        {
            Text = text;
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
