﻿using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cCuotaDAO
    {
        public string GetTable
        { get { return "tCuota"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cCuota cuota)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idCuentaCorriente", cuota.IdCuentaCorriente));
            lista.Add(new cAtributo("monto", cuota.Monto));
            lista.Add(new cAtributo("vencimiento1", cuota.Vencimiento1));
            lista.Add(new cAtributo("vencimiento2", cuota.Vencimiento2));
            lista.Add(new cAtributo("estado", cuota.Estado));
            lista.Add(new cAtributo("variacionCAC", cuota.VariacionCAC));
            lista.Add(new cAtributo("comision", cuota.Comision));
            lista.Add(new cAtributo("nro", cuota.Nro));
            lista.Add(new cAtributo("montoAjustado", cuota.MontoAjustado));
            lista.Add(new cAtributo("fecha", cuota.Fecha));
            lista.Add(new cAtributo("fechaVencimiento1", cuota.FechaVencimiento1));
            lista.Add(new cAtributo("fechaVencimiento2", cuota.FechaVencimiento2));
            lista.Add(new cAtributo("saldo", cuota.Saldo));
            lista.Add(new cAtributo("montoPago", cuota.MontoPago));
            lista.Add(new cAtributo("totalComision", cuota.TotalComision));
            lista.Add(new cAtributo("idRegistroPago", cuota.IdRegistroPago));
            lista.Add(new cAtributo("idFormaPagoOV", cuota.IdFormaPagoOV));
            lista.Add(new cAtributo("ajusteCAC", cuota.AjusteCAC));
            return lista;
        }

        public int Save(cCuota cuota)
        {
            if (string.IsNullOrEmpty(cuota.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(cuota));
            else
                return cDataBase.GetInstance().UpdateObject(cuota.Id, GetTable, AttributesClass(cuota));
        }

        public cCuota Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cCuota cuota = new cCuota();
            cuota.Id = Convert.ToString(atributos["id"]);
            cuota.IdCuentaCorriente = Convert.ToString(atributos["idCuentaCorriente"]);
            cuota.Monto = Convert.ToDecimal(atributos["monto"]);
            cuota.Vencimiento1 = Convert.ToDecimal(atributos["vencimiento1"]);
            cuota.Vencimiento2 = Convert.ToDecimal(atributos["vencimiento2"]);
            cuota.Estado = Convert.ToInt16(atributos["estado"]);
            cuota.VariacionCAC = Convert.ToDecimal(atributos["variacionCAC"]);
            cuota.Comision = Convert.ToDecimal(atributos["comision"]);
            cuota.Nro = Convert.ToInt16(atributos["nro"]);
            cuota.MontoAjustado = Convert.ToDecimal(atributos["montoAjustado"]);
            cuota.Fecha = Convert.ToDateTime(atributos["fecha"]);
            cuota.FechaVencimiento1 = Convert.ToDateTime(atributos["fechaVencimiento1"]);
            cuota.FechaVencimiento2 = Convert.ToDateTime(atributos["fechaVencimiento2"]);
            cuota.Saldo = Convert.ToDecimal(atributos["saldo"]);
            cuota.MontoPago = Convert.ToDecimal(atributos["montoPago"]);
            cuota.TotalComision = Convert.ToDecimal(atributos["totalComision"]);
            cuota.IdRegistroPago = Convert.ToString(atributos["idRegistroPago"]);
            cuota.IdFormaPagoOV = Convert.ToString(atributos["idFormaPagoOV"]);
            cuota.AjusteCAC = Convert.ToBoolean(atributos["ajusteCAC"]);
            return cuota;
        }

        public ArrayList LoadTable(string _idCC)
        {
            ArrayList clientes = new ArrayList();
            string query = "SELECT id FROM " + GetTable + " WHERE idCuentaCorriente = '" + _idCC + "' ORDER BY " + GetOrderBy;
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                clientes.Add(Load(Convert.ToString(idList[i])));
            }
            return clientes;
        }

        public List<cCuota> GetCuotas(string _idCuentaCorriente)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCuentaCorriente + " Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasPendientes(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND (estado=" + (Int16)estadoCuenta_Cuota.Activa + " OR estado=" + (Int16)estadoCuenta_Cuota.Pendiente + " ) Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasPendientes2(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND estado=" + (Int16)estadoCuenta_Cuota.Pendiente + " Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasActivas(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND estado=" + (Int16)estadoCuenta_Cuota.Activa + " Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasAnticipos(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND estado=" + (Int16)estadoCuenta_Cuota.Anticipo + " Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasActivasByIdCC(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND estado=" + (Int16)estadoCuenta_Cuota.Activa + " Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasActivasDESC(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND estado=" + (Int16)estadoCuenta_Cuota.Activa + " Order by nro DESC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasByIdFormaPagoOV(string _idCuentaCorriente, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCuentaCorriente + " AND idFormaPagoOV = '" + _idFormaPagoOV + "' Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasActivasByIdFormaPagoOV(string _idCuentaCorriente, string _idFormaPagoOV, string cantCuotasAdelantadas)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT TOP(" + cantCuotasAdelantadas + ") id FROM tCuota WHERE idCuentaCorriente = " + _idCuentaCorriente + " AND idFormaPagoOV = '" + _idFormaPagoOV + "' AND estado =" + (Int16)estadoCuenta_Cuota.Activa + "  Order by id DESC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasPagoByIdFormaPagoOV(string _idCuentaCorriente, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT TOP(1) id FROM tCuota WHERE idCuentaCorriente = " + _idCuentaCorriente + " AND idFormaPagoOV = '" + _idFormaPagoOV + "' AND (estado='" + (Int16)estadoCuenta_Cuota.Activa + "' OR estado='" + (Int16)estadoCuenta_Cuota.Pendiente + "') Order by id ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetAllCuotasActiva()
        {
            //Listado de todas las cuotas pendientes
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE estado = " + Convert.ToInt16(estadoCuenta_Cuota.Activa) + " GROUP BY idCuentaCorriente";
            SqlCommand com = new SqlCommand(query);

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetAllCuotasActivaByActiva(string idEmpresa)
        {
            //Listado de todas las cuotas pendientes
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();

            string query = "SELECT cu.id FROM tCuentaCorriente cc INNER JOIN tCuota cu ON cc.id=cu.idCuentaCorriente WHERE cc.idEmpresa =" + idEmpresa + " AND  cu.estado = " + Convert.ToInt16(estadoCuenta_Cuota.Activa) + " AND cc.estado <> " + Convert.ToInt16(estadoCuenta_Cuota.Anulado);
            SqlCommand com = new SqlCommand(query);

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasActivaMes(DateTime fecha)
        {
            //Listado de las cuotas pendientes de un mes
            DateTime min = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime max = new DateTime(fecha.Month == 12 ? fecha.Year + 1 : fecha.Year, fecha.Month == 12 ? 1 : fecha.Month + 1, 1).AddDays(-1);

            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT c.id FROM tCuota c INNER JOIN tCuentaCorriente cc ON c.idCuentaCorriente = cc.id INNER JOIN tEmpresa e ON cc.idEmpresa = e.id ";
            query += " WHERE (c.estado = " + Convert.ToInt16(estadoCuenta_Cuota.Activa) + " OR c.estado = " + Convert.ToInt16(estadoCuenta_Cuota.Pendiente) + ")";
            query += " AND c.fecha BETWEEN @fechaDesde AND @fechaHasta AND cc.estado='" + Convert.ToInt16(estadoCuenta_Cuota.Activa) + "' ";
            query += " ORDER BY e.Apellido, e.Nombre ASC";

            SqlCommand com = new SqlCommand(query);
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public cCuota GetCuotasByFecha(string _idCC, DateTime fecha)
        {
            //Lista las cuotas desde 15 del mes hasta el 14 del siguiente mes para actualizar el índice CAC de las cuotas del actual mes.
            DateTime min = new DateTime(fecha.Month == 1 ? fecha.Year - 1 : fecha.Year, fecha.Month == 1 ? 12 : fecha.Month, 1);

            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = " SELECT top(1)c.id FROM tCuota c INNER JOIN tCuentaCorriente cc ON c.idCuentaCorriente=cc.id ";
            query += " WHERE cc.id='" + _idCC + "' AND (c.estado = " + (Int16)estadoCuenta_Cuota.Activa + " OR c.estado=" + (Int16)estadoCuenta_Cuota.Pendiente + ") AND cc.estado<>2 AND fecha > @fechaDesde Order by c.id ASC";

            SqlCommand com = new SqlCommand(query);
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.CommandText = query.ToString();
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public List<cCuota> GetCuotasActivaByFecha(DateTime fecha)
        {
            //Lista las cuotas desde 15 del mes hasta el 14 del siguiente mes para actualizar el índice CAC de las cuotas del actual mes.
            DateTime min = new DateTime(fecha.Month == 1 ? fecha.Year - 1 : fecha.Year, fecha.Month == 1 ? 12 : fecha.Month, 15);
            DateTime max = new DateTime(fecha.Month == 12 ? fecha.Year + 1 : fecha.Year, fecha.Month == 12 ? 1 : fecha.Month + 1, 14);

            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = " SELECT c.id FROM tCuota c INNER JOIN tCuentaCorriente cc ON c.idCuentaCorriente=cc.id ";
            query += " WHERE (c.estado = " + (Int16)estadoCuenta_Cuota.Activa + " OR c.estado=" + (Int16)estadoCuenta_Cuota.Pendiente + ") AND cc.estado<>2 AND fecha BETWEEN @fechaDesde AND @fechaHasta Order by cc.id ASC";

            SqlCommand com = new SqlCommand(query);
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public string GetTotalACobrar()
        {
            string query = "SELECT SUM(monto) FROM " + GetTable + " WHERE estado=1";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public decimal GetTotalMontoCuotas(string _idCC, int _nroDesde, int _nroA)
        {
            string query = "SELECT SUM(monto) FROM tCuota WHERE nro BETWEEN '" + _nroDesde + "' AND '" + _nroA + "' AND idCuentaCorriente = '" + _idCC + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public cCuota GetCuotaByNro(string _idCC, int _nro, string _idFp)
        {
            //string query = "SELECT id FROM [dbo].[tCuota] WHERE idCuentaCorriente=" + _idCC + " AND nro=" + _nro + " AND estado='" + (Int16)estadoCuenta_Cuota.Activa + "' order by id DESC;";
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente=" + _idCC + " AND nro=" + _nro;

            if (_idFp != null)
                query += " AND idFormaPagoOV = '" + _idFp + "'";

            query += " order by id DESC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public cCuota GetCuotaByNro(string _idCC, string _idFormaPago, int _nro)
        {
            string query = "SELECT id FROM [dbo].[tCuota] WHERE idCuentaCorriente=" + _idCC + " AND idFormaPagoOV=" + _idFormaPago + " AND nro=" + _nro + " AND estado='" + (Int16)estadoCuenta_Cuota.Activa + "' order by id DESC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public List<cCuota> GetCuotasByNro(string _idCC, int _nroDesde, int _nroA)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT * FROM tCuota WHERE nro BETWEEN '" + _nroDesde + "' AND '" + _nroA + "' AND idCuentaCorriente = '" + _idCC + "'";
            SqlCommand com = new SqlCommand(query);

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public cCuota GetLast(string idCC)
        {
            string query = "SELECT Top(1) id FROM [dbo].[tCuota] WHERE idCuentaCorriente=" + idCC + " AND estado=" + (Int16)estadoCuenta_Cuota.Activa + " order by id DESC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public cCuota GetLastByEstado(string idCC, Int16 _idEstado)
        {
            string query = "SELECT Top(1) id FROM tCuota WHERE idCuentaCorriente=" + idCC + " AND estado=" + _idEstado + " order by id ASC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public cCuota GetLastPay(string idCC)
        {
            string query = "SELECT Top(1) id FROM tCuota WHERE idCuentaCorriente=" + idCC + " AND estado=" + (Int16)estadoCuenta_Cuota.Pagado + " order by id DESC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public cCuota GetLastPayByIdCCU(string _idCC)
        {
            string query = "SELECT TOP(1)c.id FROM tCuota c INNER JOIN tItemCCU i ON c.id = i.idCuota WHERE i.tipoOperacion = " + (Int16)eTipoOperacion.PagoCuota;
            query += " AND i.idEstado = " + (Int16)eEstadoItem.Pagado + " AND i.idCuentaCorrienteUsuario='" + _idCC + "' ORDER BY i.id ASC";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public Int16 GetCantCuotasPagas(string _idCC)
        {
            string query = "SELECT Count(id) FROM " + GetTable + " WHERE idCuentaCorriente=" + _idCC + " AND estado=" + (Int16)estadoCuenta_Cuota.Pagado;
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToInt16(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public List<cCuota> GetCuotasPagas(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND estado=" + (Int16)estadoCuenta_Cuota.Pagado + " Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public Int16 GetCantCuotasPagasAdelantos(string _idCC, string _idFormaPagoOV)
        {
            string query = "SELECT Count(id) FROM " + GetTable + " WHERE idCuentaCorriente=" + _idCC + " AND idFormaPagoOV=" + _idFormaPagoOV + " AND (estado=" + (Int16)estadoCuenta_Cuota.Pagado + " OR estado=" + (Int16)estadoCuenta_Cuota.Anticipo + ")";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToInt16(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public cCuota GetFirst(string idCC, string _idFormaPago)
        {
            string query = "SELECT Top(1) id FROM tCuota WHERE idCuentaCorriente=" + idCC + " AND idFormaPagoOV = '" + _idFormaPago + "' order by id ASC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public cCuota GetFirstActiva(string idCC, string _idFormaPago)
        {
            string query = "SELECT Top(1) id FROM tCuota WHERE idCuentaCorriente=" + idCC + " AND idFormaPagoOV = '" + _idFormaPago + "' AND estado='" + (Int16)estadoCuenta_Cuota.Activa + "' order by id ASC";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        //BORRAR DESPUES DE USAR    
        public cCuota GetFirst1(string idCC)
        {
            string query = "SELECT Top(1) id FROM tCuota WHERE idCuentaCorriente=" + idCC + " order by id ASC;";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public ArrayList GetCuotaByIdCC(string _idCC)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT nro FROM tCuota WHERE idCuentaCorriente = '" + _idCC + "' AND estado = '" + (Int16)estadoCuenta_Cuota.Activa + "'";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        public ArrayList GetCuotasOrderByFormaPagoOV(string _idCC)
        {
            /*ArrayList cuotas = new ArrayList();
            string query = "SELECT idFormaPagoOV FROM tCuota WHERE idCuentaCorriente='" + _idCC + "' GROUP BY idFormaPagoOV ORDER BY idFormaPagoOV";

            SqlCommand com = new SqlCommand(query);
            cuotas = cDataBase.GetInstance().ExecuteReader(com);

            return cuotas;*/

            //Listo todas cuotas con el nro de cuenta corriente correspondiente
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuotas = new List<cCuota>();
            string query = "SELECT id, idFormaPagoOV FROM tCuota WHERE idCuentaCorriente='" + _idCC + "'";
            SqlCommand com = new SqlCommand(query);
            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cuotas.Add(DAO.Load(Convert.ToString(idList[i])));
            }

            //Listo los nro de la forma de pago de OV
            ArrayList listCuotas = new ArrayList();
            int index = 0;
            foreach (cCuota c in cuotas)
            {
                if (c.IdFormaPagoOV != index.ToString())
                    listCuotas.Add(c.IdFormaPagoOV);
                index = Convert.ToInt16(c.IdFormaPagoOV);
            }

            return listCuotas;
        }

        public List<cCuota> GetCuotasVencidas(string _idCuentaCorriente)
        {
            //Listado de las cuotas pendientes de un mes
            DateTime fecha = DateTime.Now;

            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT * FROM tCuota WHERE idCuentaCorriente = '" + _idCuentaCorriente + "' AND estado = '" + (Int16)estadoCuenta_Cuota.Activa + "' and fechaVencimiento2 < @fechaVencida";
            SqlCommand com = new SqlCommand(query);

            com.Parameters.Add("@fechaVencida", SqlDbType.DateTime);
            com.Parameters["@fechaVencida"].Value = fecha;

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasPendientesByIdCC(string _idCuentaCorriente, string _idFormaPagoOV)
        {
            //Listado de las cuotas pendientes de un mes
            DateTime fecha = DateTime.Now;

            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = '" + _idCuentaCorriente + "' AND idFormaPagoOV = '" + _idFormaPagoOV + "' AND estado = '" + (Int16)estadoCuenta_Cuota.Pendiente + "' GROUP BY id";
            SqlCommand com = new SqlCommand(query);

            //com.Parameters.Add("@fechaVencida", SqlDbType.DateTime);
            //com.Parameters["@fechaVencida"].Value = fecha;

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        //public List<cCuota> GetCuotasPendientes(string _idOV, DateTime fechaHoy, bool _existCuota)
        public List<cCuota> GetCuotasPendientes(string _idEmpresa, DateTime fechaHoy)
        {
            List<cCuota> ovs = new List<cCuota>();
            string query = "SELECT c.id FROM tCuota c INNER JOIN tCuentaCorriente cc ON c.idCuentaCorriente = cc.id";
            query += " WHERE cc.idEmpresa='" + _idEmpresa + "' AND cc.estado ='1' AND (c.estado = '1' OR c.estado = '5') AND (c.fechaVencimiento1 BETWEEN @fechaHoy AND @fechaVenc OR c.fechaVencimiento2 BETWEEN @fechaHoy AND @fechaVenc2 OR c.fechaVencimiento2 <= @fechaVenc2)";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaHoy", SqlDbType.DateTime);
            //com.Parameters["@fechaHoy"].Value = DateTime.Now;
            com.Parameters["@fechaHoy"].Value = fechaHoy;

            com.Parameters.Add("@fechaVenc", SqlDbType.DateTime);
            //com.Parameters["@fechaVenc"].Value = Convert.ToDateTime(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + "12");
            com.Parameters["@fechaVenc"].Value = Convert.ToDateTime(fechaHoy.Year + "/" + fechaHoy.Month + "/" + "12");

            com.Parameters.Add("@fechaVenc2", SqlDbType.DateTime);
            //com.Parameters["@fechaVenc2"].Value = Convert.ToDateTime(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + "12");
            com.Parameters["@fechaVenc2"].Value = Convert.ToDateTime(fechaHoy.Year + "/" + fechaHoy.Month + "/" + "22");

            com.CommandText = query.ToString();
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
                ovs.Add(Load(Convert.ToString(idList[i])));

            return ovs;
        }

        public List<cCuota> GetCuotasActivasByRangoNro(string _idCuentaCorriente, string _nro)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCuentaCorriente + " AND estado = '" + (Int16)estadoCuenta_Cuota.Activa + "' AND nro <= '" + _nro + "' Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetCuotasSinceNro(string _idCuentaCorriente, string _nro)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT id FROM tCuota WHERE idCuentaCorriente = " + _idCuentaCorriente + " AND nro >= '" + _nro + "' Order by nro ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public decimal GetSaldoCuota(string _idCC, string _idFormaPago, Int16 _estado, string _moneda)
        {
            string query = null;
            if (_estado == (Int16)estadoCuenta_Cuota.Pagado)
                query = "SELECT top(1)saldo FROM tCuota WHERE idCuentaCorriente = '" + _idCC + "' AND ";
            else
                query = "SELECT top(1)montoAjustado FROM tCuota WHERE idCuentaCorriente = '" + _idCC + "' AND ";

            query += " idFormaPagoOV='" + _idFormaPago + "' ";

            //if (_moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
            //    query += " AND (variacionCAC != 0 OR nro!=1) AND ";
            //else
                query += " AND ";

            if (_estado == (Int16)estadoCuenta_Cuota.Pagado)
                query += "estado = '" + (Int16)estadoCuenta_Cuota.Pagado + "' ORDER BY id DESC";
            else
                query += "(estado = '" + (Int16)estadoCuenta_Cuota.Pendiente + "' OR estado = '" + (Int16)estadoCuenta_Cuota.Activa + "')";

            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public string GetNroSaldoCuota(string _idCC, string _idFormaPago, Int16 _estado, string _moneda)
        {
            string query = null;
            if (_estado == (Int16)estadoCuenta_Cuota.Pagado)
                query = "SELECT top(1)nro FROM tCuota WHERE idCuentaCorriente = '" + _idCC + "' AND ";
            else
                query = "SELECT top(1)nro FROM tCuota WHERE idCuentaCorriente = '" + _idCC + "' AND ";

            query += " idFormaPagoOV='" + _idFormaPago + "' ";

            if (_moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                query += " AND (variacionCAC != 0 OR nro!=1) AND ";
            else
                query += " AND ";

            if (_estado == (Int16)estadoCuenta_Cuota.Pagado)
                query += "estado = '" + (Int16)estadoCuenta_Cuota.Pagado + "' ORDER BY id DESC";
            else
                query += "(estado = '" + (Int16)estadoCuenta_Cuota.Pendiente + "' OR estado = '" + (Int16)estadoCuenta_Cuota.Activa + "')";

            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }


        public decimal GetCuotasMes(string _idEmpresa, DateTime dateDesde, DateTime dateHasta, string _idFp)
        {
            string query = "SELECT c.Vencimiento1 FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND ";
            query += " c.fechaVencimiento1 BETWEEN @fechaDesde AND @fechaHasta AND e.id='" + _idEmpresa + "' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND fp.Id='" + _idFp + "' GROUP BY c.Vencimiento1";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;
            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();

            decimal saldo = 0;

            if (!string.IsNullOrEmpty(cDataBase.GetInstance().ExecuteScalar(com)) && cDataBase.GetInstance().ExecuteScalar(com) != null)
                saldo = Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
            else
                saldo = 0;

            return saldo;
        }

        public decimal GetCuotasMesMonto(string _idEmpresa, DateTime dateDesde, DateTime dateHasta, string _idFp)
        {
            string query = "SELECT c.Monto FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND ";
            query += " c.fechaVencimiento1 BETWEEN @fechaDesde AND @fechaHasta AND e.id='" + _idEmpresa + "' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND fp.Id='" + _idFp + "' GROUP BY c.Monto";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;
            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();

            decimal saldo = 0;

            if (!string.IsNullOrEmpty(cDataBase.GetInstance().ExecuteScalar(com)) && cDataBase.GetInstance().ExecuteScalar(com) != null)
                saldo = Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
            else
                saldo = 0;

            return saldo;
        }

        public List<cCuota> GetCantCuotasNextYear(string _idEmpresa, string _idFormaPago, DateTime dateDesde, DateTime dateHasta)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT c.id FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND ";
            query += " c.fechaVencimiento1 > @fechaDesde AND c.idFormaPagoOV = '" + _idFormaPago + "' AND e.id='" + _idEmpresa + "' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' GROUP BY c.id";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;

            com.CommandText = query.ToString();

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public ArrayList GetSaldos()
        {
            ArrayList unidades = new ArrayList();

            //DateTime dateDesde = Convert.ToDateTime(DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " + "1");
            //DateTime dateHasta = Convert.ToDateTime(DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " + "29");

            //string query = "SELECT cc.id, e.Apellido, e.nombre,  p.descripcion FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa ";
            string query = "SELECT cc.id, e.apellido FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa ";
            query += " INNER JOIN tOperacionVenta op ON op.id=eu.idOv  INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id ";
            query += " INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "'"; 
            //query += " INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND ";
            //query += " c.fechaVencimiento1 BETWEEN @fechaDesde AND @fechaHasta AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "'";
            //query += " GROUP BY cc.id, e.Apellido, e.nombre, p.descripcion ORDER BY e.Apellido, e.nombre,  p.descripcion";
            query += " GROUP BY cc.id, e.apellido ORDER BY e.apellido";

            SqlCommand com = new SqlCommand();
            //com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            //com.Parameters["@fechaDesde"].Value = dateDesde;
            //com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            //com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        public List<cCuota> GetCuotasMesProyecto(string _idProyecto, DateTime dateDesde, DateTime dateHasta, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuotas = new List<cCuota>();

            string query = "SELECT c.id FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND ";
            query += " c.fechaVencimiento1 BETWEEN @fechaDesde AND @fechaHasta AND p.id='" + _idProyecto + "' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND fp.Moneda='" + _tipoMoneda + "' GROUP BY c.id";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;
            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuotas.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuotas;
        }



        public List<cCuota> GetCuotasByProyecto(string _idProyecto)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();

            DateTime dateDesde = Convert.ToDateTime(DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " + "1");
            DateTime dateHasta = Convert.ToDateTime(DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " + "29");

            string query = "SELECT c.id FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv ";
            query += " INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto ";
            query += " INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND c.fechaVencimiento1 BETWEEN @fechaDesde AND @fechaHasta ";
            query += " AND cc.estado='1' GROUP BY c.id, p.descripcion ORDER BY p.descripcion";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;
            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }


        public List<cCuota> GetCuotasMesProyectoPendientes(string _idProyecto, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuotas = new List<cCuota>();

            string query = "SELECT c.id FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa ";
            query += " INNER JOIN tOperacionVenta op ON op.id=eu.idOv  INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id ";
            query += " INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente ";
            query += " WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND cc.estado='1' AND p.id='" + _idProyecto + "' AND fp.moneda = '" + _tipoMoneda + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Pendiente + "' GROUP BY c.id";
            //query += " WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND cc.estado='1' AND p.id='" + _idProyecto + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Pendiente + "' GROUP BY c.id";

            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuotas.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuotas;
        }

        public List<cCuota> GetCuotasProyectoRestantes(string _idProyecto, DateTime dateDesde, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            List<cCuota> cuota = new List<cCuota>();
            string query = "SELECT c.id FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv ";
            query += " INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto ";
            query += " INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' AND c.fechaVencimiento1 > @fechaDesde ";
            query += " AND p.id='" + _idProyecto + "' AND fp.moneda='" + _tipoMoneda + "' GROUP BY c.id";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;

            com.CommandText = query.ToString();

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;

            for (int i = 0; idList.Count > i; i++)
            {
                cuota.Add(DAO.Load(Convert.ToString(idList[i])));
            }
            return cuota;
        }

        public List<cCuota> GetItemByCuotasPendientes(string _idCCU)
        {
            List<cCuota> cc = new List<cCuota>();
            string query = "SELECT c.id, c.idCuentaCorriente FROM tCuota c INNER JOIN tItemCCU i ON i.idCuota = c.id WHERE (c.estado =" + (Int16)estadoCuenta_Cuota.Activa + " OR c.estado =" + (Int16)estadoCuenta_Cuota.Pendiente + ") AND i.idCuentaCorrienteUsuario ='" + _idCCU + "' group by c.id, c.idCuentaCorriente";

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


        public decimal GetSaldoByProyecto()
        {
            string query = "SELECT i.id, SUM(i.saldo) FROM tItemCCU i INNER JOIN tCuentaCorrienteUsuario ccu ON i.idCuentaCorrienteUsuario = ccu.id INNER JOIN tEmpresa e ON ccu.idEmpresa = e.id INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa ";
            query += " INNER JOIN tProyecto p ON eu.idProyecto = p.id WHERE i.idEstado <>'" + (Int16)eEstadoItem.A_confirmar + "' AND i.idEstado <>'" + (Int16)eEstadoItem.Eliminado + "' GROUP BY i.id";

            SqlCommand com = new SqlCommand();
            //com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            //com.Parameters["@fechaDesde"].Value = dateDesde;
            //com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            //com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();

            decimal saldo = 0;

            if (!string.IsNullOrEmpty(cDataBase.GetInstance().ExecuteScalar(com)) && cDataBase.GetInstance().ExecuteScalar(com) != null)
                saldo = Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
            else
                saldo = 0;

            return saldo;
        }

    }
}

