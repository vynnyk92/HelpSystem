using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Helpers;
using System.Web.WebPages;
//using Microsoft.I
using HelpDesk.WebUI.Entities;
using HelpDesk.WebUI.Concrete;

namespace HelpDesk.WebUI.Providers
{
    public class HelpDeskRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string login)
        {
            string[] role = new string[] { };
            using (HelpdeskContext db = new HelpdeskContext())
            {
                try
                {
                    User user = db.Users.Where(u => u.Login == login).FirstOrDefault();
                    if (user != null)
                    {
                        Role userRole = db.Roles.Find(user.RoleId);
                        if (userRole != null)
                        {
                            role = new string[] { userRole.Name };
                        }
                    }
                }
                catch { role = new string[] { }; }
                return role;
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;

            using (HelpdeskContext db = new HelpdeskContext())
            {
                try
                {
                    User user = (from u in db.Users
                                 where u.Login == username
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        Role userRole = db.Roles.Find(user.RoleId);
                        if (userRole != null && userRole.Name == roleName)
                        {
                            outputResult = true;
                        }
                    }
                }
                catch (Exception)
                {

                    outputResult=false;
                }
                return outputResult;
            }
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

       

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

      

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}