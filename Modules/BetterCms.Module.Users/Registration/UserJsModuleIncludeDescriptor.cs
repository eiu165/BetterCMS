﻿using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Controllers;

namespace BetterCms.Module.Users.Registration
{
    /// <summary>
    /// bcms.user.js module descriptor.
    /// </summary>
    public class UserJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public UserJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.user")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<UserController>(this, "loadSiteSettingsUsersUrl", c => c.Index(null)),
                            new JavaScriptModuleLinkTo<UserController>(this, "loadEditUserUrl", c=> c.EditUser("{0}")), 
                            new JavaScriptModuleLinkTo<UserController>(this, "deleteUserUrl", c => c.DeleteUser("{0}", "{1}")),
                            /*new JavaScriptModuleLinkTo<RoleController>(this, "loadEditRoleUrl", c => c.EditRoleView("{0}")),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadSiteSettingsRoleUrl", c => c.RolesListView(null)) */
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "usersListTabTitle", () => UsersGlobalization.SiteSettings_Users_ListTab_Title), 
                            new JavaScriptModuleGlobalization(this, "usersAddNewTitle", () => UsersGlobalization.EditUser_Window_Title),
                            new JavaScriptModuleGlobalization(this, "rolesListTabTitle", () => UsersGlobalization.SiteSettings_Roles_ListTab_Title),
                            new JavaScriptModuleGlobalization(this, "rolesAddNewTitle", () => UsersGlobalization.Role_AddNeww_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "deleteUserConfirmMessage", () => UsersGlobalization.DeleteUser_Confirmation_Message)
                        };
        }
    }
}