using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class NavigationHint
    {
        public int Id { get; set; }
        public string Pattern { get; set; }
        public byte Priority { get; set; }
        public int SiteId { get; set; }

        public virtual Site Site { get; set; }
    }
}
