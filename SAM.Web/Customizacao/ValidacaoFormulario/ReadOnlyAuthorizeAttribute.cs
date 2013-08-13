using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAM.Web.Autenticacao;
//TODO: REMOVER
namespace SAM.Web.Customizacao.ValidacaoFormulario
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnlyAuthorizeAttribute : Attribute, IMetadataAware
    {

        public String Roles { get; set; }
        public bool IsReadOnly
        {
            get
            {
                if (this.Roles != null)
                {
                    var roleList = this.Roles.Split(',').Select(o => o.Trim()).ToList();
                    return !(roleList.Where(role => new UsuarioAutenticado(((UsuarioAutenticado)HttpContext.Current.User).Login).IsInRole(role)).Count() > 0);
                }
                else
                    return true;
            }
        }
        
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["IsReadOnly"] = this.IsReadOnly;
        }
    }
}