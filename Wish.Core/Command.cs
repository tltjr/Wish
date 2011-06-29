namespace Wish.Core {
	public class Command {
		public string Raw { get; private set; }
		public string Name { get; private set; }
		public string[] Args { get; private set; }

		public Command(string raw, string name, string[] args) {
			Raw = raw;
			Name = name;
			Args = args;
		}
	}
}
