using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SAM.Web.Controllers.Comum
{
    public class UploadController : Controller
    {
        [HttpPost]
        public ActionResult UploadFiles()
        {
            foreach (var item in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    UploadWholeFile(Request, statuses);
                /*
                else
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                */
                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";
                return result;
            }
            return Json(new List<ViewDataUploadFilesResult>());
        }

        #region Private
        private string StorageRoot
        {
            get
            {
                return Path.Combine(Server.MapPath("/Upload/Files/"));
                /*var diretorioPadrao = Server.MapPath("");
                var diretorioImagem = "/Files";
                if (!System.IO.File.Exists(diretorioPadrao + diretorioImagem))
                {
                    var d = diretorioPadrao;
                    foreach (string item in diretorioImagem.Split('/'))
                    {
                        if (!System.IO.File.Exists(d + item))
                        {
                            d += "/" + item;
                            System.IO.Directory.CreateDirectory(d);
                        }
                    }
                    return Path.Combine(d);
                }
                else
                {
                    return Path.Combine(diretorioPadrao + diretorioImagem);
                }
                 */
            }
        }

        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var guid = Guid.NewGuid();
                var extensao = new FileInfo(file.FileName).Extension;
                var nomeArquivoTemporario = guid + extensao;
                var fullPath = Path.Combine(StorageRoot, Path.GetFileName(nomeArquivoTemporario));
                file.SaveAs(fullPath);
                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = nomeArquivoTemporario,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/imagem/" + nomeArquivoTemporario,
                    delete_url = nomeArquivoTemporario,
                    delete_type = "GET",
                });
            }
        }
        #endregion
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }
}
