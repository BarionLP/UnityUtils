using System;

namespace Ametrin.Utils.Optional
{
    public readonly struct Result<T>
    {
        public ResultFlag Status { get; }
        private T Value { get; }

        private Result(ResultFlag status, T value)
        {
            Status = status;
            Value = value;
        }

        public static Result<T> Of(T value)
        {
            if (value is null) return Failed(ResultFlag.Null);
            return new(ResultFlag.Succeeded, value);
        }
        public static Result<T> Failed(ResultFlag status = ResultFlag.Failed)
        {
            if (status.IsSuccess()) throw new ArgumentException("Cannot Succeed without value! Use Result.Of", nameof(status));
            return new(status, default);
        }

        public readonly bool HasFailed => Status.IsFail();
        public readonly bool IsSuccess => Status.IsSuccess();

        public readonly void Resolve(Action<T> success, Action<ResultFlag> failed = null)
        {
            if (HasFailed)
            {
                failed?.Invoke(Status);
                return;
            }

            success(Value!);
        }

        public readonly Result<TResult> Map<TResult>(Func<T, TResult> map) => IsSuccess ? map(Value!) : Result<TResult>.Failed(Status);
        public readonly Result<TResult> Map<TResult>(Func<T, Result<TResult>> map) => IsSuccess ? map(Value!) : Result<TResult>.Failed(Status);
        public readonly Result<TResult> Map<TResult>(Func<T, Option<TResult>> map) => IsSuccess ? map(Value!).ToResult(Status) : Result<TResult>.Failed(Status);
        public readonly Result<TResult> Map<TResult>(Func<T, TResult> map, Func<ResultFlag, TResult> error) => IsSuccess ? map(Value!) : error(Status);

        public T Reduce(Func<ResultFlag, T> operation) => IsSuccess ? Value! : operation(Status);
        public T Reduce(Func<T> operation) => IsSuccess ? Value! : operation();
        public T Reduce(T @default) => IsSuccess ? Value! : @default;
        public T ReduceOrThrow() => IsSuccess ? Value! : throw new NullReferenceException($"Result was empty: {Status}");


        public static implicit operator Result<T>(ResultFlag status) => Failed(status);
        public static implicit operator Result<T>(T value) => Of(value);
    }

    public static class ResultExtensions
    {
        public static bool IsFail(this ResultFlag flag) => flag.HasFlag(ResultFlag.Failed);
        public static bool IsSuccess(this ResultFlag flag) => flag is ResultFlag.Succeeded;
    }


    [Flags] //for fails first bit must be 1
    public enum ResultFlag
    {
        Succeeded = 0b0000000000000000000000000000000,
        Failed = 0b1000000000000000000000000000000,
        InvalidArgument = 0b1000000000000000000000000000001,
        IOError = 0b1000000000000000000000000000010,
        WebError = 0b1000000000000000000000000000100,
        Null = 0b1000000000000000000000000001000,
        ConnectionFailed = 0b1000000000000000000000000010000,
        AlreadyExists = 0b1000000000000000000000000100000,
        Canceled = 0b1000000000000000000000001000000,
        OutOfRange = 0b1000000000000000000000010000000,
        AccessDenied = 0b1000000000000000000001000000000,
        PathNotFound = IOError | Null,
        PathAlreadyExists = IOError | AlreadyExists,
        NoInternet = WebError | ConnectionFailed,
        InvalidFile = IOError | InvalidArgument,
    }
}