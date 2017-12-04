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
    public class cCuentaCorrienteDAO
    {
        public string GetTable
        { get { return "tCuentaCorriente"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cCuentaCorriente cuentacorriente)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idEmpresa", cuentacorriente.IdEmpresa));
            lista.Add(new cAtributo("cantCuotas", cuentacorriente.CantCuotas));
            lista.Add(new cAtributo("total", cuentacorriente.Total));
            lista.Add(new cAtributo("saldo", cuentacorriente.Saldo));
            lista.Add(new cAtributo("saldoPesos", cuentacorriente.SaldoPeso));
            lista.Add(new cAtributo("formaPago", cuentacorriente.FormaPago));
            lista.Add(new cAtributo("idIndiceCAC", cuentacorriente.IdIndiceCAC));
            lista.Add(new cAtributo("idUnidad", cuentacorriente.IdUnidad));
            lista.Add(new cAtributo("unidadFuncional", cuentacorriente.UnidadFuncional));
            lista.Add(new cAtributo("estado", cuentacorriente.IdEstado));
            lista.Add(new cAtributo("anticipo", cuentacorriente.Anticipo));
            lista.Add(new cAtributo("iva", cuentacorriente.Iva));
            lista.Add(new cAtributo("idEmpresaUnidad", cuentacorriente.IdEmpresaUnidad));
            lista.Add(new cAtributo("idOperacionVenta", cuentacorriente.IdOperacionVenta));
            lista.Add(new cAtributo("monedaAcordada", cuentacorriente.MonedaAcordada));
            lista.Add(new cAtributo("textoAnulado", cuentacorriente.TextoAnulado));
            return lista;
        }

        public cCuentaCorriente Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cCuentaCorriente cc = new cCuentaCorriente();
            cc.Id = Convert.ToString(atributos["id"]);
            cc.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            cc.CantCuotas = Convert.ToInt16(atributos["cantCuotas"]);
            cc.Total = Convert.ToDecimal(atributos["total"]);
            cc.Saldo = Convert.ToDecimal(atributos["saldo"]);
            cc.SaldoPeso = Convert.ToDecimal(atributos["saldoPesos"]);
            cc.IdIndiceCAC = Convert.ToString(atributos["idIndiceCAC"]);
            cc.FormaPago = Convert.ToString(atributos["formaPago"]);
            cc.IdUnidad = Convert.ToString(atributos["idUnidad"]);
            cc.UnidadFuncional = Convert.ToString(atributos["unidadFuncional"]);
            cc.IdEstado = Convert.ToInt16(atributos["estado"]);
            cc.Anticipo = Convert.ToDecimal(atributos["anticipo"]);
            cc.Iva = Convert.ToBoolean(atributos["iva"]);
            cc.IdEmpresaUnidad = Convert.ToString(atributos["idEmpresaUnidad"]);
            cc.IdOperacionVenta = Convert.ToString(atributos["idOperacionVenta"]);
            cc.MonedaAcordada = Convert.ToString(atributos["monedaAcordada"]);
            cc.TextoAnulado = Convert.ToString(atributos["textoAnulado"]);
            return cc;
        }

        public int Save(cCuentaCorriente cuentacorriente)
        {
            if (string.IsNullOrEmpty(cuentacorriente.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(cuentacorriente));
            else
                return cDataBase.GetInstance().UpdateObject(cuentacorriente.Id, GetTable, AttributesClass(cuentacorriente));
        }

        public List<cCuentaCorriente> GetCuentaCorriente(string _idEmpresa, Int16 _estado, string _obra, string _moneda)
        {
            List<cCuentaCorriente> cc = new List<cCuentaCorriente>();
            string query = "SELECT cc.id FROM " + GetTable + " cc INNER JOIN tEmpresaUnidad eu ON cc.idEmpresaUnidad = eu.id INNER JOIN tEmpresa e ON e.id = eu.idEmpresa WHERE ";
            
            if (!string.IsNullOrEmpty(_idEmpresa) || _idEmpresa == "0")
                query += " cc.idEmpresa = '" + _idEmpresa + "' AND";

            query += " total <> '0' AND";

            //if (_estado != 3)
                query += " cc.estado= '" + _estado + "'";
            //else
              //  query += " cc.estado<> '" + _estado + "'";

            if (!string.IsNullOrEmpty(_obra))
                query += " AND eu.idProyecto= '" + _obra + "'";

            if (!string.IsNullOrEmpty(_moneda) && _moneda != "-1") {
                if (_moneda == tipoMoneda.Dolar.ToString())
                    query += " AND cc.monedaAcordada= '" + 0 + "'";
                else
                    query += " AND cc.monedaAcordada= '" + 1 + "'";
            }

            query += " Order by e.Apellido, e.Nombre ASC";

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

        public cCuentaCorriente GetCuentaCorrienteById(string _idEmpresa)
        {
            string query = "SELECT id FROM tCuentaCorriente WHERE id = '" + _idEmpresa + "'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            return Load(Convert.ToString(id));
        }

        public cCuentaCorriente GetCuentaCorrienteByIdEmpresaUnidad(string _idEmpresaUnidad)
        {
            string query = "SELECT id FROM tCuentaCorriente WHERE idEmpresaUnidad = '" + _idEmpresaUnidad + "'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null) return null;
            return Load(Convert.ToString(id));
        }

        public List<cCuentaCorriente> GetCuentaCorrienteByIdCliente(string _idEmpresa, Int16 _estado)
        {
            List<cCuentaCorriente> cc = new List<cCuentaCorriente>();
            string query = "SELECT id FROM " + GetTable + " WHERE ";

            query += " idEmpresa = '" + _idEmpresa + "'";

            if (_estado != -1)
            {
                if (_estado != 2)
                    query += " AND estado= '" + _estado + "' Order by id ASC";
                else
                    query += " AND estado<> '" + _estado + "' Order by id ASC";
            }

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

        public DataTable GetTotalACobrarPorProyecto()
        {
            SqlCommand cmd = new SqlCommand();

            string query = "SELECT cc.idUnidad, SUM(c.monto) as cant FROM tCuentaCorriente cc INNER JOIN tEmpresa e ON cc.idEmpresa=e.id INNER JOIN tCuota c ON cc.id = c.idCuentaCorriente WHERE c.estado = 1 GROUP BY cc.idUnidad ORDER BY cant";
            cmd.CommandText = query.ToString();

            DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);
            if (dataSet == null) return null;

            return dataSet.Tables[0];
        }

        public DataTable GetTotalACobrarPorCliente()
        {
            SqlCommand cmd = new SqlCommand();

            string query = "SELECT e.id, SUM(c.monto) as cant FROM tCuentaCorriente cc INNER JOIN tEmpresa e ON cc.idEmpresa=e.id INNER JOIN tCuota c ON cc.id = c.idCuentaCorriente WHERE c.estado = 1 GROUP BY e.id ORDER BY cant";
            cmd.CommandText = query.ToString();

            DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);
            if (dataSet == null) return null;

            return dataSet.Tables[0];
        }

        public cCuentaCorriente GetCuentaCorrienteByIdOv(string _idOv)
        {
            string query = "SELECT id FROM tCuentaCorriente WHERE idOperacionVenta = '" + _idOv + "' AND estado=" + (Int16)estadoCuenta_Cuota.Activa;

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public cCuentaCorriente GetCCByIdUnidad(string _idUnidad, string _idEmpresaUnidad)
        {
            //string query = "SELECT id FROM tCuentaCorriente WHERE idEmpresaUnidad='" + _idEmpresaUnidad + "' AND estado='" + (Int16)estadoCuenta_Cuota.Activa + "'";

            string query = "SELECT c.id FROM tCuentaCorriente c INNER JOIN tOperacionVenta o ON c.idOperacionVenta=o.id INNER JOIN tEmpresaUnidad eu ON eu.idOv=o.id";
            query += " WHERE eu.id='" + _idEmpresaUnidad + "' AND c.estado='1' AND eu.papelera='1'";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public List<cCuentaCorriente> GetCuotasMesProyectoPendientes(string _idProyecto, Int16 _tipoMoneda)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            List<cCuentaCorriente> cuotas = new List<cCuentaCorriente>();

            string query = "SELECT cc.id FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa ";
            query += " INNER JOIN tOperacionVenta op ON op.id=eu.idOv  INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id ";
            query += " INNER JOIN tProyecto p ON p.id = eu.idProyecto INNER JOIN tCuentaCorriente cc ON cc.id = c.idCuentaCorriente ";
            //query += " WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND cc.estado='1' AND p.id='" + _idProyecto + "' AND fp.moneda = '" + _tipoMoneda + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Pendiente + "' GROUP BY c.id";
            query += " WHERE eu.papelera = '1' AND eu.idOv <> '-1' AND cc.estado='1' AND p.id='" + _idProyecto + "' AND c.estado='" + (Int16)estadoCuenta_Cuota.Pendiente + "' GROUP BY cc.id";

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

    }
}
