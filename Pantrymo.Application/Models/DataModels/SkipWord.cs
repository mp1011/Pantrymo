﻿using System;
using System.Collections.Generic;

namespace Pantrymo.Application.Models
{
    public partial class SkipWord
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Occurences { get; set; }
    }
}