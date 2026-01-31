namespace BookLibrary.Ui.Services;

public record ApiResult<T>(T? Data, string? Error)
{
    public bool IsSuccess => Error is null;

    public static ApiResult<T> Success(T? data) => new(data, null);

    public static ApiResult<T> Failure(string error) => new(default, error);
}
