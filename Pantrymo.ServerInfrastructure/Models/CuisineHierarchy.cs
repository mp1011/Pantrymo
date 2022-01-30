using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pantrymo.Application.Models
{
    public partial class CuisineHierarchy
    {
        public int Id { get; set; }
        public int CuisineId { get; set; }
        public HierarchyId HierarchyId { get; set; }

        public virtual Cuisine Cuisine { get; set; }
    }
}
