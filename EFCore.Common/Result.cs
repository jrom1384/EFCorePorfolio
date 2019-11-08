namespace EFCore.Common
{
    public class Result<T>
    {
        public Result(T value)
        {
            this.IsSuccess = true;
            this.Value = value;
        }

        public Result(string userDefinedError)
        {
            this.IsSuccess = false;
            this.ErrorType = ErrorType.Defined;
            this.HResult = 0;
            this.ErrorMessage = userDefinedError;
        }

        //For status check only no return Type values.
        public Result(bool isSuccess = true)
        {
            this.IsSuccess = isSuccess;
        }

        public Result(string exceptionErrorMessage, int hResult = 0)
        {
            this.IsSuccess = false;
            this.ErrorType = ErrorType.UnhandledException;
            this.HResult = hResult;
            this.ErrorMessage = exceptionErrorMessage;
        }

        public Result(ErrorType errorType, int hResult = 0)
        {
            this.IsSuccess = false;
            this.ErrorType = errorType;
            this.HResult = hResult;
        }

        //Success properties
        public bool IsSuccess { get; set; } = false;

        public T Value { get; set; }

        //Failed Properties
        public int HResult { get; set; } = 0;

        public string ErrorMessage { get; set; } = string.Empty;

        public ErrorType ErrorType { get; set; } = ErrorType.UnhandledException;
    }
}
