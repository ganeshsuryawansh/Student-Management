namespace StudentManagementSystem.Middleware
{
    public abstract class AppException : Exception
    {
        public abstract int StatusCode { get; }
        protected AppException(string message) : base(message) { }
    }

    public class NotFoundException : AppException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
        public NotFoundException(string message) : base(message) { }
    }

    public class BadRequestException : AppException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;
        public BadRequestException(string message) : base(message) { }
    }

    public class ConflictException : AppException
    {
        public override int StatusCode => StatusCodes.Status409Conflict;
        public ConflictException(string message) : base(message) { }
    }

    public class UnauthorizedAppException : AppException
    {
        public override int StatusCode => StatusCodes.Status401Unauthorized;
        public UnauthorizedAppException(string message) : base(message) { }
    }
}
