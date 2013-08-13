using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Util
{
    /// <summary>
    /// Classe responsável para extender os métodos de C#
    /// </summary>
    public static class Extensions
    {
        public static string FormatarCNPJ(this string cnpj)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(cnpj))
            {
                if (cnpj.Length == 14)
                    result = cnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");

                if (cnpj.Length == 11)
                    result = cnpj.Insert(3, ".").Insert(7, ".").Insert(11, "-");

                if ((cnpj.Length != 11) && (cnpj.Length != 14))
                    result = cnpj;
            }
            return result;
        }

        /// <summary>
        /// <para>Verifica se a string passada por parametro contem um trecho da validada.</para>
        /// <para>A comparação é completa. Removido acentos, tils, cedilhas e formatação da caixa.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="textoPesquisa">Texto a ser validado</param>
        /// <returns>bool</returns>
        public static bool ContainsIgnoraAcento(this string texto, string textoPesquisa)
        {
            if (!string.IsNullOrEmpty(textoPesquisa))
                textoPesquisa = RemoveDiacritics(textoPesquisa).ToLowerInvariant();

            if (!string.IsNullOrEmpty(texto))
                texto = RemoveDiacritics(texto).ToLowerInvariant();

            if (!string.IsNullOrEmpty(texto) && texto.Contains(textoPesquisa))
                return true;

            return false;
        }

        /// <summary>
        /// Método resposável para remover acentos
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string RemoveDiacritics(string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }
    }
}
