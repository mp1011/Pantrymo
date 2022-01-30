using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class SchemaVersion
    {
        public int Id { get; set; }
        public string ScriptName { get; set; }
        public DateTime Applied { get; set; }
    }
}
