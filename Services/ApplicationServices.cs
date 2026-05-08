using Graduation_Project.Dtos;
using Graduation_Project.Repositories;

namespace Graduation_Project.Services
{
    public class ApplicationServices : IApplicationServivces
    {
        private readonly IApplicationRepository _repository;

        public ApplicationServices(IApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApplicationDto>> GetApplicationByApplicant(Guid id)
        {
            return await _repository.GetByApplicantIdAsync(id);
        }
    }
}
