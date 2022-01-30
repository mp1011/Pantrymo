using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class ScraperHint
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Selector { get; set; }
        public byte HintType { get; set; }

        public virtual Site Site { get; set; }
    }
}
