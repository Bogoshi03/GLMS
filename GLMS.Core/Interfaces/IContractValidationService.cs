using GLMS.Core.Entities;

namespace GLMS.Core.Interfaces
{
    public interface IContractValidationService
    {
        bool CanCreateRequest(Contract contract);
    }
}
