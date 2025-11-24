namespace SurvayBacket.Api.Abstractions
{
    public class Result
    {

        public Result( bool isSussecc , Error error)
        {
             if((isSussecc && error != Error.None) || (!isSussecc && error == Error.None))
                throw new InvalidOperationException();

            IsSuccess = isSussecc;
            Error = error;
        }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; } =default!;

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new (value,true ,Error.None);
        public static Result<Tvalue> Failure<Tvalue>(Error error) => new (default!, false,error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public Result( TValue tvalue,bool isSuccess,Error error) : base(isSuccess, error)
        {
            _value = tvalue;
        }
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Failure Result can't have value");
      
    }
}
