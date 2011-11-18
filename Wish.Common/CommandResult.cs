using System;

namespace Wish.Common
{
    public class CommandResult
    {
        public bool IsExit { get; set; }
        // indates wish has processed this fully
        public bool FullyProcessed { get; set; }
        // indicates the event itself is handled and should not be processed by any default methods
        public bool Handled { get; set; }
        public string Text { get; set; }
        public string Error { get; set; }
        public string WorkingDirectory { get; set; }
        public int PromptLength { get; set; }
        public State State { get; set; }
    }

    public enum State { Normal, Tabbing }
}
