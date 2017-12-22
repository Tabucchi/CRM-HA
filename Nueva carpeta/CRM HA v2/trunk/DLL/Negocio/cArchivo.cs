using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cArchivo
    {
        private string id;
        private string descripcion;
        private string extension;
        private byte[] archivo;
        private DateTime fecha;        
        private string idProyecto;
        private Int16 _papelera;
        
        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }
        public byte[] Archivo
        {
            get { return archivo; }
            set { archivo = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string IdProyecto
        {
            get { return idProyecto; }
            set { idProyecto = value; }
        }

        public Int16 Papelera
        {
            get { return _papelera; }
            set { _papelera = value; }
        }
        #endregion

        public int Save()
        {
            cArchivoDAO DAO = new cArchivoDAO();
            return DAO.Save(this);
        }

        public static cArchivo Load(string id)
        {
            cArchivoDAO DAO = new cArchivoDAO();
            return DAO.Load(id);
        }

        public cArchivo()
        {
        }

        public cArchivo(string _descripcion, string _extension, byte[] _archivo, string _idProyecto)
        {
            descripcion = _descripcion;
            extension = _extension;
            archivo = _archivo;
            fecha = DateTime.Now;
            idProyecto = _idProyecto;
            _papelera = (Int16)papelera.Activo;
            this.Save();
        }
        
        public static byte[] GetFile(string idProyecto)
        {
            cArchivoDAO DAO = new cArchivoDAO();
            return DAO.GetFile(idProyecto);
        }

        public byte[] GetFile2
        {
            get
            {
                cArchivoDAO DAO = new cArchivoDAO();
                return DAO.GetFile2;
            }
        }

        /*
         * 
            string path3 = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Unidades\\asd.xlsx";
            byte[] excelContents;
            cArchivo DAO = new cArchivo();
            excelContents = DAO.GetFile2;
            File.WriteAllBytes(path3, excelContents);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Unidades\\asd.xlsx";
            cProyecto proyecto = cProyecto.Load(Request["idProyecto"].ToString());
            string filename = proyecto.Descripcion;

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            try
            {
                Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename + ".xls");
            }
            catch
            {
                Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename + ".xlsx");
            }

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
         */
    }
}
