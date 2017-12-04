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
    public class cNotaDebitoDAO
    {
        public string GetTable
        { get { return "tNotaDebito"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cNotaDebito notaCredito)
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

        public int Save(cNotaDebito notaDebito)
        {
            if (string.IsNullOrEmpty(notaDebito.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(notaDebito));
            else
                return cDataBase.GetInstance().UpdateObject(notaDebito.Id, GetTable, AttributesClass(notaDebito));
        }

        public cNotaDebito Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cNotaDebito notaDebito = new cNotaDebito();
            notaDebito.Id = Convert.ToString(atributos["id"]);
            notaDebito.IdCuota = Convert.ToString(atributos["idCuota"]);
            notaDebito.IdItemCCU = Convert.ToString(atributos["idItemCCU"]);
            notaDebito.Nro = Convert.ToInt32(atributos["nro"]);
            notaDebito.Fecha = Convert.ToDateTime(atributos["fecha"]);
            notaDebito.Monto = Convert.ToDecimal(atributos["monto"]);
            notaDebito._Papelera = Convert.ToInt16(atributos["papelera"]);
            return notaDebito;
        }

        public cNotaDebito LoadByNro(string _nro)
        {
            cNotaDebito unidad = new cNotaDebito();
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

        public string GetNotaDebitoByIdItemCCU(string _idItemCCU)
        {
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

        public cNotaDebito GetNDByIdItemCCU(string _idItemCCU)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }

        public cNotaDebito GetNotaDebitoByNro(string _nro)
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

        public cNotaDebito GetObjectNotaDebitoByIdItemCCU(string _idItemCCU)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }

        public List<cNotaDebito> GetAllNotasDebito()
        {
            List<cNotaDebito> cc = new List<cNotaDebito>();
            string query = "SELECT nd.id FROM " + GetTable + " nd ORDER BY nd.nro ASC";

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

        public List<cNotaDebito> GetNotasDebito(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            List<cNotaDebito> cc = new List<cNotaDebito>();
            string query = "SELECT DISTINCT nd.nro FROM " + GetTable + " nd INNER JOIN tItemCCU i ON nd.idItemCCU = i.id ";
            query += " INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario = ccu.id ";
            query += " INNER JOIN tEmpresa e ON ccu.idEmpresa = e.id ";
            query += " WHERE nd.nro <> '0' ";

            if (_estado != "-1")
                query += " AND nd.papelera = '" + _estado + "' ";

            if (_desde != null && _hasta != null)
                query += " AND nd.fecha BETWEEN @fechaDesde AND @fechaHasta";

            if (_idEmpresa != "0")
                query += " AND e.id='" + _idEmpresa + "'";

            if (!string.IsNullOrEmpty(_nro))
                query += " AND nd.nro like '%" + _nro + "%'";

            query += " ORDER BY nd.nro ASC";

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

        public List<cNotaDebito> GetNotasDebitoToday(string _idCCU)
        {
            List<cNotaDebito> cc = new List<cNotaDebito>();
            string query = "SELECT nd.id FROM tItemCCU i INNER JOIN tNotaDebito nd ON i.id=nd.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND nd.papelera = 1 ";
            query += " AND nd.fecha > @fecha ORDER BY nd.id DESC";

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

