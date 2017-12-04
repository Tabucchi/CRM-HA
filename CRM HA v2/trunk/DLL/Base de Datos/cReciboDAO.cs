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
    public class cReciboDAO
    {
        public string GetTable
        { get { return "tRecibo"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cReciboCuota recibo)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCuota", recibo.IdCuota));
            lista.Add(new cAtributo("idItemCCU", recibo.IdItemCCU));
            lista.Add(new cAtributo("nro", recibo.Nro));
            lista.Add(new cAtributo("fecha", recibo.Fecha));
            lista.Add(new cAtributo("monto", recibo.Monto));
            lista.Add(new cAtributo("papelera", recibo._Papelera));
            return lista;
        }

        public int Save(cReciboCuota recibo)
        {
            if (string.IsNullOrEmpty(recibo.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(recibo));
            else
                return cDataBase.GetInstance().UpdateObject(recibo.Id, GetTable, AttributesClass(recibo));
        }

        public cReciboCuota Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cReciboCuota recibo = new cReciboCuota();
            recibo.Id = Convert.ToString(atributos["id"]);
            recibo.IdCuota = Convert.ToString(atributos["idCuota"]);
            recibo.IdItemCCU = Convert.ToString(atributos["idItemCCU"]);
            recibo.Nro = Convert.ToInt32(atributos["nro"]);
            recibo.Fecha = Convert.ToDateTime(atributos["fecha"]);
            recibo.Monto = Convert.ToDecimal(atributos["monto"]);
            recibo._Papelera = Convert.ToInt16(atributos["papelera"]);
            return recibo;
        }

        public cReciboCuota LoadByNro(string _nro)
        {
            cReciboCuota unidad = new cReciboCuota();
            string query = "SELECT id FROM tRecibo WHERE nro ='" + _nro + "'";
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

        public string GetNroReciboByIdItemCCU(string _idItemCCU)
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
        
        public decimal GetMonto(string _idItemCCU)
        {
            string query = "SELECT SUM(i.Credito + i.Debito) FROM tRecibo r INNER JOIN tItemCCU i ON r.idItemCCU = i.id WHERE i.id='" + _idItemCCU + "'";
            SqlCommand com = new SqlCommand();
            
            com.CommandText = query.ToString();
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public cReciboCuota GetLastReciboByCCU(string _idCCU)
        {
            string query = "SELECT TOP(1)r.id FROM tItemCCU i INNER JOIN tRecibo r ON i.id = r.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' ORDER BY r.id DESC;";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }

        public cReciboCuota GetReciboByIdItemCCU(string _idItemCCU)
        {
            //string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' AND papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            string query = "SELECT id FROM " + GetTable + " WHERE idItemCCU='" + _idItemCCU + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (res == null)
                return null;
            else
                return Load(res);
        }
        
        public cReciboCuota GetReciboByNro(string _nro)
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

        public List<cReciboCuota> GetAllRecibos()
        {
            List<cReciboCuota> cc = new List<cReciboCuota>();
            string query = "SELECT r.id FROM " + GetTable + " r ORDER BY r.nro ASC";

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

        public List<cReciboCuota> GetRecibos(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            List<cReciboCuota> cc = new List<cReciboCuota>();
            string query = "SELECT DISTINCT r.nro FROM " + GetTable + " r INNER JOIN tItemCCU i ON r.idItemCCU = i.id INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario = ccu.id";
            query += " INNER JOIN tEmpresa e ON ccu.idEmpresa = e.id";
            query += " WHERE r.nro <> '0'";

            if (_estado != "-1")
                query += " AND r.papelera = '" + _estado + "' ";

            if (_desde != null && _hasta != null)
                query += " AND r.fecha BETWEEN @fechaDesde AND @fechaHasta";

            if (_idEmpresa != "0")
                query += " AND e.id='" + _idEmpresa + "'";

            if (!string.IsNullOrEmpty(_nro))
                query += " AND r.nro like '%" + _nro + "%'";

            query += " ORDER BY r.nro ASC";

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

        public List<cReciboCuota> GetRecibosByNroCuota(string _idCC, string _nro, string _idFormaPago)
        {
            List<cReciboCuota> cc = new List<cReciboCuota>();
            string query = "SELECT r.id FROM tRecibo r INNER JOIN tCuota c ON r.idCuota=c.id ";
            query += " WHERE c.nro='" + _nro + "' AND c.idCuentaCorriente='" + _idCC + "' AND c.idFormaPagoOV='" + _idFormaPago + "' AND r.papelera = '1'";

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

        public List<cReciboCuota> GetRecibosByNroFromItemCCU(string _nro)
        {
            List<cReciboCuota> cc = new List<cReciboCuota>();
            string query = "SELECT r.id FROM tItemCCU i INNER JOIN tRecibo r ON i.id=r.idItemCCU WHERE r.nro='" + _nro + "'";

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

        public List<cReciboCuota> GetRecibosToday(string _idCCU)
        {
            List<cReciboCuota> cc = new List<cReciboCuota>();
            string query = "SELECT r.id FROM tItemCCU i INNER JOIN tRecibo r ON i.id=r.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND r.papelera = 1 ";
            query += " AND r.fecha > @fecha ORDER BY r.id ASC";

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

