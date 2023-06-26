using System;

namespace Ametrin.Utils{
    #nullable enable
    public sealed class Result<T>{
        public readonly ResultStatus Status = ResultStatus.Failed;
        private T? Value { get; } = default;

        private Result(ResultStatus status){
            Status = status;
        }
        private Result(T? value) : this(ResultStatus.Succeeded){
            Value = value;
        }

        public static Result<T> Succeeded(in T value){
            if (value is null) throw new ArgumentNullException(nameof(value), "Cannot succeed when result is null");
            return new Result<T>(value);
        }

        public static Result<T> Failed(ResultStatus status = ResultStatus.Failed){
            if (status is ResultStatus.Succeeded) throw new ArgumentException("Cannot Succeed without value! Use Result.Succeeded", nameof(status));
            return new(status);
        }

        public void Resolve(Action<T> success, Action<ResultStatus>? failed = null) {
            if(!HasFailed()) {
                success(Value!);
                return;
            }

            if(failed is not null) {
                failed(Status);
            }
        }

        public T Get() => Value!;
        public bool HasFailed() => Status.HasFlag(ResultStatus.Failed);

        public static implicit operator Result<T>(ResultStatus status) => Result<T>.Failed(status);
        public static implicit operator Result<T>(T value){
            if(value is null) return Result<T>.Failed(ResultStatus.ResultNull);
            return Result<T>.Succeeded(value);
        }
    }

    public sealed class Result    {
        public readonly ResultStatus Status = ResultStatus.Failed;

        private Result(ResultStatus status){
            Status = status;
        }

        public void Resolve(Action success, Action<ResultStatus> failed) {
            if(HasFailed()) {
                failed(Status);
                return;
            }

            success();
        }

        public bool HasFailed() => Status.HasFlag(ResultStatus.Failed);
        public static Result Of(ResultStatus status) => new(status);
        public static implicit operator Result(ResultStatus status) =>  Of(status);
    }

    [Flags] //for fails first bit must be 1
    public enum ResultStatus{
        Succeeded = 0b00000,
        Failed = 0b10000,
        InvalidArgument = 0b10001,
        //FailedOperation     = 0b10010, //replace
        PathDoesNotExist = 0b10011,
        ResultNull = 0b10100,
        ValueDoesNotExist = 0b10101,
        AlreadyExists = 0b10110,
        Canceled = 0b10111,
        OutOfRange = 0b11000
    }
}
