using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class CuisineLookup
    {
        public int Id { get; set; }
        public int CuisineId { get; set; }
        public string Text { get; set; }
    }
}
