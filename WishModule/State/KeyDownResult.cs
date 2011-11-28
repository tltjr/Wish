namespace Wish.State
{
    public class KeyDownResult
    {
        public bool Handled { get; set; }
        public Common.State State { get; set; }
        public string Text { get; set; }
        public string PromptLength { get; set; }
        public bool FullyProcessed { get; set; }
    }
}
