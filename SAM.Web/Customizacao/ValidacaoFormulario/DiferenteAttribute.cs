using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAM.Web.Customizacao.ValidacaoFormulario
{
    //TODO: CRIAR UNOBTRUSIVE PARA A VALIDACAO DIFERENTE
    //http://www.falconwebtech.com/post/2012/04/21/MVC3-Custom-Client-Side-Validation-with-Unobtrusive-Ajax.aspx
    public class DiferenteAttribute : ValidationAttribute, IClientValidatable
    {
        public string PropName { get; set; }

        public DiferenteAttribute(string otherProperty)
        {
            PropName = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(PropName);

            if (otherPropertyInfo == null || value == null)
                return null;

            var otherPropertyStringValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null).ToString();
            if (otherPropertyStringValue == null)
                return null;

            if (Equals(value.ToString(), otherPropertyStringValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessage,
                ValidationType = "diferente"
            };
        }
    }
}