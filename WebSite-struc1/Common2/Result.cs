namespace Common2
{
    public class Result<TSuccess, TError>
    {
        private Result(TSuccess success)
        {
            Success = success;
        }

        private Result(TError error)
        {
            Error = error;
        }

        public TError Error { get; }

        public TSuccess Success { get; }

        public static Result<TSuccess, TError> SuccessOf(TSuccess success)
        {
            return new Result<TSuccess, TError>(success);
        }

        public static Result<TSuccess, TError> ErrorOf(TError error)
        {
            return new Result<TSuccess, TError>(error);
        }
    }
}
