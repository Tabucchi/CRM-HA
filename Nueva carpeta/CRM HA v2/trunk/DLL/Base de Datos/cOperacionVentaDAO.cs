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
    public class cOperacionVentaDAO
    {
        public string GetTable
        { get { return "tOperacionVenta"; } }

        public List<cAtributo> AttributesClass(cOperacionVenta ov)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idEmpresaUnidad", ov.IdEmpresaUnidad));
            lista.Add(new cAtributo("monedaAcordada", ov.MonedaAcordada));
            lista.Add(new cAtributo("precioAcordado", ov.PrecioAcordado));
            lista.Add(new cAtributo("anticipo", ov.Anticipo));
            lista.Add(new cAtributo("idIndiceCAC", ov.IdIndiceCAC));
            lista.Add(new cAtributo("totalComision", ov.TotalComision));
            lista.Add(new cAtributo("iva", ov.Iva));
            lista.Add(new cAtributo("valorDolar", ov.ValorDolar));
            lista.Add(new cAtributo("estado", ov.IdEstado));
            lista.Add(new cAtributo("fecha", ov.Fecha));
            lista.Add(new cAtributo("cac", ov.Cac));
            lista.Add(new cAtributo("uva", ov.Uva));
            lista.Add(new cAtributo("valorBaseUVA", ov.ValorBaseUVA));

            if(!string.IsNullOrEmpty(ov.FechaPosesion.ToString()))
                lista.Add(new cAtributo("fechaPosesion", ov.FechaPosesion));
            else
                lista.Add(new cAtributo("fechaPosesion", DBNull.Value));
            
            if (!string.IsNullOrEmpty(ov.FechaEscritura.ToString()))
                lista.Add(new cAtributo("fechaEscritura", ov.FechaEscritura));
            else
                lista.Add(new cAtributo("fechaEscritura", DBNull.Value));

            return lista;
        }

        public int Save(cOperacionVenta ov)
        {
            if (string.IsNullOrEmpty(ov.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(ov));
            else
                return cDataBase.GetInstance().UpdateObject(ov.Id, GetTable, AttributesClass(ov));
        }

        public cOperacionVenta Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cOperacionVenta ov = new cOperacionVenta();
            ov.Id = Convert.ToString(atributos["id"]);
            ov.IdEmpresaUnidad = Convert.ToString(atributos["idEmpresaUnidad"]);
            ov.MonedaAcordada = Convert.ToString(atributos["monedaAcordada"]);
            ov.PrecioAcordado = Convert.ToDecimal(atributos["precioAcordado"]);
            ov.Anticipo = Convert.ToDecimal(atributos["anticipo"]);
            ov.IdIndiceCAC = Convert.ToString(atributos["idIndiceCAC"]);
            ov.TotalComision = Convert.ToDecimal(atributos["totalComision"]);
            ov.Iva = Convert.ToBoolean(atributos["iva"]);
            ov.ValorDolar = Convert.ToDecimal(atributos["valorDolar"]);
            ov.IdEstado = Convert.ToInt16(atributos["estado"]);
            ov.Fecha = Convert.ToDateTime(atributos["fecha"]);
            ov.Cac = Convert.ToBoolean(atributos["cac"]);
            ov.Uva = Convert.ToBoolean(atributos["uva"]);
            ov.ValorBaseUVA = Convert.ToDecimal(atributos["valorBaseUVA"]);

            if (!string.IsNullOrEmpty(atributos["fechaPosesion"].ToString()))
                ov.FechaPosesion = Convert.ToDateTime(atributos["fechaPosesion"]);
            else
                ov.FechaPosesion = null;
            
            if (!string.IsNullOrEmpty(atributos["fechaEscritura"].ToString()))
                ov.FechaEscritura = Convert.ToDateTime(atributos["fechaEscritura"]);
            else
                ov.FechaEscritura = null;

            return ov;
        }

        public List<cOperacionVenta> GetOperacionesVenta()
        {
            List<cOperacionVenta> cc = new List<cOperacionVenta>();
            string query = "SELECT ov.id FROM " + GetTable + " ov INNER JOIN tEmpresaUnidad eu ON ov.idEmpresaUnidad=eu.id INNER JOIN tEmpresa e ON e.id = eu.idEmpresa WHERE estado=" + (Int16)estadoOperacionVenta.Activo;
            query += " ORDER BY e.Apellido, e.Nombre ASC";

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

        public List<cOperacionVenta> Search(string _idEmpresa, string _idProyecto, Int16 _idEstado, Int16 _monedaIndice, string _desde, string _hasta)
        {
            List<cOperacionVenta> ovs = new List<cOperacionVenta>();
            string blanco = " ";
            string query = "SELECT ov.* FROM tOperacionVenta ov INNER JOIN tEmpresaUnidad eu ON ov.idEmpresaUnidad=eu.id INNER JOIN tEmpresa e ON e.id = eu.idEmpresa WHERE ov.id<>'-1'";

            if (_idEstado != (Int16)estadoOperacionVenta.Todas)
                query += blanco + "AND ov.estado=" + _idEstado;

            if (_idEmpresa != "0")
                query += blanco + "AND eu.idEmpresa=" + _idEmpresa;

            if (_idProyecto != "0")
            {
                query += blanco + "AND eu.idProyecto= '" + _idProyecto + "'";
            }

            switch(_monedaIndice){
                case (Int16)eMonedaIndice.Dolar:
                    query += blanco + "AND ov.monedaAcordada=" + (Int16)tipoMoneda.Dolar;
                    break;
                case (Int16)eMonedaIndice.Pesos:
                    query += blanco + "AND ov.monedaAcordada=" + (Int16)tipoMoneda.Pesos;
                    break;
                case (Int16)eMonedaIndice.CAC:
                    query += blanco + "AND ov.cac=" + Convert.ToInt16(true);
                    break;
                case (Int16)eMonedaIndice.UVA:
                    query += blanco + "AND ov.uva=" + Convert.ToInt16(true);
                    break;
            }

            if (_desde != null && _hasta != null)
                query += " AND ov.fecha BETWEEN @fechaDesde AND @fechaHasta";
            
            query += blanco + "ORDER BY e.Apellido, e.Nombre ASC";

            SqlCommand com = new SqlCommand(query);

            if (_desde != null && _hasta != null)
            {
                com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
                com.Parameters["@fechaDesde"].Value = _desde;

                com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
                com.Parameters["@fechaHasta"].Value = _hasta;
            }

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                ovs.Add(Load(Convert.ToString(idList[i])));
            }
            return ovs;
        }

        public List<cOperacionVenta> GetOV_AConfirmar()
        {
            List<cOperacionVenta> ovs = new List<cOperacionVenta>();
            string query = "SELECT id FROM " + GetTable + " WHERE estado = '" + (Int16)estadoActualizarPrecio.A_confirmar + "' ORDER BY id ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
                ovs.Add(Load(Convert.ToString(idList[i])));

            return ovs;
        }

        public List<cOperacionVenta> GetOVByIdEmpresa(string _idEmpresa)
        {
            List<cOperacionVenta> ovs = new List<cOperacionVenta>();
            string query = "SELECT DISTINCT o.id FROM tOperacionVenta o INNER JOIN tEmpresaUnidad e ON o.id=e.idOv WHERE e.idEmpresa='" + _idEmpresa + "' AND o.estado='" + (Int16)estadoOperacionVenta.Activo + "' AND e.papelera='" + (Int16)papelera.Activo + "'";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null) return null;
            for (int i = 0; idList.Count > i; i++)
                ovs.Add(Load(Convert.ToString(idList[i])));

            return ovs;
        }

        public string GetEmpresaByIdOv(string _idOv)
        {
            string query = "SELECT TOP(1) e.id FROM [dbo].tEmpresaUnidad eu INNER JOIN tEmpresa e ON eu.idEmpresa = e.id WHERE eu.idOv = '" + _idOv + "'";
            SqlCommand com = new SqlCommand(query);

            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public Dictionary<string, int> GetAsignacionesPendientesPorUsuario(DateTime dateDesde, DateTime dateHasta)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();

            string query = "SELECT Count(op.id) AS cant, op.id as IdOp FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta ";
            query += " INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND ";
            query += " c.fechaVencimiento1 BETWEEN @fechaDesde AND @fechaHasta AND cc.estado='1' AND c.estado='1' GROUP BY op.id ";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = dateDesde;
            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = dateHasta;

            com.CommandText = query.ToString();

            DataSet dataSet = cDataBase.GetInstance().GetDataSet(com, GetTable);

            if (dataSet == null) return null;
            DataTable dt = dataSet.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                d.Add(Convert.ToString(row["IdOp"]), Convert.ToInt32(row["cant"]));
            }
            return d;
        }

        public cOperacionVenta GetOperacionByFormaPago(string _idFormaPago)
        {
            string query = "SELECT op.id FROM tOperacionVenta op INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta WHERE fp.id='" + _idFormaPago + "'";
            SqlCommand com = new SqlCommand(query);
            string id = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));

            if (id != null)
                return cOperacionVenta.Load(id);
            else
                return null;
        }

        public cOperacionVenta GetIdOperacionVentaByUnidad(string _idUnidad)
        {
            string query = "SELECT o.id FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id = eu.idUnidad INNER JOIN tOperacionVenta o ON o.idEmpresaUnidad=eu.id ";
            query += " WHERE u.id='" + _idUnidad + "'";
            SqlCommand cmd = new SqlCommand(query);
            string id = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));

            if (id != null)
                return cOperacionVenta.Load(id);
            else
                return null;
        }
    }
}
