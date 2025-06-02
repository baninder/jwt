namespace JwtAuthApi.Core.Common
{
    /// <summary>
    /// Generic result pattern for operation outcomes
    /// </summary>
    /// <typeparam name="T">Result data type</typeparam>
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }
        public string? ErrorCode { get; private set; }
        public Dictionary<string, string[]>? ValidationErrors { get; private set; }

        private Result(bool isSuccess, T? data, string? errorMessage, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public static Result<T> Success(T data) => new(true, data, null);
        
        public static Result<T> Failure(string errorMessage, string? errorCode = null) => 
            new(false, default, errorMessage, errorCode);
        
        public static Result<T> ValidationFailure(Dictionary<string, string[]> validationErrors) =>
            new(false, default, "Validation failed")
            {
                ValidationErrors = validationErrors
            };

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
        {
            return IsSuccess ? onSuccess(Data!) : onFailure(ErrorMessage!);
        }
    }

    /// <summary>
    /// Non-generic result for operations without return data
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public string? ErrorCode { get; private set; }

        private Result(bool isSuccess, string? errorMessage, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public static Result Success() => new(true, null);
        
        public static Result Failure(string errorMessage, string? errorCode = null) => 
            new(false, errorMessage, errorCode);

        public static implicit operator Result(bool success) => 
            success ? Success() : Failure("Operation failed");
    }
}
