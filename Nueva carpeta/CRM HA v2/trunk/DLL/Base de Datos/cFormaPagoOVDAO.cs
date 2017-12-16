using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cFormaPagoOVDAO
    {
        public string GetTable
        { get { return "tFormaPagoOV"; } }

        public List<cAtributo> AttributesClass(cFormaPagoOV fp)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idOperacionVenta", fp.IdOperacionVenta));
            lista.Add(new cAtributo("moneda", fp.Moneda));
            lista.Add(new cAtributo("monto", fp.Monto));
            lista.Add(new cAtributo("saldo", fp.Saldo));
            lista.Add(new cAtributo("cantCuotas", fp.CantCuotas));
            lista.Add(new cAtributo("gastosAdtvo", fp.GastosAdtvo));
            lista.Add(new cAtributo("interesAnual", fp.InteresAnual));
            lista.Add(new cAtributo("rangoCuotaCAC", fp.RangoCuotaCAC));
            lista.Add(new cAtributo("valor", fp.Valor));
            lista.Add(new cAtributo("fechaVencimiento", fp.FechaVencimiento));
            lista.Add(new cAtributo("papelera", fp.Papelera));
            return lista;
        }

        public int Save(cFormaPagoOV fp)
        {
            if (string.IsNullOrEmpty(fp.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(fp));
            else
                return cDataBase.GetInstance().UpdateObject(fp.Id, GetTable, AttributesClass(fp));
        }

        public cFormaPagoOV Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cFormaPagoOV fp = new cFormaPagoOV();
            fp.Id = Convert.ToString(atributos["id"]);
            fp.IdOperacionVenta = Convert.ToString(atributos["idOperacionVenta"]);
            fp.Moneda = Convert.ToString(atributos["moneda"]);
            fp.Monto = Convert.ToDecimal(atributos["monto"]);
            fp.Saldo = Convert.ToDecimal(atributos["saldo"]);
            fp.CantCuotas = Convert.ToInt16(atributos["cantCuotas"]);
            fp.RangoCuotaCAC = Convert.ToString(atributos["rangoCuotaCAC"]);
            fp.GastosAdtvo = Convert.ToString(atributos["gastosAdtvo"]);
            fp.InteresAnual = Convert.ToDecimal(atributos["interesAnual"]);
            fp.Valor = Convert.ToDecimal(atributos["valor"]);
            fp.FechaVencimiento = Convert.ToDateTime(atributos["fechaVencimiento"]);
            fp.Papelera = Convert.ToInt16(atributos["papelera"]);
            return fp;
        }

        public cFormaPagoOV LoadByIdOV(string _idOV)
        {
            cFormaPagoOV formaPagoOV = new cFormaPagoOV();
            string query = "SELECT id FROM " + GetTable + " WHERE idOperacionVenta='" + _idOV + "'";
            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null) return null;
            formaPagoOV = Load(idList);
            return formaPagoOV;
        }

        public List<cFormaPagoOV> GetFormaPagoOVByIdOV(string _idOperacionVenta)
        {
            List<cFormaPagoOV> cc = new List<cFormaPagoOV>();
            string query = "SELECT id FROM " + GetTable + " WHERE idOperacionVenta='" + _idOperacionVenta + "' ORDER BY fechaVencimiento ASC";

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

        public List<cFormaPagoOV> GetFormaPagoOVByIdOVActivas(string _idOperacionVenta)
        {
            List<cFormaPagoOV> cc = new List<cFormaPagoOV>();
            string query = "SELECT fp.id FROM " + GetTable + " fp INNER JOIN tOperacionVenta op ON fp.idOperacionVenta=op.id WHERE fp.idOperacionVenta=" + _idOperacionVenta + " AND op.estado='" + (Int16)estadoOperacionVenta.Activo  + "' ORDER BY fp.fechaVencimiento ASC";

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

        public ArrayList LoadTableFormaPago(string _idEmpresa, string _idUnidad)
        {
            ArrayList formasPago = new ArrayList();
            string query = "SELECT f.id FROM tFormaPagoOV f INNER JOIN tOperacionVenta o ON f.idOperacionVenta = o.id INNER JOIN tEmpresaUnidad eu ON eu.idOv=o.id INNER JOIN tEmpresa e ON e.id=eu.idEmpresa ";
            query += " INNER JOIN tCuentaCorriente cc ON o.id = cc.idOperacionVenta WHERE e.id='" + _idEmpresa + "' AND eu.idUnidad = '" + _idUnidad + "' AND f.papelera ='" + (Int16)papelera.Activo + "' AND cc.estado='" + (Int16)estadoCuenta_Cuota.Activa + "' GROUP BY f.id";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                formasPago.Add(Load(Convert.ToString(idList[i])));
            }
            return formasPago;
        }       
    }
}
