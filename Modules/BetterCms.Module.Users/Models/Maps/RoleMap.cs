﻿using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class RoleMap : EntityMapBase<Role>
    {
        public RoleMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("Roles");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
        }
    }
}