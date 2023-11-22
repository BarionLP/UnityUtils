using System;

namespace Ametrin.Utils{
    #nullable enable 
    public readonly struct Result<T>{

        public  ResultFlag Status { get; }
        private T? Value { get; }

        public Result(ResultFlag status, T? value){
            Status = status;
            Value = value;
        }

        public static Result<T> Of(T value){
            if (value is null) return Failed(ResultFlag.Null);
            return new(ResultFlag.Succeeded, value);
        }
        public static Result<T> Failed(ResultFlag status = ResultFlag.Failed){
            if (status is ResultFlag.Succeeded) throw new ArgumentException("Cannot Succeed without value! Use Result.Succeeded", nameof(status));
            return new(status, default);
        }

        public readonly bool HasFailed => Status.HasFlag(ResultFlag.Failed);
        public readonly bool IsSuccess => Status is ResultFlag.Succeeded;

        public readonly void Resolve(Action<T> success, Action<ResultFlag>? failed = null){
            if (HasFailed){
                failed?.Invoke(Status);
                return;
            }

            success(Value!);
        }

        // [Obsolete] //still need it when can't use lambda (e.g. Spans)
        public bool TryResolve(out T result){
            result = Value!;
            return !HasFailed;
        }

        public readonly Result<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class{
            return HasFailed ? Result<TResult>.Failed(Status) : map(Value!);
        }
        public readonly Result<TResult> Map<TResult>(Func<T, Result<TResult>> map) where TResult : class{
            return HasFailed ? Result<TResult>.Failed(Status) : map(Value!);
        }
        public readonly Result<TResult> Map<TResult>(Func<T, TResult> map, Func<ResultFlag, TResult> error) where TResult : class
        {
            return HasFailed ? error(Status) : map(Value!);
        }

        public T Reduce(Func<ResultFlag, T> operation) => IsSuccess ? Value! : operation(Status);
        public T Reduce(Func<T> operation) => IsSuccess ? Value! : operation();
        public T Reduce(T @default) => IsSuccess ? Value! : @default;
        public T? ReduceOrNull() => IsSuccess ? Value : default;
        public T ReduceOrThrow() => IsSuccess ? Value! : throw new NullReferenceException($"Result was empty: {Status}");        
        
        public static implicit operator Result<T>(ResultFlag status) => Result<T>.Failed(status);
        public static implicit operator Result<T>(T value) => Result<T>.Of(value);
    }

    // [Obsolete] //will be replaced by StatusFlags with extension methods
    public sealed class Result{
        public readonly ResultFlag Status = ResultFlag.Failed;

        private Result(ResultFlag status)
        {
            Status = status;
        }

        public bool HasFailed() => Status.HasFlag(ResultFlag.Failed);
        public bool IsSuccess() => Status is ResultFlag.Succeeded;

        public void Resolve(Action success, Action<ResultFlag>? failed = null){
            if (HasFailed()){
                failed?.Invoke(Status);
                return;
            }

            success();
        }

        public TReturn Resolve<TReturn>(Func<TReturn> success, Func<ResultFlag, TReturn> failed) => HasFailed() ? failed(Status) : success();
        public TReturn Resolve<TReturn>(Func<TReturn> success, TReturn @default) => HasFailed() ? @default : success();
        public bool Catch(Action<ResultFlag> operation){
            if (HasFailed()) operation(Status);
            return HasFailed();
        }

        public static Result Of(ResultFlag status) => new(status);
        public static Result<T> Of<T>(T value) where T : class => Result<T>.Of(value);
        public static implicit operator Result(ResultFlag status) => Of(status);
    }
    #nullable disable

    [Flags] //for fails first bit must be 1
    public enum ResultFlag{
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
