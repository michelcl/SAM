using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SAM.Web.Autenticacao
{
    public class CustomRoleProvider : RoleProvider
    {
        #region Metodos RoleProvider nao implementados
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
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
        #endregion

        public CustomRoleProvider()
            : base()
        {
        }

        public override bool IsUserInRole(string login, string roleName)
        {
            //var user = User;
            //if (user != null)
            //    return user.IsInRole(roleName);
            //else
                return false;
        }
        public override string[] GetRolesForUser(string login)
        {
            /*
            var user = new User(1, "login", "nome", "senha"); //new Negocio.Usuario().Listar(login);
            user.Role = new Role(1, "Papel 1");
            var listaRights = new List<Right>();
            listaRights.Add(new Right(1, "Direito 1"));
            listaRights.Add(new Right(2, "Direito 2"));
            listaRights.Add(new Right(3, "Direito 3"));
            user.Role.Rights = listaRights;

            string[] roles = new string[user.Role.Rights.Count + 1];
            roles[0] = user.Role.Description;
            int idx = 0;
            foreach (Right right in user.Role.Rights)
                roles[++idx] = right.Description;
            return roles;
            */
            
            var papeis = new Negocio.Papel().Listar(login).ToList();
            string[] roles = new string[papeis.Count];

            int idx = 0;
            foreach (var papel in papeis)
                roles[idx++] = papel.Descricao;
            return roles;
        }
    }
}