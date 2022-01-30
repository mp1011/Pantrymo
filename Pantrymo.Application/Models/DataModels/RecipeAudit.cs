using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class RecipeAudit
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public DateTime AuditDate { get; set; }
        public bool ValidUrl { get; set; }
        public bool ValidImage { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
