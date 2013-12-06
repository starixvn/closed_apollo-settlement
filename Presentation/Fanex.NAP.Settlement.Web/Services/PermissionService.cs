// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.11.22">Created</change>
// </history>
namespace Fanex.NAP.Settlement.Web.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Settlement.Web.Configuration;

    /// <summary>
    /// Permission service.
    /// </summary>
    public class PermissionService
    {
        private static readonly IUserService _UserService;

        static PermissionService()
        {
            _UserService = new UserService();
        }

        /// <summary>
        /// Get Roll Access: get permission to access with each menu item of current user login.
        /// </summary>
        /// <param name="currentFunctions">Current Functions.</param>
        /// <returns>Return Dictionary.</returns>
        public static Dictionary<string, int> GetRoleAccess(IEnumerable<FunctionPolicy> currentFunctions)
        {
            Dictionary<string, int> roleAccess = new Dictionary<string, int>();

            foreach (FunctionPolicy functionPolicy in currentFunctions)
            {
                if (functionPolicy.Policy >= (int)Constant.Policy.Read)
                {
                    roleAccess.Add(functionPolicy.Function.Key, functionPolicy.Policy);
                }
            }

            return roleAccess;
        }

        public static bool HasHostPermission(string functionName)
        {
            return AppSession.RoleAccess != null &&
                   AppSession.RoleAccess.ContainsKey(functionName) &&
                   AppSession.RoleAccess[functionName] == (int)Constant.Policy.Host;
        }

        /// <summary>
        /// HasPermission: check that user has permission to access one menu link.
        /// </summary>
        /// <param name="functionName">Function Name.</param>
        /// <returns>Return result check permission.</returns>
        public static bool HasReadPermission(string functionName)
        {
            return AppSession.RoleAccess != null &&
                   AppSession.RoleAccess.ContainsKey(functionName) &&
                   AppSession.RoleAccess[functionName] == (int)Constant.Policy.Read;
        }

        public static bool HasPermission(string functionName)
        {
            return AppSession.RoleAccess != null &&
                   AppSession.RoleAccess.ContainsKey(functionName) &&
                   AppSession.RoleAccess[functionName] >= (int)Constant.Policy.Read;
        }
    }
}