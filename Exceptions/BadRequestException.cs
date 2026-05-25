using System.Net;

namespace Graduation_Project.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message)
            : base(message, (int)HttpStatusCode.BadRequest)
        {
        }
    }
}