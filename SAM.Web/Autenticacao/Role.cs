using System.Collections.Generic;

namespace SAM.Web.Autenticacao
{
    public class Role
    {
        protected Role() { }
        public Role(int roleid, string roleDescription)
        {
            RoleId = roleid;
            Description = roleDescription;
        }
        public virtual int RoleId { get; set; }
        public virtual string Description { get; set; }
    }
}