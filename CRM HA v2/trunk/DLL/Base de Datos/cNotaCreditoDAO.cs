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
    public class cNotaCreditoDAO
    {
        public string GetTable
        { get { return "tNotaCredito"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cNotaCredito notaCredito)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCuota", notaCredito.IdCuota));
            lista.Add(new cAtributo("idItemCCU", notaCredito.IdItemCCU));
            lista.Add(new cAtributo("nro", notaCredito.Nro));
            lista.Add(new cAtributo("fecha", notaCredito.Fecha));
            lista.Add(new cAtributo("monto", notaCredito.Monto));
            lista.Add(new cAtributo("papelera", notaCredito._Papelera));
            return lista;
        }

        public int Save(cNotaCredito notaCredito)
        {
            if (string.IsNullOrEmpty(notaCredito.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(notaCredito));
            else
                return cDataBase.GetInstance().UpdateObject(notaCredito.Id, GetTable, AttributesClass(notaCredito));
        }

        public cNotaCredito Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cNotaCredito notaCredito = new cNotaCredito();
            notaCredito.Id = Convert.ToString(atributos["id"]);
            notaCredito.IdCuota = Convert.ToString(atributos["idCuota"]);
            notaCredito.IdItemCCU = Convert.ToString(atributos["idItemCCU"]);
            notaCredito.Nro = Convert.ToInt32(atributos["nro"]);
            notaCredito.Fecha = Convert.ToDateTime(atributos["fecha"]);
            notaCredito.Monto = Convert.ToDecimal(atributos["monto"]);
            notaCredito._Papelera = Convert.ToInt16(atributos["papelera"]);
            return notaCredito;
        }

        public cNotaCredito LoadByNro(string _nro)
        {
            cNotaCredito unidad = new cNotaCredito();
            string query = "SELECT id FROM " + GetTable + " WHERE nro ='" + _nro + "'";
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
                result = "0";

            return Convert.ToInt64(result);
        }

        public string GetReciboByIdCuota(string _idCuota)
        {
            string query = "SELECT nro FROM " + GetTable + " WHERE idCuota='" + _idCuota + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public string GetNotaCreditoByIdItemCCU(string _idItemCCU)
        {
            //string query = "SELECT nro FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            string query = "SELECT nro FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public string GetTipoComprobanteByIdItemCCU(string _idItemCCU)
        {
            string query = "SELECT tipoComprobante FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public cNotaCredito GetNCByIdItemCCU(string _idItemCCU)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }

        public cNotaCredito GetNotaCreditoByNro(string _nro)
        {
            try
            {
                string query = "SELECT id FROM " + GetTable + " WHERE nro = " + _nro;
                SqlCommand cmd = new SqlCommand(query);
                string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
                return Load(res);
            }
            catch
            {
                return null;
            }
        }

        public cNotaCredito GetObjectNotaCreditoByIdItemCCU(string _idItemCCU)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }

        public List<cNotaCredito> GetAllNotasCredito()
        {
            List<cNotaCredito> cc = new List<cNotaCredito>();
            string query = "SELECT nc.id FROM " + GetTable + " nc ORDER BY nc.nro ASC";

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

        public List<cNotaCredito> GetNotasCredito(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            List<cNotaCredito> cc = new List<cNotaCredito>();
            string query = "SELECT DISTINCT nc.nro FROM " + GetTable + " nc INNER JOIN tItemCCU i ON nc.idItemCCU = i.id";
            query += " INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario = ccu.id";
            query += " INNER JOIN tEmpresa e ON ccu.idEmpresa = e.id";
            query += " WHERE nc.id<>'0' ";

            if (_estado != "-1")
                query += " AND nc.papelera = '" + _estado + "' ";

            if (_desde != null && _hasta != null)
                query += " AND nc.fecha BETWEEN @fechaDesde AND @fechaHasta";

            if (_idEmpresa != "0")
                query += " AND e.id='" + _idEmpresa + "'";

            if (!string.IsNullOrEmpty(_nro))
                query += " AND nc.nro like '%" + _nro + "%'";

            query += " ORDER BY nc.nro ASC";

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

        public List<cNotaCredito> GetNotasCreditoToday(string _idCCU)
        {
            List<cNotaCredito> cc = new List<cNotaCredito>();
            string query = "SELECT nc.id FROM tItemCCU i INNER JOIN tNotaCredito nc ON i.id=nc.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND nc.papelera = 1 ";
            query += " AND nc.fecha > @fecha ORDER BY nc.id DESC";

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

