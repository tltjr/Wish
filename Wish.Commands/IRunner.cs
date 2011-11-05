namespace Wish.Commands
{
    public interface IRunner
    {
        string Execute(string line);
        string WorkingDirectory { get; }
    }
}
