using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class AlternateComponentName
    {
        public int Id { get; set; }
        public string AlternateName { get; set; }
        public int ComponentId { get; set; }

        public virtual Component Component { get; set; }
    }
}
