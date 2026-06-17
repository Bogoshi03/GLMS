using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.Json.Serialization;

namespace GLMS.Core.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public string? ContactDetails { get; set; }

        public string? Region { get; set; }

        [JsonIgnore]
        public ICollection<Contract>? Contracts { get; set; }
    }
}

