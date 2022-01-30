using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pantrymo.Application.Models
{
    public partial class ComponentHierarchy
    {
        public int ComponentId { get; set; }
        public HierarchyId HierarchyId { get; set; }
        public int Id { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Component Component { get; set; }
    }
}
