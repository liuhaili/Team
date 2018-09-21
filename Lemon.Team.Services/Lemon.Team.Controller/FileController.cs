using Lemon.Team.DAL;
using Lemon.Team.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Lemon.Team.Controllers
{
    public class FileController : ControllerBase
    {
        [SessionValidate]
        public MyResult Upload(string par0, string par1, HttpPostedFileBase filedata)
        {
            string faceDir = System.Web.HttpContext.Current.Server.MapPath(@"~/" + par0);
            if (!Directory.Exists(faceDir))
                Directory.CreateDirectory(faceDir);
            string mapPath = par0 + "/" + par1;
            string facePath = faceDir + "/" + par1;

            Stream sm = filedata.InputStream;
            byte[] buffer = new byte[sm.Length];
            sm.Read(buffer, 0, buffer.Length);
            sm.Close();

            System.IO.File.WriteAllBytes(facePath, buffer);
            return ServiceResult(mapPath);
        }
    }
}
