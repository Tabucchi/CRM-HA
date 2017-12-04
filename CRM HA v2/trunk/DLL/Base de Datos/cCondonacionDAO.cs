using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cCondonacionDAO
    {
        public string GetTable
        { get { return "tCondonacion"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cCondonacion condonacion)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCuota", condonacion.IdCuota));
            lista.Add(new cAtributo("idItemCCU", condonacion.IdItemCCU));
            lista.Add(new cAtributo("nro", condonacion.Nro));
            lista.Add(new cAtributo("fecha", condonacion.Fecha));
            lista.Add(new cAtributo("monto", condonacion.Monto));
            lista.Add(new cAtributo("papelera", condonacion._Papelera));
            return lista;
        }

        public int Save(cCondonacion condonacion)
        {
            if (string.IsNullOrEmpty(condonacion.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(condonacion));
            else
                return cDataBase.GetInstance().UpdateObject(condonacion.Id, GetTable, AttributesClass(condonacion));
        }

        public cCondonacion Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cCondonacion condonacion = new cCondonacion();
            condonacion.Id = Convert.ToString(atributos["id"]);
            condonacion.IdCuota = Convert.ToString(atributos["idCuota"]);
            condonacion.IdItemCCU = Convert.ToString(atributos["idItemCCU"]);
            condonacion.Nro = Convert.ToInt32(atributos["nro"]);
            condonacion.Fecha = Convert.ToDateTime(atributos["fecha"]);
            condonacion.Monto = Convert.ToDecimal(atributos["monto"]);
            condonacion._Papelera = Convert.ToInt16(atributos["papelera"]);
            return condonacion;
        }

        public cCondonacion LoadByNro(string _nro)
        {
            cCondonacion unidad = new cCondonacion();
            string query = "SELECT id FROM tCondonacion WHERE nro ='" + _nro + "'";
            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null)
                return null;
            unidad = Load(idList);
            return unidad;
        }

        public Int64 GetLastNro()
        {
            string query = "SELECT TOP (1) nro FROM " + GetTable + " ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            string result = cDataBase.GetInstance().ExecuteScalar(com);
            if (result == null)
                result = "24999";

            return Convert.ToInt64(result);
        }

        //public string GetReciboByIdCuota(string _idCuota)
        //{
        //    string query = "SELECT nro FROM " + GetTable + " WHERE idCuota='" + _idCuota + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
        //    SqlCommand com = new SqlCommand();
        //    com.CommandText = query.ToString();
        //    return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        //}

        public string GetNroCondonacionByIdItemCCU(string _idItemCCU)
        {
            //string query = "SELECT nro FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            string query = "SELECT nro FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        //public decimal GetMonto(string _idItemCCU)
        //{
        //    string query = "SELECT SUM(i.Credito + i.Debito) FROM tRecibo r INNER JOIN tItemCCU i ON r.idItemCCU = i.id WHERE i.id='" + _idItemCCU + "'";
        //    SqlCommand com = new SqlCommand();

        //    com.CommandText = query.ToString();
        //    return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        //}

        //public cCondonacion GetReciboByIdItemCCU(string _idItemCCU)
        //{
        //    string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
        //    SqlCommand com = new SqlCommand(query);
        //    string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        //    if (res == null)
        //        return null;
        //    else
        //        return Load(res);
        //}

        public cCondonacion GetReciboByIdItemCCU(string _idItemCCU)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }

        public cCondonacion GetCondonacionByNro(string _nro)
        {
            try
            {
                string query = "SELECT id FROM " + GetTable + " WHERE nro = " + _nro + " AND papelera='" + (Int16)papelera.Activo + "'";
                SqlCommand cmd = new SqlCommand(query);
                string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
                return Load(res);
            }
            catch
            {
                return null;
            }
        }

        public List<cCondonacion> GetAllCondonaciones()
        {
            List<cCondonacion> cc = new List<cCondonacion>();
            string query = "SELECT c.id FROM " + GetTable + " c ORDER BY c.nro ASC";

            SqlCommand com = new SqlCommand(query);
            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cCondonacion> GetCondonaciones(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            List<cCondonacion> cc = new List<cCondonacion>();
            string query = "SELECT DISTINCT c.nro FROM " + GetTable + " c INNER JOIN tItemCCU i ON c.idItemCCU = i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario = ccu.id";
            query += " INNER JOIN tEmpresa e ON ccu.idEmpresa = e.id";
            query += " WHERE c.nro <> '0'";

            if (_estado != "-1")
                query += " AND c.papelera = '" + _estado + "' ";

            if (_desde != null && _hasta != null)
                query += " AND c.fecha BETWEEN @fechaDesde AND @fechaHasta";

            if (_idEmpresa != "0")
                query += " AND e.id='" + _idEmpresa + "'";

            if (!string.IsNullOrEmpty(_nro))
                query += " AND c.nro like '%" + _nro + "%'";

            query += " ORDER BY c.nro ASC";

            SqlCommand com = new SqlCommand(query);

            if (_desde != null && _hasta != null)
            {
                com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
                com.Parameters["@fechaDesde"].Value = _desde;

                com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
                com.Parameters["@fechaHasta"].Value = _hasta;
            }

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(LoadByNro(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cCondonacion> GetCondonacionToday(string _idCCU)
        {
            List<cCondonacion> cc = new List<cCondonacion>();
            string query = "SELECT c.id FROM tItemCCU i INNER JOIN tCondonacion c ON i.id=c.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND c.papelera = 1 ";
            query += " AND c.fecha > @fecha ORDER BY c.id DESC";

            SqlCommand com = new SqlCommand(query);

            com.Parameters.Add("@fecha", SqlDbType.DateTime);
            com.Parameters["@fecha"].Value = DateTime.Today.Date;

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }
    }
}
