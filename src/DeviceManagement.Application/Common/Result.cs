namespace DeviceManagement.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; private init; }
    public T? Data { get; private init; }
    public string ErrorMessage { get; private init; } = string.Empty;
    public IReadOnlyCollection<string> Errors { get; private init; } = [];

    private Result() { }

    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            Errors = [errorMessage]
        };
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        var errorList = errors.ToList();
        return new Result<T>
        {
            IsSuccess = false,
            ErrorMessage = string.Join(", ", errorList),
            Errors = errorList
        };
    }
}

public class Result
{
    public bool IsSuccess { get; private init; }
    public string ErrorMessage { get; private init; } = string.Empty;
    public IReadOnlyCollection<string> Errors { get; private init; } = [];

    private Result() { }

    public static Result Success()
    {
        return new Result
        {
            IsSuccess = true
        };
    }

    public static Result Failure(string errorMessage)
    {
        return new Result
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            Errors = [errorMessage]
        };
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        var errorList = errors.ToList();
        return new Result
        {
            IsSuccess = false,
            ErrorMessage = string.Join(", ", errorList),
            Errors = errorList
        };
    }
}
