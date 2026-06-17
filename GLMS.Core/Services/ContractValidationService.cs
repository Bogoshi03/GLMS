using GLMS.Core.Entities;
using GLMS.Core.Enums;
using GLMS.Core.Interfaces;

namespace GLMS.Core.Services
{
    public class ContractValidationService : IContractValidationService
    {
        public bool CanCreateRequest(Contract contract)
        {
            return contract.Status != ContractStatus.Expired
                && contract.Status != ContractStatus.OnHold;
        }
    }
}
