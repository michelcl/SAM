using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simple.ImageResizer.MvcExtensions;

namespace SAM.Web.Controllers.Comum
{
    public class ImageController : Controller
    {
        [OutputCache(VaryByParam = "*", Duration = 60 * 60 * 24)]
        public ImageResult Index(string filename, int w = 100, int h = 100)
        {
            string filepath = Path.Combine(Server.MapPath("/Upload/Files/"), filename);
            return new ImageResult(filepath, w, h);
        }
    }
}
