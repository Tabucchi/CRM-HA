using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cComentarioMorososDAO
    {
        public string GetTable
        { get { return "tComentarioMorosos"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cComentarioMorosos morosos)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCuentaCorriente", morosos.IdCuentaCorriente));
            lista.Add(new cAtributo("comentario", morosos.Comentario));
            lista.Add(new cAtributo("fecha", morosos.Fecha));
            lista.Add(new cAtributo("idUsuario", morosos.IdUsuario));
            return lista;
        }

        public int Save(cComentarioMorosos comentario)
        {
            if (string.IsNullOrEmpty(comentario.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(comentario));
            else
                return cDataBase.GetInstance().UpdateObject(comentario.Id, GetTable, AttributesClass(comentario));
        }

        public cComentarioMorosos Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cComentarioMorosos morosos = new cComentarioMorosos();
            morosos.Id = Convert.ToString(atributos["id"]);
            morosos.IdCuentaCorriente = Convert.ToString(atributos["idCuentaCorriente"]);
            morosos.Comentario = Convert.ToString(atributos["comentario"]);
            morosos.Fecha = Convert.ToDateTime(atributos["fecha"]);
            morosos.IdUsuario = Convert.ToString(atributos["idUsuario"]);
            return morosos;
        }

        public List<cComentarioMorosos> GetComentariosByIdCCC(string _idCC)
        {
            List<cComentarioMorosos> comentarios = new List<cComentarioMorosos>();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuentaCorriente=" + _idCC + " Order by Fecha ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                comentarios.Add(Load(Convert.ToString(idList[i])));
            }
            return comentarios;
        }
    }
}
