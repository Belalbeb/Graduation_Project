using System.Net;

namespace Graduation_Project.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message)
            : base(message, (int)HttpStatusCode.Unauthorized)
        {
        }
    }
}