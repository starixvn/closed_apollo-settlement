// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.11.22">Created</change>
// </history>

namespace Fanex.NAP.Settlement.Web.Configuration
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Settlement.Web.Constants;

    /// <summary>
    /// AppSession class.
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>Current User.</value>
        public static User CurrentUser
        {
            get
            {
                return SessionManager.Get<User>(SessionKey.CurrentLogInUser);
            }

            set
            {
                SessionManager.Set<User>(SessionKey.CurrentLogInUser, value);
            }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get
            {
                try
                {
                    return SessionManager.Get<Dictionary<string, int>>(SessionKey.CurrentUserRoleAccess);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            set
            {
                SessionManager.Set<Dictionary<string, int>>(SessionKey.CurrentUserRoleAccess, value);
            }
        }
    }
}