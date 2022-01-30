using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class LatestRecipeAudit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Site { get; set; }
        public bool ValidImage { get; set; }
        public bool ValidUrl { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
