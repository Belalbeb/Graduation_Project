using System.Net;

namespace Graduation_Project.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
            : base(message, (int)HttpStatusCode.NotFound)
        {
        }
    }
}