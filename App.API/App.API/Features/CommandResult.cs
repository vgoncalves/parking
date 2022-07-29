namespace App.API.Features
{
    public class CommandResult<T>
    {
        private CommandResult() { }

        public static CommandResult<T> Success(T payload) => new CommandResult<T>() { Payload = payload };

        public static CommandResult<T> Fail(IEnumerable<string> errors) => new CommandResult<T>() { Errors = errors };

        public T? Payload { get; set; }

        public bool IsSuccess => !Errors.Any();

        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
}
