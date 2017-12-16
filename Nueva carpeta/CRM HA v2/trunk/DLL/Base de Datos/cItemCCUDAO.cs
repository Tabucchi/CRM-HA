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
    public class cItemCCUDAO
    {
        public string GetTable
        { get { return "tItemCCU"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cItemCCU cuentacorriente)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCuentaCorrienteUsuario", cuentacorriente.IdCuentaCorrienteUsuario));
            lista.Add(new cAtributo("fecha", cuentacorriente.Fecha));
            lista.Add(new cAtributo("concepto", cuentacorriente.Concepto));
            lista.Add(new cAtributo("debito", cuentacorriente.Debito));
            lista.Add(new cAtributo("credito", cuentacorriente.Credito));
            lista.Add(new cAtributo("saldo", cuentacorriente.Saldo));
            lista.Add(new cAtributo("idCuota", cuentacorriente.IdCuota));
            lista.Add(new cAtributo("idEstado", cuentacorriente.IdEstado));
            lista.Add(new cAtributo("tipoOperacion", cuentacorriente.TipoOperacion));

            return lista;
        }

        public cItemCCU Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cItemCCU cc = new cItemCCU();
            cc.Id = Convert.ToString(atributos["id"]);
            cc.IdCuentaCorrienteUsuario = Convert.ToString(atributos["idCuentaCorrienteUsuario"]);
            cc.Fecha = Convert.ToDateTime(atributos["fecha"]);
            cc.Concepto = Convert.ToString(atributos["concepto"]);
            cc.Debito = Convert.ToDecimal(atributos["debito"]);
            cc.Credito = Convert.ToDecimal(atributos["credito"]);
            cc.Saldo = Convert.ToDecimal(atributos["saldo"]);
            cc.IdCuota = Convert.ToString(atributos["idCuota"]);
            cc.IdEstado = Convert.ToInt16(atributos["idEstado"]);
            cc.TipoOperacion = Convert.ToInt16(atributos["tipoOperacion"]);
            return cc;
        }

        public int Save(cItemCCU cuentacorriente)
        {
            if (string.IsNullOrEmpty(cuentacorriente.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(cuentacorriente));
            else
                return cDataBase.GetInstance().UpdateObject(cuentacorriente.Id, GetTable, AttributesClass(cuentacorriente));
        }

        public List<cItemCCU> GetCuentaCorriente(string _idCC)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _idCC + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetCuentaCorrienteLast10(string _idCC)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE id in (SELECT TOP(100)id FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _idCC + "' ORDER BY fecha desc) ";
            query += " AND idEstado<>" + (Int16)eEstadoItem.A_confirmar + " AND idEstado<>" + (Int16)eEstadoItem.Eliminado + " AND idEstado<> " + (Int16)eEstadoItem.CuotaSinCAC + " ORDER BY id ASC";

            //string query = "SELECT id FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _idCC + "'";
            //query += " AND idEstado<>" + (Int16)eEstadoItem.A_confirmar + " AND idEstado<>" + (Int16)eEstadoItem.Eliminado + " AND idEstado<> " + (Int16)eEstadoItem.CuotaSinCAC + " ORDER BY fecha ASC";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetCuentaCorrienteLast10Desc(string _idCC)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE id in (SELECT TOP(10)id FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _idCC + "' ORDER BY fecha desc) ORDER BY id DESC";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetItems(Int16 _idEstado)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE IdEstado=" + _idEstado;

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }
        public List<cItemCCU> Search(string _idCC, string _fechaDesde, string _fechaHasta)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _idCC + "'";

            if (_fechaDesde != null && _fechaHasta != null)
                query += " AND fecha BETWEEN @fechaDesde AND @fechaHasta";

            SqlCommand com = new SqlCommand(query);

            if (_fechaDesde != null && _fechaHasta != null)
            {
                com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
                com.Parameters["@fechaDesde"].Value = Convert.ToDateTime(_fechaDesde);

                com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
                com.Parameters["@fechaHasta"].Value = Convert.ToDateTime(_fechaHasta).AddDays(1);
            }

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public cItemCCU GetCuentaCorrienteByIdCuota(string _idCuota)
        {
            string query = "SELECT TOP(1)id FROM " + GetTable + " WHERE idCuota = '" + _idCuota + "' ORDER BY id DESC";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public string GetLastSaldoByIdCCU(string _idCCU)
        {
            string query = "SELECT TOP(1) saldo FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _idCCU + "' AND idEstado <>'" + (Int16)eEstadoItem.A_confirmar + "' AND idEstado <>'" + (Int16)eEstadoItem.Eliminado + "' ORDER BY id DESC";
            SqlCommand cmd = new SqlCommand(query);
            string r = cDataBase.GetInstance().ExecuteScalar(cmd);
            if (!string.IsNullOrEmpty(r))
                return r;
            else
                return "0";
        }

        public cItemCCU GetLastItemById(string _id)
        {
            string query = "SELECT TOP(1)id FROM " + GetTable + " WHERE idCuentaCorrienteUsuario = '" + _id + "' ORDER BY id DESC";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public string GetTopNroCuota(string _idCuentaCorriente)
        {
            string query = "SELECT TOP(1) c.nro FROM tCuota c INNER JOIN tItemCCU i ON c.id = i.idCuota WHERE i.idEstado = '" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.idCuentaCorriente = '" + _idCuentaCorriente + "' ORDER BY c.nro DESC";

            SqlCommand cmd = new SqlCommand(query);
            string r = cDataBase.GetInstance().ExecuteScalar(cmd);
            return r;
        }

        public string GetCCByIdCuota(string _idCuota)
        {
            string query = "SELECT idCuota FROM " + GetTable + " WHERE idCuota = '" + _idCuota + "'";
            SqlCommand cmd = new SqlCommand(query);
            return cDataBase.GetInstance().ExecuteScalar(cmd);
        }

        public cItemCCU GetItemCCUByIdCuota(string _idCuota)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE idCuota = '" + _idCuota + "'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public cItemCCU GetFirtsItemCCUByIdCuota(string _idCuota)
        {
            string query = "SELECT Top(1)id FROM " + GetTable + " WHERE idCuota = '" + _idCuota + "' ORDER BY id ASC";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public cItemCCU GetLastReciboByIdCCU(string _idCCU)
        {
            string query = "SELECT Top(1)i.id FROM tItemCCU i INNER JOIN tRecibo r ON i.id=r.idItemCCU WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND r.papelera = 1 ";
            query += " AND i.fecha > @fecha ORDER BY r.id DESC";

            SqlCommand com = new SqlCommand(query);

            com.Parameters.Add("@fecha", SqlDbType.DateTime);
            com.Parameters["@fecha"].Value = DateTime.Today.Date;

            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            
            if (res == null)
                return null;
            else
                return Load(res);
        }
        
        public cItemCCU GetLastNotaCreditoByIdCCU(string _idCCU)
        {
            string query = "SELECT Top(1)i.id FROM tItemCCU i INNER JOIN tNotaCredito nc ON nc.idItemCCU=i.id WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND nc.papelera = 1 ORDER BY nc.id DESC";
            query += " AND i.fecha > @fecha ORDER BY r.id DESC";

            SqlCommand com = new SqlCommand(query);

            com.Parameters.Add("@fecha", SqlDbType.DateTime);
            com.Parameters["@fecha"].Value = DateTime.Today.Date;

            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));

            if (res == null)
                return null;
            else
                return Load(res);
        }

        public cItemCCU GetLastNotaDebitoByIdCCU(string _idCCU)
        {
            string query = "SELECT Top(1)i.id FROM tItemCCU i INNER JOIN tNotaDebito nd ON nd.idItemCCU=i.id WHERE i.idCuentaCorrienteUsuario='" + _idCCU + "' AND nd.papelera = 1 ORDER BY nd.id DESC";
            query += " AND i.fecha > @fecha ORDER BY r.id DESC";

            SqlCommand com = new SqlCommand(query);

            com.Parameters.Add("@fecha", SqlDbType.DateTime);
            com.Parameters["@fecha"].Value = DateTime.Today.Date;

            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));

            if (res == null)
                return null;
            else
                return Load(res);
        }

        public int GetCantCuotasById(string _idCuota)
        {
            string query = "SELECT Count(id) FROM tItemCCU WHERE idCuota= '" + _idCuota + "' GROUP BY idCuota";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToInt16(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public List<cItemCCU> GetItemsByCuotas(string _idCuota)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuota = '" + _idCuota + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public decimal GetTotalCuotasByEstado(string _idCuota, Int16 _idEstado)
        {
            string query = "SELECT SUM(debito) FROM tItemCCU WHERE idCuota = '" + _idCuota + "' and idEstado='" + _idEstado + "'";
            SqlCommand cmd = new SqlCommand(query);
            string r = cDataBase.GetInstance().ExecuteScalar(cmd);
            if (!string.IsNullOrEmpty(r))
                return Convert.ToDecimal(r);
            else
                return 0;
        }

        public List<cItemCCU> GetItemsByCuotas(string _idCuotaDesde, string _idCuotaHasta)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuota BETWEEN '" + _idCuotaDesde + "' AND '" + _idCuotaHasta + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetItemsByCuotas_EstadoPagado(string _idCuota)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuota = '" + _idCuota + "' AND idEstado='" + (Int16)eEstadoItem.Pagado + "' AND tipoOperacion='" + (Int16)eTipoOperacion.Cuota + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetItemsFromId(string _id, string _idCCU)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM tItemCCU WHERE id > '" + _id + "' AND idCuentaCorrienteUsuario = '" + _idCCU + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetItemByCuotasPendientes(string _idCC)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT i.id FROM tItemCCU i INNER JOIN tCuota c ON i.idCuota = c.id WHERE (c.estado =" + (Int16)estadoCuenta_Cuota.Activa + " OR c.estado =" + (Int16)estadoCuenta_Cuota.Pendiente + ") AND i.idCuentaCorrienteUsuario ='" + _idCC + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetItemByCuotasPendientesByIdCC(string _idCC)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT i.id FROM tItemCCU i INNER JOIN tCuota c ON i.idCuota = c.id WHERE (c.estado =" + (Int16)estadoCuenta_Cuota.Activa + " OR c.estado =" + (Int16)estadoCuenta_Cuota.Pendiente + ") AND c.idCuentaCorriente ='" + _idCC + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public List<cItemCCU> GetItemByCuotasAdelanto(string _idCC, Int16 _idEstado)
        {
            List<cItemCCU> cc = new List<cItemCCU>();
            string query = "SELECT id FROM " + GetTable + " WHERE IdEstado=" + _idEstado + " AND idCuentaCorrienteUsuario = '" + _idCC + "'";

            SqlCommand com = new SqlCommand(query);
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

