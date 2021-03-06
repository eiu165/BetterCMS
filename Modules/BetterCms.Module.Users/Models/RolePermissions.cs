﻿using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class RolePermissions : EquatableEntity<RolePermissions>
    {
        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }
        
    }
}