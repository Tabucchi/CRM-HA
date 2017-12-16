using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cImagenCuotaDAO
    {
        public string GetTable
        { get { return "tImagenCuota"; } }

        public List<cAtributo> AttributesClass(cImagenCuota imagen)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCC", imagen.IdCC));
            lista.Add(new cAtributo("idCuota", imagen.IdCuota));
            lista.Add(new cAtributo("descripcion", imagen.Descripcion));
            lista.Add(new cAtributo("imagen", imagen.Imagen));
            lista.Add(new cAtributo("papelera", imagen.Papelera));
            return lista;
        }

        public cImagenCuota Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cImagenCuota imagen = new cImagenCuota();
            imagen.Id = Convert.ToString(atributos["id"]);
            imagen.IdCC = Convert.ToInt32(atributos["idCC"]);
            imagen.IdCuota = Convert.ToString(atributos["idCuota"]);
            imagen.Descripcion = Convert.ToString(atributos["descripcion"]);
            imagen.Imagen = (byte[])atributos["imagen"];
            imagen.Papelera = Convert.ToInt16(atributos["papelera"]);
            return imagen;
        }

        public int Save(cImagenCuota imagen)
        {
            if (string.IsNullOrEmpty(imagen.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(imagen));
            else
                return cDataBase.GetInstance().UpdateObject(imagen.Id, GetTable, AttributesClass(imagen));
        }

        public string Existe(string _idCuota)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idCuota='" + _idCuota + "' AND papelera=1";

            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();

            string existe = null;
            if (!string.IsNullOrEmpty(cDataBase.GetInstance().ExecuteScalar(com)))
                existe = cDataBase.GetInstance().ExecuteScalar(com);
            else
                existe = null;

            return existe;
        }
        public string GetCancelarImagen(string _id)
        {
            string query = "DELETE FROM " + GetTable + " WHERE id=" + _id;
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public cImagenCuota GetImagenById(string _id)
        {
            string query = "SELECT id FROM tImagenCuota WHERE id='" + _id + "'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);

            if (id != null)
                return Load(Convert.ToString(id));
            else
                return null;
        }

        public cImagenCuota GetImagenByCuota(string _idCuota)
        {
            string query = "SELECT id FROM tImagenCuota WHERE idCuota='" + _idCuota + "'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);

            if (id != null)
                return Load(Convert.ToString(id));
            else
                return null;
        }

        public cImagenCuota GetImagenByRegistroPago(string _idRegistroPago)
        {
            string query = "SELECT i.id FROM tRegistroPago p INNER JOIN tImagenCuota i ON p.idImagen=i.id WHERE p.id='" + _idRegistroPago + "'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);

            if (id != null)
                return Load(Convert.ToString(id));
            else
                return null;
        }
    }
}
