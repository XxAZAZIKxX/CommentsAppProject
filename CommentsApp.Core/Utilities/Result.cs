namespace CommentsApp.Core.Utilities;

public readonly struct Result<T> : IEquatable<Result<T>>
{
    private readonly T? _value;
    private readonly Exception? _exception;
    public T Value => _value ?? throw new InvalidOperationException("Value is not setted!");
    public Exception Exception => _exception ?? throw new InvalidOperationException("Exception is not setted!");

    public bool IsSuccessful { get; }
    public bool IsFailed => !IsSuccessful;

    public Result()
    {
        _exception = new Exception("Exception is not setted");
        IsSuccessful = false;
    }

    public Result(T value)
    {
        _value = value;
        IsSuccessful = true;
    }

    public Result(Exception exception)
    {
        _exception = exception;
        IsSuccessful = false;
    }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Exception exception) => new(exception);

    public TResult Match<TResult>(Func<T, TResult> ifSuccessful, Func<Exception, TResult> ifFailed)
    {
        return IsSuccessful ? ifSuccessful(_value!) : ifFailed(_exception!);
    }

    public bool Equals(Result<T> other)
    {
        return EqualityComparer<T?>.Default.Equals(_value, other._value) && Equals(_exception, other._exception);
    }

    public override bool Equals(object? obj)
    {
        return obj is Result<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, _exception);
    }

    public static bool operator ==(Result<T> left, Result<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<T> left, Result<T> right)
    {
        return !left.Equals(right);
    }
}