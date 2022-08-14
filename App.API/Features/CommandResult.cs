namespace App.API.Features;

public record CommandResult<T>
{
    private CommandResult()
    {
    }

    public static CommandResult<T> Success(T? payload)
    {
        return new CommandResult<T>() { Payload = payload };
    }

    public static CommandResult<T> InvalidRequest(params string[] errors)
    {
        return new CommandResult<T>() { Errors = errors, FailureReason = FailureReason.InvalidRequest};
    }
    public T? Payload { get; init; }

    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    public FailureReason FailureReason { get; init; } = FailureReason.None;
}

public enum FailureReason
{
    None = 0,
    InvalidRequest,
    EntityNotFound,
}