using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace crm.Controles
{
    /// <summary>
    /// Descripción breve de ImagenCuota
    /// </summary>
    public class ImagenCuota : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string empno;
            if (context.Request.QueryString["id"] != null)
                empno = context.Request.QueryString["id"].ToString();
            else
                throw new ArgumentException("No parameter specified");

            context.Response.ContentType = "jpeg/bmp";
            Stream strm = ShowEmpImage(empno);
            if (strm != null)
            {
                byte[] buffer = new byte[4096];
                int byteSeq = strm.Read(buffer, 0, 4096);

                while (byteSeq > 0)
                {
                    context.Response.OutputStream.Write(buffer, 0, byteSeq);
                    byteSeq = strm.Read(buffer, 0, 4096);
                }
                context.Response.BinaryWrite(buffer);
            }
        }

        public Stream ShowEmpImage(string empno)
        {
            try
            {
                cImagenCuota imagen = cImagenCuota.GetImagenById(empno);
                return new MemoryStream((byte[])imagen.Imagen);

                //cImagenCuota imagen = cImagenCuota.GetImagenByCuota(empno);
                //return new MemoryStream((byte[])imagen.Imagen);
            }
            catch
            {
                return null;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}