﻿using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public partial class Site : IWithLastModifiedDate, IWithName { }
    public partial class Component : IWithLastModifiedDate, IWithName { }
    public partial class AlternateComponentName : IWithLastModifiedDate { }
    public partial class Cuisine : IWithName { }
}
