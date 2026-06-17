using GLMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GLMS.Core.Entities
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public int ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Required]
        public string? Description { get; set; }

        public decimal CostUSD { get; set; }

        public decimal CostZAR { get; set; }

        public RequestStatus Status { get; set; }
    }
}
