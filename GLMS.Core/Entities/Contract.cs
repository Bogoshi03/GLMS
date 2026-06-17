using System;
using System.Collections.Generic;
using System.Text;
using GLMS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GLMS.Core.Entities
{
    public class Contract
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public Client? Client { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ContractStatus Status { get; set; }

        public string? ServiceLevel { get; set; }

        public string? SignedAgreementPath { get; set; }

        [JsonIgnore]
        public ICollection<ServiceRequest>? ServiceRequests { get; set; }

        public string? FilePath { get; set; }
    }
}
